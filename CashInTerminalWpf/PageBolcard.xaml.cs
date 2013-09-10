using System;
using System.Windows;
using System.Windows.Controls;
using CashInTerminalWpf.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageBolcard.xaml
    /// </summary>
    public partial class PageBolcard
    {
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageBolcard()
        {
            InitializeComponent();
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.CreditPaymentTypeSelect);
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientNumber1.Text.Length + ClientNumber2.Text.Length + ClientNumber3.Text.Length + ClientNumber4.Text.Length == 16)
                {
                    var result = ClientNumber1.Text + "****" + ClientNumber3.Text + ClientNumber4.Text;
                    Log.Info("Input value: " + result);
                    _FormMain.ClientInfo.AccountNumber = result;
                    _FormMain.OpenForm(FormEnum.BolCardRetype);
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
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

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            try
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
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ControlNumPadOnClearAll(object sender, NumPadRoutedEventArgs numPadRoutedEventArgs)
        {
            try
            {
                ClientNumber1.Text = String.Empty;
                ClientNumber2.Text = String.Empty;
                ClientNumber3.Text = String.Empty;
                ClientNumber4.Text = String.Empty;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ControlNumPadOnBackSpace(object sender, NumPadRoutedEventArgs args)
        {
            try
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
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            _FormMain.OpenForm(FormEnum.Products);
        }
    }
}
