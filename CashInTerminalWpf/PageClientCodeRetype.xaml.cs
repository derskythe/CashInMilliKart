using System;
using System.Windows;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageClientCodeRetype.xaml
    /// </summary>
    public partial class PageClientCodeRetype
    {
        private MainWindow FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageClientCodeRetype()
        {
            InitializeComponent();            
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormEnum.CreditPaymentTypeSelect);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            if (ClientNumber.Text.Length > 4)
            {
                if (ClientNumber.Text != FormMain.ClientInfo.AccountNumber)
                {
                    FormMain.OpenForm(FormEnum.InvalidNumber);
                    return;
                }

                try
                {
                    DateTime now = DateTime.Now;
                    var request = new GetClientInfoRequest
                    {
                        PaymentOperationType = (int)FormMain.ClientInfo.PaymentOperationType,
                        ClientCode = FormMain.ClientInfo.AccountNumber,
                        SystemTime = now,
                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, FormMain.ServerPublicKey)
                    };

                    FormMain.InfoRequest = request;
                    FormMain.OpenForm(FormEnum.Progress);
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                    FormMain.OpenForm(FormEnum.OutOfOrder);
                }
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Name);
            FormMain = (MainWindow)Window.GetWindow(this);

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
            if (ClientNumber.Text.Length > 0)
            {
                ClientNumber.Text = ClientNumber.Text.Substring(0, ClientNumber.Text.Length - 1);
            }
        }

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            ClientNumber.Text += args.NewChar;
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormEnum.Products);
        }
    }
}
