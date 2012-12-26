using System;
using System.Threading;
using System.Windows.Forms;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;
using NLog;
using Org.BouncyCastle.Crypto;
using crypto;

namespace CashInTerminal
{
    public partial class FormMain : Form
    {
        #region Fields

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private AsymmetricCipherKeyPair _Keys;
        private CCNETDevice _CcnetDevice;
        private bool _Init;
        private bool _AuthTerminal;
        private bool _EncashmentMode;
        private LocalDb _Db;
        private bool _Running = true;
        private delegate void PanelShowDelegate(object item);
        private delegate void PanelHideDelegate(object item);

        private delegate void SetStackedAmountDelegate(object item);

        private int _SelectedProduct;
        private CashInServer _Server;
        private TextBox _PanelActivationFocus;
        private TextBox _PanelClientCodeFocus;
        private long _PaymentId;
        private String _CurrentCurrency;
        private String _SelectedPanel;

        #endregion

        #region Form Load

        public FormMain()
        {
            InitializeComponent();
            Log.Info("Started");
        }

        private void FormMainLoad(object sender, EventArgs e)
        {
            ChangePannel(pnlOutOfOrder);

            _Init = true;
            // Init keys
            Log.Info("Init keys");
            try
            {
                if (string.IsNullOrEmpty(Settings.Default.PrivateKey))
                {
                    // Make keys
                    var keys = Wrapper.SaveToString(Wrapper.GenerateKeys(1024));
                    Settings.Default.PrivateKey = keys[0];
                    Settings.Default.PublicKey = keys[1];

                    Settings.Default.Save();
                }

                _Keys = Wrapper.GetKeys(Settings.Default.PrivateKey, Settings.Default.PublicKey);

                _Init &= true;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
                _Init &= false;
            }

            // Init cashcode
            Log.Info("Init Cashcode");
            try
            {
                _CcnetDevice = new CCNETDevice();
                _CcnetDevice.Open(Settings.Default.DevicePort, CCNETPortSpeed.B_9600);
                _CcnetDevice.Init();
                _CcnetDevice.BillStacked += CcnetDeviceBillStacked;
                _CcnetDevice.ReadCommand += new CCNETDevice.ReadCommandDelegate(CcnetDeviceReadCommand);

                _Init &= true;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
                _Init &= false;
            }

            // Init DB
            Log.Info("Init Db");
            try
            {
                _Db = new LocalDb();
                _Db.CountCasseteBanknotes();
            }
            catch (Exception exp)
            {
                _Init &= false;
                Log.ErrorException(exp.Message, exp);
            }

            Log.Info("Init server connection");
            try
            {
                _Server = new CashInServerClient();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            _AuthTerminal = !String.IsNullOrEmpty(Settings.Default.TerminalCode);

            _PingThread = new Thread(PingThread);
            _PingThread.Start();

            _SendPaymentThread = new Thread(SendPaymentThread);
            _SendPaymentThread.Start();

            if (_Init && !_AuthTerminal)
            {
                ChangePannel(pnlActivation);
            }
            else if (!_Init)
            {
                ChangePannel(pnlOutOfOrder);
            }
        }



        #endregion

        private void SetStackedAmount(object amount)
        {
            if (lblMoneyTotal.InvokeRequired)
            {
                lblMoneyTotal.Invoke(new SetStackedAmountDelegate(SetStackedAmount), amount);
            }
            else
            {
                lblMoneyTotal.Text = amount.ToString();
            }
        }

        #region Panels

        private void PanelShow(object item)
        {
            var panel = (Panel)item;
            if (panel.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                panel.Invoke(new PanelShowDelegate(PanelShow), item);
            }
            else
            {
                panel.Visible = true;
                panel.Dock = DockStyle.Fill;
            }
        }

        private void PanelHide(object item)
        {
            var panel = (Panel)item;
            if (panel.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                panel.Invoke(new PanelHideDelegate(PanelHide), item);
            }
            else
            {
                panel.Visible = false;
            }
        }

        private void ChangePannel(Panel currentPannel)
        {
            foreach (Control childControl in Controls)
            {
                if (childControl is Panel)
                {
                    if (childControl == currentPannel)
                    {
                        _SelectedPanel = currentPannel.Name;
                        PanelShow(childControl);
                    }
                    else
                    {
                        PanelHide(childControl);
                    }
                }
            }
        }

        #endregion

        private void BtnRussianClick(object sender, EventArgs e)
        {
            ChangePannel(pnlProducts);
        }

        private void BtnAzeriClick(object sender, EventArgs e)
        {
            ChangePannel(pnlProducts);
        }

        private void BtnEnglishClick(object sender, EventArgs e)
        {
            ChangePannel(pnlProducts);
        }

        private void CleanPrevUserData()
        {
            txtClientCodeClient.Text = String.Empty;
            txtClientCodePassport.Text = String.Empty;
        }

        private void BtnPayCreditClick(object sender, EventArgs e)
        {
            _SelectedProduct = 1;
            CleanPrevUserData();
            ChangePannel(pnlClientCode);
        }

        private void BtnPayDebitClick(object sender, EventArgs e)
        {
            _SelectedProduct = 2;
            CleanPrevUserData();
            ChangePannel(pnlClientCode);
        }

        private void BtnClientCodeBackClick(object sender, EventArgs e)
        {
            _SelectedProduct = 0;
            ChangePannel(pnlProducts);
        }

        private void BtnClientCodeNextClick(object sender, EventArgs e)
        {
            if (txtClientCodeClient.Text.Length == 0 || txtClientCodePassport.Text.Length == 0)
            {
                return;
            }

            switch (_SelectedProduct)
            {
                case 1:
                    lblCreditInfoFullname.Text = "TestFirstName TestLastName TestMiddleName";
                    lblCreditInfoAccountNumber.Text = txtClientCodeClient.Text;
                    lblCreditInfoPassport.Text = txtClientCodePassport.Text;
                    lblCreditInfoProductName.Text = "Test product";
                    lblCreditInfoCurrentAmount.Text = "1000 AZN";
                    lblCreditInfoTotalAmount.Text = "2000 AZN";
                    lblCreditInfoDate.Text = DateTime.Now.AddMonths(6).ToLongDateString();
                    ChangePannel(pnlCreditInfo);
                    break;

                case 2:
                    lblDebitInfoFullname.Text = "TestFirstName TestLastName TestMiddleName";
                    lblDebitInfoAccount.Text = txtClientCodeClient.Text;
                    lblDebitInfoPassport.Text = txtClientCodePassport.Text;
                    lblDebitInfoDate.Text = DateTime.Now.AddMonths(-6).ToLongDateString();
                    ChangePannel(pnlDebitInfo);
                    break;
            }
        }

        private void BtnClientCodeBackspaceClick(object sender, EventArgs e)
        {
            if (_PanelClientCodeFocus.Text.Length > 0)
            {
                _PanelClientCodeFocus.Text = _PanelClientCodeFocus.Text.Substring(0, _PanelClientCodeFocus.Text.Length - 1);
            }
        }

        private void BtnClientCode0Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "0";
        }

