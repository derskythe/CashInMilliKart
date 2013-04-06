using System;
using System.Windows.Forms;
using CashInTerminal.BaseForms;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;
using Containers.Enums;
using ResultCodes = CashInTerminal.CashIn.ResultCodes;

namespace CashInTerminal
{
    public partial class FormActivation : FormMdiChild
    {
        private TextBox _SelectedBox;

        private TextBox SelectedBox
        {
            get { return _SelectedBox ?? (_SelectedBox = txtActivationTerminal); }
            set { _SelectedBox = value; }
        }

        public FormActivation()
        {
            InitializeComponent();
        }

        private void BtnActivationClearClick(object sender, EventArgs e)
        {
            if (SelectedBox.Text.Length > 0)
            {
                SelectedBox.Text = SelectedBox.Text.Substring(0, SelectedBox.Text.Length - 1);
            }
        }

        private void BtnActivationBackspaceClick(object sender, EventArgs e)
        {
            SelectedBox.Text += String.Empty;
        }

        private void BtnActivation0Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"0";
        }

        private void BtnActivation1Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"1";
        }

        private void BtnActivation2Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"2";
        }

        private void BtnActivation3Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"3";
        }

        private void BtnActivation4Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"4";
        }

        private void BtnActivation5Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"5";
        }

        private void BtnActivation6Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"6";
        }

        private void BtnActivation7Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"7";
        }

        private void BtnActivation8Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"8";
        }

        private void BtnActivation9Click(object sender, EventArgs e)
        {
            SelectedBox.Text += @"9";
        }

        private void BtnActivationClick(object sender, EventArgs e)
        {
            if (txtActivationCode.Text.Length <= 0 || txtActivationTerminal.Text.Length <= 0)
            {
                return;
            }

            btnActivation.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            String errorMessage;
            try
            {
                var result = FormMain.Server.InitTerminal(Convert.ToInt32(txtActivationTerminal.Text),
                                                          txtActivationCode.Text, Settings.Default.PublicKey);

                if (result == null)
                {
                    throw new NullReferenceException("Server response is null");
                }

                //CheckSignature(result);

                if (result.ResultCodes == ResultCodes.Ok)
                {
                    int port = 0;
                    if (cmbPorts.SelectedIndex >= 0)
                    {
                        port = Convert.ToInt32(Convert.ToString(cmbPorts.SelectedItem).Substring(3));
                    }

                    Settings.Default.DevicePort = port;
                    Log.Info("Success activation!");
                    Settings.Default.TerminalCode = txtActivationTerminal.Text;
                    Settings.Default.Save();

                    FormMain.AuthTerminal = true;
                    FormMain.TerminalStatus = TerminalCodes.Ok;
                    btnActivation.Enabled = true;
                    Cursor.Current = Cursors.Default;

                    try
                    {
                        GetTerminalInfo();
                        FormMain.InitCashCode(Settings.Default.DevicePort);
                        FormMain.StartTimers();
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                    }

                    ChangeView(typeof(FormProducts));

                    return;
                }

                errorMessage = String.Format("ErrorCode: {0}, Description: {1}", result.ResultCodes, result.Description);
            }
            catch (Exception exp)
            {
                errorMessage = exp.Message;
                Log.ErrorException(exp.Message, exp);
            }

            btnActivation.Enabled = true;
            Cursor.Current = Cursors.Default;

            MessageBox.Show(errorMessage);
        }

        private void TxtActivationTerminalClick(object sender, EventArgs e)
        {
            SelectedBox = txtActivationTerminal;
        }

        private void TxtActivationCodeClick(object sender, EventArgs e)
        {
            SelectedBox = txtActivationCode;
        }

        private void GetTerminalInfo()
        {
            DateTime now = DateTime.Now;
            var cmd = new StandardRequest
            {
                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                SystemTime = now,
                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, FormMain.ServerPublicKey)
            };
            var response = FormMain.Server.GetTerminalInfo(cmd);

            if (response != null)
            {
                FormMain.TerminalInfo = response.Terminal;
            }
        }

        private void FormActivationLoad(object sender, EventArgs e)
        {
            HomeButton = false;
            try
            {
                GetComList(cmbPorts);
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

        private void BtnTestClick(object sender, EventArgs e)
        {
            int port = 0;
            if (cmbPorts.SelectedIndex >= 0)
            {
                port = Convert.ToInt32(Convert.ToString(cmbPorts.SelectedItem).Substring(3));
            }

            try
            {
                FormMain.InitCashCode(port);
                FormMain.CcnetDevice.Reset();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);

                MessageBox.Show(exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
