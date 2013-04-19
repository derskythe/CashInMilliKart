using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageDebitSelectAccount.xaml
    /// </summary>
    public partial class PageDebitSelectAccount
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageDebitSelectAccount()
        {
            InitializeComponent();
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            switch (_FormMain.ClientInfo.PaymentOperationType)
            {
                case PaymentOperationType.DebitPaymentByClientCode:
                    _FormMain.OpenForm(FormEnum.ClientCode);
                    break;

                case PaymentOperationType.DebitPaymentByPassportAndAccount:
                    _FormMain.OpenForm(FormEnum.ClientByPassport);
                    break;

                default:
                    _FormMain.OpenForm(FormEnum.Products);
                    break;
            }
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Name);
            _FormMain = (MainWindow)Window.GetWindow(this);

            try
            {
                if (_FormMain.Clients != null && _FormMain.Clients.Length > 0)
                {
                    AddButtons();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void AddButtons()
        {
            lock (_FormMain.Clients)
            {
                Grid.Children.Clear();

                try
                {
                    foreach (var tag in _FormMain.Clients)
                    {
                        string text = tag.ClientAccount + @" " + tag.CreditName + " " +
                                      tag.BeginDate.ToString("MM.yyyy") + " " + tag.CreditAmount + " " +
                                      tag.Currency;
                        Log.Debug(text);

                        var button = new Button
                        {
                            Style = FindResource("MenuButtonStyle") as Style,
                            Name = "Button" + tag.ClientAccount + tag.CreditAmount,
                            Tag = tag,
                            Content = text
                        };
                        button.Click += ButtonOnClick;

                        Grid.Children.Add(button);
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
            }
        }

        private void ButtonOnClick(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                var button = (Button)sender;
                Log.Info(button.Name);
                _FormMain.ClientInfo.Client = (CashIn.ClientInfo)button.Tag;
                _FormMain.OpenForm(FormEnum.DebitClientInfo);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }
    }
}