        private void BtnClientCodeClearClick(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text = String.Empty;
        }

        private void BtnClientCode1Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "1";
        }

        private void BtnClientCode2Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "2";
        }

        private void BtnClientCode3Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "3";
        }

        private void BtnClientCode4Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "4";
        }

        private void BtnClientCode5Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "5";
        }

        private void BtnClientCode6Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "6";
        }

        private void BtnClientCode7Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "7";
        }

        private void BtnClientCode8Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "8";
        }

        private void BtnClientCode9Click(object sender, EventArgs e)
        {
            _PanelClientCodeFocus.Text += "9";
        }

        private void BtnCreditInfoBackClick(object sender, EventArgs e)
        {
            ChangePannel(pnlClientCode);
        }

        private void BtnCreditInfoNextClick(object sender, EventArgs e)
        {
            _CurrentCurrency = "AZN";
            StartCashcode();
            ChangePannel(pnlMoney);
        }

        private void BtnDebitInfoBackClick(object sender, EventArgs e)
        {
            ChangePannel(pnlClientCode);
        }

        private void BtnDebitInfoNextClick(object sender, EventArgs e)
        {
            _CurrentCurrency = "AZN";
            StartCashcode();
            ChangePannel(pnlMoney);
        }

        private void StartCashcode()
        {
            SetStackedAmount("0");
            try
            {
                _PaymentId = _Db.InsertTransaction(_SelectedProduct, _CurrentCurrency, 1, 0,
                                                   Convert.ToInt32(Settings.Default.TerminalCode), false);

                _Db.UpdateTransactionId(_PaymentId, String.Format("{0}0000{1}", Settings.Default.TerminalCode, _PaymentId));
                _Db.InsertPaymentValue(_PaymentId, txtClientCodeClient.Text, 0);
                _Db.InsertPaymentValue(_PaymentId, txtClientCodePassport.Text, 1);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            _CcnetDevice.Poll();
            _CcnetDevice.EnableAll();
            _CcnetDevice.StartPool = true;
        }

        private void StopCashcode()
        {
            _CcnetDevice.StartPool = false;
            _CcnetDevice.Disable();
        }

        private void BtnEncashmentPrintClick(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void BtnEncashmentFinishClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            _CcnetDevice.Reset();

            try
            {
                _Db.DeleteCasseteBanknotes();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            Thread.Sleep(1000);
            ChangePannel(pnlProducts);
            _EncashmentMode = false;

            Cursor.Current = Cursors.Default;
        }

        private void FormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            _Running = false;

            Thread.Sleep(250);
            _CcnetDevice.Dispose();
        }

        private void TxtClientCodeClientClick(object sender, EventArgs e)
        {
            _PanelClientCodeFocus = txtClientCodeClient;
        }

        private void TxtClientCodePassportClick(object sender, EventArgs e)
        {
            _PanelClientCodeFocus = txtClientCodePassport;
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
                var result = _Server.InitTerminal(Convert.ToInt32(txtActivationTerminal.Text), txtActivationCode.Text, Settings.Default.PublicKey);

                if (result == null)
                {
                    throw new NullReferenceException("Server response is null");
                }

                if (result.ResultCodes == ResultCodes.Ok)
                {
                    Log.Info("Success activation!");
                    Settings.Default.TerminalCode = txtActivationTerminal.Text;
                    Settings.Default.Save();

                    _AuthTerminal = true;
                    ChangePannel(pnlProducts);
                    Cursor.Current = Cursors.Default;
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

        private void BtnActivationClearClick(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text = String.Empty;
        }

        private void BtnActivation0Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "0";
        }

        private void BtnActivationBackspaceClick(object sender, EventArgs e)
        {
            if (_PanelActivationFocus.Text.Length > 0)
            {
                _PanelActivationFocus.Text = _PanelActivationFocus.Text.Substring(0, _PanelActivationFocus.Text.Length - 1);
            }
        }

        private void BtnActivation1Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "1";
        }

        private void BtnActivation2Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "2";
        }

        private void BtnActivation3Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "3";
        }

        private void BtnActivation4Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "4";
        }

        private void BtnActivation5Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "5";
        }

        private void BtnActivation6Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "6";
        }

        private void BtnActivation7Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "7";
        }

        private void BtnActivation8Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "8";
        }

        private void BtnActivation9Click(object sender, EventArgs e)
        {
            _PanelActivationFocus.Text += "9";
        }

        private void TxtActivationTerminalClick(object sender, EventArgs e)
        {
            _PanelActivationFocus = txtActivationTerminal;
        }

        private void TxtActivationCodeClick(object sender, EventArgs e)
        {
            _PanelActivationFocus = txtActivationCode;
        }

        private void PnlActivationVisibleChanged(object sender, EventArgs e)
        {
            _PanelActivationFocus = txtActivationTerminal;
        }

        private void PnlClientCodeVisibleChanged(object sender, EventArgs e)
        {
            _PanelClientCodeFocus = txtClientCodeClient;
        }

        private void CcnetDeviceBillStacked(CCNETDeviceState e)
        {
            SetStackedAmount(e.Amount);
        }

        private void CcnetDeviceReadCommand(CCNETDeviceState e)
        {
            Log.Debug(e.ToString());
        }

        private void BtnMoneyBackClick(object sender, EventArgs e)
        {
            switch (_SelectedProduct)
            {
                case 1:
                    ChangePannel(pnlCreditInfo);
                    break;

                case 2:
                    ChangePannel(pnlDebitInfo);
                    break;
            }
        }

        private void BtnMoneyNextClick(object sender, EventArgs e)
        {
            try
            {
                _Db.ConfirmTransaction(_PaymentId);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            PrintCheck();
        }

        private void PrintCheck()
        {
            //throw new NotImplementedException();
        }
    }
}
