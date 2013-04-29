using System;
using System.Windows;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageServiceNotAvailable.xaml
    /// </summary>
    public partial class PageServiceNotAvailable
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageServiceNotAvailable()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (_FormMain.ClientInfo.PaymentOperationType)
                {

                    case PaymentOperationType.CreditPaymentByPassportAndAccount:
                    case PaymentOperationType.CreditPaymentByClientCode:
                        _FormMain.OpenForm(FormEnum.CreditPaymentTypeSelect);
                        break;

                    case PaymentOperationType.DebitPaymentByPassportAndAccount:
                    case PaymentOperationType.DebitPaymentByClientCode:                    
                        _FormMain.OpenForm(FormEnum.DebitPaymentTypeSelect);
                        break;

                    case PaymentOperationType.GoldenPay:
                    case PaymentOperationType.Komtek:
                        _FormMain.OpenForm(FormEnum.OtherPaymentsCategories);
                        break;

                    case PaymentOperationType.CreditPaymentBolcard:
                        _FormMain.OpenForm(FormEnum.CreditPaymentTypeSelect);
                        break;

                    default:
                        _FormMain.OpenForm(FormEnum.Products);
                        break;
                }

                return;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            _FormMain.OpenForm(FormEnum.Products);
        }
    }
}
