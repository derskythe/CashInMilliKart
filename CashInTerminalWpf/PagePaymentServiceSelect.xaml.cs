using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PaymentServiceSelect.xaml
    /// </summary>
    public partial class PagePaymentServiceSelect
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private int _Page;
        private int _TotalPages;
        private const int MAX_ELEMENTS = 12;

        public PagePaymentServiceSelect()
        {
            InitializeComponent();
        }

        // ReSharper disable PossibleNullReferenceException
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            try
            {
                _TotalPages =
                    Convert.ToInt32(
                        Math.Round(Math.Ceiling(_FormMain.ClientInfo.PaymentCategory.Services.Length / (double)MAX_ELEMENTS)));
                CheckPageButtons();
                AddButtons();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
                _FormMain.OpenForm(FormEnum.OutOfOrder);
            }
        }
        // ReSharper restore PossibleNullReferenceException

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            if (_FormMain.PrevForm == FormEnum.OtherPaymentsCategories)
            {
                _FormMain.OpenForm(FormEnum.OtherPaymentsCategories);
            }
            else
            {
                _FormMain.OpenForm(FormEnum.MoneyMoversPaymentsCategories);
            }
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void AddButtons()
        {
            try
            {
                Grid.Children.Clear();

                int i = 0;
                int numToSkip = (_Page * MAX_ELEMENTS);
                foreach (var item in _FormMain.ClientInfo.PaymentCategory.Services)
                {
                    if (i < numToSkip)
                    {
                        i++;
                        continue;
                    }
                    if (i > (numToSkip + MAX_ELEMENTS - 1))
                    {
                        break;
                    }

                    string text;
                    if (!String.IsNullOrEmpty(item.LocalizedName.ValueAz))
                    {
                        text = item.LocalizedName.ValueAz;
                    }
                    else if (!String.IsNullOrEmpty(item.LocalizedName.ValueEn))
                    {
                        text = item.LocalizedName.ValueEn;
                    }
                    else
                    {
                        text = item.LocalizedName.ValueRu;
                    }
                    Log.Debug(text);

                    var button = new Button
                    {
                        Style = FindResource("PaymentServiceButtonStyle") as Style,
                        Name = "Button_" + item.Name + item.Id.ToString(CultureInfo.InvariantCulture),
                        Tag = item
                    };
                    button.Click += ButtonOnClick;

                    var textField = new TextBlock
                        {
                            Style = FindResource("MainTextBlockStyle") as Style,
                            TextWrapping = TextWrapping.Wrap,
                            Text = text
                        };

                    button.Content = textField;
                    Grid.Children.Add(button);
                    i++;
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var button = (Button)sender;
                Log.Info(String.Format("Selected: {0}", button.Name));
                var tag = (PaymentService)button.Tag;

                switch (tag.AssemblyId)
                {
                    case "PayPoint":
                        _FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.Komtek;
                        break;

                    case "GoldenPay":
                        _FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.GoldenPay;
                        break;

                    case "MoneyMovers":
                        _FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.MoneyMovers;
                        break;

                    default:
                        _FormMain.ClientInfo.PaymentOperationType = PaymentOperationType.Unknown;
                        break;
                }
                //_FormMain.ClientInfo.PaymentOperationType = tag.AssemblyId == "PayPoint"
                //                                                ? PaymentOperationType.Komtek
                //                                                : PaymentOperationType.GoldenPay;

                _FormMain.ClientInfo.PaymentService = tag;
                _FormMain.OpenForm(FormEnum.PaymentServiceInputData);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonPrevClick(object sender, RoutedEventArgs e)
        {
            _Page--;
            CheckPageButtons();
            AddButtons();
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            _Page++;
            CheckPageButtons();
            AddButtons();
        }

        private void CheckPageButtons()
        {
            if (_TotalPages == 1)
            {
                ButtonNext.Visibility = Visibility.Collapsed;
                ButtonPrev.Visibility = Visibility.Collapsed;
                return;
            }

            ButtonNext.Visibility = (_Page + 1) >= _TotalPages ? Visibility.Collapsed : Visibility.Visible;
            ButtonPrev.Visibility = _Page == 0 ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
