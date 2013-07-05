using System;
using System.Text.RegularExpressions;
using System.Windows;
using CashInTerminalWpf.Enums;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageCreditRequest.xaml
    /// </summary>
    public partial class PageCreditRequest
    {
        private MainWindow _FormMain;
        private const String LABEL = "(___) ___-__-__";
        private readonly Regex _Number = new Regex(@"^\(\d{3}\)\s{1}\d{3}\-\d{2}\-\d{2}$");
        private int _I;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageCreditRequest()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            ClientNumber.Text = LABEL;
            ControlNumPad.AddHandler(NumPadControl.NewCharEvent, new NumPadControl.NewCharEventHandler(ControlNumPadOnNewChar));
            ControlNumPad.AddHandler(NumPadControl.BackspaceEvent, new NumPadControl.BackspaceEventHandler(ControlNumPadOnBackSpace));
            ControlNumPad.AddHandler(NumPadControl.ClearAllEvent, new NumPadControl.ClearAllEventHandler(ControlNumPadOnClearAll));
        }

        private void ControlNumPadOnClearAll(object sender, NumPadRoutedEventArgs numPadRoutedEventArgs)
        {
            ClientNumber.Text = LABEL;
            _I = 0;
            ButtonNext.IsEnabled = false;
        }

        private void ControlNumPadOnBackSpace(object sender, NumPadRoutedEventArgs args)
        {
            try
            {
                while (!CheckChar(GetChar(_I)))
                {
                    _I--;
                }

                if (_I <= 0)
                {
                    _I = 0;
                    return;
                }
                
                Log.Debug(_I);
                var tmp = ClientNumber.Text.ToCharArray();

                if (_I > (tmp.Length - 1))
                {
                    _I = tmp.Length - 1;
                }
                tmp[_I] = '_';
                ClientNumber.Text = String.Concat(tmp);

                if (_I > 0)
                {
                    _I--;
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            ButtonNext.IsEnabled = false;
            //if (ClientNumber.Text.Length > 0)
            //{
            //    ClientNumber.Value = ClientNumber.Text.Substring(0, ClientNumber.Text.Length - 1);
            //}
        }

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            try
            {
                if (_I >= LABEL.Length)
                {
                    return;
                }

                while (!CheckChar(GetChar(_I)))
                {
                    _I++;
                }

                Log.Debug(_I);
                var tmp = ClientNumber.Text.ToCharArray();
                tmp[_I] = Convert.ToChar(args.NewChar);
                ClientNumber.Text = String.Concat(tmp);

                _I++;                
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            //Log.Debug(ClientNumber.Value + " " + ClientNumber.Text);
            //ClientNumber.Value += args.NewChar;

            if (_Number.IsMatch(ClientNumber.Text))
            {
                ButtonNext.IsEnabled = true;
            }
        }

        private bool CheckChar(char currentChar)
        {
            if (currentChar != ' ' && currentChar != '(' && currentChar != ')' && currentChar != '-')
            {
                return true;
            }

            return false;
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonHomeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _FormMain.OpenForm(FormEnum.Products);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _FormMain.OpenForm(FormEnum.Products);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            if (_Number.IsMatch(ClientNumber.Text))
            {
                _FormMain.ClientInfo.AccountNumber = ClientNumber.Text;
                _FormMain.OpenForm(FormEnum.CreditRequestAccept);
            }            
        }

        private char GetChar(int pos)
        {            
            if (pos < ClientNumber.Text.Length && pos >= 0)
            {
                return ClientNumber.Text[_I];
            }

            return '_';
        }
    }
}
