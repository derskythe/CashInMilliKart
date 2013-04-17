using System;
using System.Windows;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.CustomControls;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using Containers.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageClientByPassportRetype.xaml
    /// </summary>
    public partial class PageClientByPassportRetype
    {
        private readonly MainWindow FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageClientByPassportRetype()
        {
            InitializeComponent();
            FormMain = (MainWindow)Window.GetWindow(this);
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormMain.ClientInfo.PaymentOperationType == PaymentOperationType.DebitPaymentByPassportAndAccount
                           ? FormEnum.DebitPaymentTypeSelect
                           : FormEnum.CreditPaymentTypeSelect);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            if (ClientNumber.Text.Length > 4)
            {
                FormMain.ClientInfo.Passport = ClientNumber.Text;

                try
                {
                    var now = DateTime.Now;
                    var request = new GetClientInfoRequest
                    {
                        PaymentOperationType = (int)FormMain.ClientInfo.PaymentOperationType,
                        CreditAccount = FormMain.ClientInfo.AccountNumber,
                        PasportNumber = FormMain.ClientInfo.Passport,
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
            ControlNumPad.AddHandler(NumPadControl.NewCharEvent, new NumPadControl.NewCharEventHandler(ControlNumPadOnNewChar));
            ControlNumPad.AddHandler(NumPadControl.BackspaceEvent, new NumPadControl.BackspaceEventHandler(ControlNumPadOnBackSpace));
            ControlNumPad.AddHandler(NumPadControl.ClearAllEvent, new NumPadControl.ClearAllEventHandler(ControlNumPadOnClearAll));
            ControlAlphabet.AddHandler(AlphabetControl.NewCharEvent, new AlphabetControl.NewCharEventHandler(ControlNumPadOnNewChar));
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            FormMain.OpenForm(FormEnum.Products);
        }

        private void ControlNumPadOnNewChar(object sender, AlphabetPadRoutedEventArgs args)
        {
            ClientNumber.Text += args.NewChar;
        }

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            ClientNumber.Text += args.NewChar;
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
    }
}
