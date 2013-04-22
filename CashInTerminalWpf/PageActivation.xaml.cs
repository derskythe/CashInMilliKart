using System;
using System.Windows;
using System.Windows.Forms;
using CashInTerminal.Enums;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using Containers.Enums;
using NLog;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.Forms.MessageBox;
using ResultCodes = CashInTerminalWpf.CashIn.ResultCodes;
using TextBox = System.Windows.Controls.TextBox;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for PageActivation.xaml
    /// </summary>
    public partial class PageActivation
    {
        private TextBox _Selected;
        private MainWindow _FormMain;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public PageActivation()
        {
            InitializeComponent();            
        }

        private void ButtonTestClick(object sender, RoutedEventArgs e)
        {
            int port = 0;
            if (CmbPorts.SelectedIndex >= 0)
            {
                port = Convert.ToInt32(Convert.ToString(CmbPorts.SelectedItem).Substring(3));
            }

            try
            {
                _FormMain.InitCashCode(port);
                _FormMain.CcnetDevice.Reset();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
                MessageBox.Show(exp.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            if (TxtActivationCode.Text.Length <= 0 || TxtTerminal.Text.Length <= 0)
            {
                return;
            }

            ButtonNext.IsEnabled = false;

            String errorMessage;
            try
            {
                var result = _FormMain.Server.InitTerminal(Convert.ToInt32(TxtTerminal.Text),
                                                          TxtActivationCode.Text, Settings.Default.PublicKey);
                if (result == null)
                {
                    throw new NullReferenceException("Server response is null");
                }

                if (result.ResultCodes == (ResultCodes)Containers.Enums.ResultCodes.Ok)
                {
                    int port = 0;
                    if (CmbPorts.SelectedIndex >= 0)
                    {
                        port = Convert.ToInt32(Convert.ToString(CmbPorts.SelectedItem).Substring(3));
                    }

                    Settings.Default.DevicePort = port;
                    Log.Info("Success activation!");
                    Settings.Default.TerminalCode = TxtTerminal.Text;
                    Settings.Default.Save();

                    _FormMain.AuthTerminal = true;
                    _FormMain.TerminalStatus = TerminalCodes.Ok;
                    ButtonNext.IsEnabled = true;

                    try
                    {
                        GetTerminalInfo();
                        _FormMain.InitCashCode(Settings.Default.DevicePort);
                        _FormMain.StartTimers();
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                    }

                    _FormMain.OpenForm(FormEnum.Products);

                    return;
                }

                errorMessage = String.Format("ErrorCode: {0}, Description: {1}", result.ResultCodes, result.Description);
            }
            catch (Exception exp)
            {
                errorMessage = exp.Message;
                Log.ErrorException(exp.Message, exp);
            }

            ButtonNext.IsEnabled = true;

            MessageBox.Show(errorMessage);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info(Title);
            _FormMain = (MainWindow)Window.GetWindow(this);

            ControlNumPad.AddHandler(NumPadControl.NewCharEvent, new NumPadControl.NewCharEventHandler(ControlNumPadOnNewChar));
            ControlNumPad.AddHandler(NumPadControl.BackspaceEvent, new NumPadControl.BackspaceEventHandler(ControlNumPadOnBackSpace));
            ControlNumPad.AddHandler(NumPadControl.ClearAllEvent, new NumPadControl.ClearAllEventHandler(ControlNumPadOnClearAll));

            _Selected = TxtTerminal;

            try
            {
                GetComList(CmbPorts);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void GetComList(ComboBox comboBoxCom)
        {
            string[] comList = System.IO.Ports.SerialPort.GetPortNames();
            Array.Sort(comList);

            foreach (string s in comList)
            {
                var regexp = new System.Text.RegularExpressions.Regex("^[A-Z]+[0-9]+");
                string str = regexp.Match(s).Value;

                comboBoxCom.Items.Add(str);
            }

            if (comboBoxCom.SelectedItem == null || comboBoxCom.SelectedIndex == -1)
            {
                comboBoxCom.SelectedIndex = 0;
            }
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {

        }

        private void ControlNumPadOnClearAll(object sender, NumPadRoutedEventArgs numPadRoutedEventArgs)
        {
            _Selected.Text = String.Empty;
        }

        private void ControlNumPadOnBackSpace(object sender, NumPadRoutedEventArgs args)
        {
            if (_Selected.Text.Length > 0)
            {
                _Selected.Text = _Selected.Text.Substring(0, _Selected.Text.Length - 1);
            }
        }

        private void ControlNumPadOnNewChar(object sender, NumPadRoutedEventArgs args)
        {
            _Selected.Text += args.NewChar;
        }

        private void TxtTerminalGotFocus(object sender, RoutedEventArgs e)
        {
            _Selected = (TextBox)sender;
        }

        private void TxtActivationCodeGotFocus(object sender, RoutedEventArgs e)
        {
            _Selected = (TextBox)sender;
        }

        private void GetTerminalInfo()
        {
            DateTime now = DateTime.Now;
            var cmd = new StandardRequest
            {
                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                SystemTime = now,
                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _FormMain.ServerPublicKey)
            };
            var response = _FormMain.Server.GetTerminalInfo(cmd);

            if (response != null)
            {
                _FormMain.TerminalInfo = response.Terminal;
            }
        }
    }
}
