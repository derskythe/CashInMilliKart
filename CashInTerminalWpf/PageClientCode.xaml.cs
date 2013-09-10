using System;
using System.Windows;
using CashInTerminalWpf.Enums;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageBase.xaml
    /// </summary>
    public partial class PageBase
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageBase()
        {
            InitializeComponent();            
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            ControlNumPad.AddHandler(NumPadControl.NewCharEvent, new NumPadControl.NewCharEventHandler(ControlNumPadOnNewChar));
            ControlNumPad.AddHandler(NumPadControl.BackspaceEvent, new NumPadControl.BackspaceEventHandler(ControlNumPadOnBackSpace));
            ControlNumPad.AddHandler(NumPadControl.ClearAllEvent, new NumPadControl.ClearAllEventHandler(ControlNumPadOnClearAll));
        }

        private void ControlNumPadOnClearAll(object sender, NumPadRoutedEventArgs numPadRoutedEventArgs)
        {
            ClientNumber.Text = String.Empty;
        }

        private void ControlNumPadOnBackSpace(object sender, NumPadRoutedEventArgs args)
        {
            try
            {
                if (ClientNumber.Text.Length > 0)
                {
                    ClientNumber.Text = ClientNumber.Text.Substring(0, ClientNumber.Text.Length - 1);
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            ClientNumber.Text += args.NewChar;
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

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            if (ClientNumber.Text.Length > 4)
            {
                Log.Info("Input value: " + ClientNumber.Text);
                _FormMain.ClientInfo.AccountNumber = ClientNumber.Text;
                _FormMain.OpenForm(FormEnum.ClientCodeRetype);
            }
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
