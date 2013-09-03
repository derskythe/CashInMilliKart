using System;
using System.Windows;
using System.Windows.Controls;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageBolcardRetype.xaml
    /// </summary>
    public partial class PageBolcardRetype
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageBolcardRetype()
        {
            InitializeComponent();
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.CreditPaymentTypeSelect);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            if (ClientNumber1.Text.Length + ClientNumber2.Text.Length + ClientNumber3.Text.Length + ClientNumber4.Text.Length == 16)
            {
                var result = ClientNumber1.Text + "%" + ClientNumber3.Text + ClientNumber4.Text;
                Log.Info("Input value: " + result);
                if (result != _FormMain.ClientInfo.AccountNumber)
                {
                    _FormMain.OpenForm(FormEnum.InvalidNumber);
                    return;
                }
                try
                {
                    var now = DateTime.Now;
                    var request = new GetClientInfoRequest
                    {
                        PaymentOperationType = (int)_FormMain.ClientInfo.PaymentOperationType,
                        Bolcard8Digits = _FormMain.ClientInfo.AccountNumber,
                        SystemTime = now,
                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _FormMain.ServerPublicKey),
                        Ticks = now.Ticks
                    };

                    _FormMain.LongRequestType = LongRequestType.CreditDebitInfo;
                    _FormMain.InfoRequest = request;
                    _FormMain.OpenForm(FormEnum.Progress);
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                    _FormMain.OpenForm(FormEnum.OutOfOrder);
                }
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);
            ControlNumPad.AddHandler(NumPadControl.NewCharEvent, new NumPadControl.NewCharEventHandler(ControlNumPadOnNewChar));
            ControlNumPad.AddHandler(NumPadControl.BackspaceEvent, new NumPadControl.BackspaceEventHandler(ControlNumPadOnBackSpace));
            ControlNumPad.AddHandler(NumPadControl.ClearAllEvent, new NumPadControl.ClearAllEventHandler(ControlNumPadOnClearAll));
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {

        }

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            if (ClientNumber1.Text.Length < 4)
            {
                ClientNumber1.Text += args.NewChar;
            }
            else if (ClientNumber2.Text.Length < 4)
            {
                ClientNumber2.Text += args.NewChar;
            }
            else if (ClientNumber3.Text.Length < 4)
            {
                ClientNumber3.Text += args.NewChar;
            }
            else if (ClientNumber4.Text.Length < 4)
            {
                ClientNumber4.Text += args.NewChar;
            }
        }

        private void ControlNumPadOnClearAll(object sender, NumPadRoutedEventArgs numPadRoutedEventArgs)
        {
            ClientNumber1.Text = String.Empty;
            ClientNumber2.Text = String.Empty;
            ClientNumber3.Text = String.Empty;
            ClientNumber4.Text = String.Empty;
        }

        private void ControlNumPadOnBackSpace(object sender, NumPadRoutedEventArgs args)
        {
            TextBox current;
            if (ClientNumber4.Text.Length > 0)
            {
                current = ClientNumber4;
            }
            else if (ClientNumber3.Text.Length > 0)
            {
                current = ClientNumber3;
            }
            else if (ClientNumber2.Text.Length > 0)
            {
                current = ClientNumber2;
            }
            else
            {
                current = ClientNumber1;
            }

            current.Text = current.Text.Substring(0, current.Text.Length - 1);
        }
    }
}
