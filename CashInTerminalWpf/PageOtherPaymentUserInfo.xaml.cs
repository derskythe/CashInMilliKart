using System;
using System.Windows;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageOtherPayment.xaml
    /// </summary>
    public partial class PageOtherPaymentUserInfo
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageOtherPaymentUserInfo()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            try
            {
                if (_FormMain == null)
                {
                    Label1.Content = String.Empty;
                    Label2.Content = String.Empty;
                    Label3.Content = String.Empty;
                    Label4.Content = String.Empty;
                    Label3.Visibility = Visibility.Collapsed;
                    Label4.Visibility = Visibility.Collapsed;
                }
                else
                {
                    string text;
                    if (!String.IsNullOrEmpty(_FormMain.ClientInfo.PaymentService.LocalizedName.ValueAz))
                    {
                        text = _FormMain.ClientInfo.PaymentService.LocalizedName.ValueAz;
                    }
                    else if (!String.IsNullOrEmpty(_FormMain.ClientInfo.PaymentService.LocalizedName.ValueEn))
                    {
                        text = _FormMain.ClientInfo.PaymentService.LocalizedName.ValueEn;
                    }
                    else
                    {
                        text = _FormMain.ClientInfo.PaymentService.LocalizedName.ValueRu;
                    }
                    Label1.Content = Properties.Resources.ServiceName + ": " + text;
                    Label2.Content = Properties.Resources.AbonentCode + ": " + _FormMain.ClientInfo.Client.ClientAccount;

                    var response = (PaymentServiceInfoResponse)_FormMain.InfoResponse;

                    if (response != null && !String.IsNullOrEmpty(response.Person))
                    {
                        Label3.Content = Properties.Resources.AbonentName + ": " + response.Person;
                        Label4.Content = Properties.Resources.AbonentDebt + ": " + response.Debt + " AZN";
                    }
                    else
                    {
                        Label3.Content = String.Empty;
                        Label4.Content = String.Empty;
                    }
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.PaymentServiceInputData);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            _FormMain.ClientInfo.CurrentCurrency = _FormMain.ClientInfo.Client.Currency = "AZN";
            _FormMain.OpenForm(FormEnum.MoneyInput);
        }
    }
}
