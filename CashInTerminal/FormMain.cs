using System;
using System.Text;
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
        private AsymmetricCipherKeyPair _LocalKeys;
        private AsymmetricKeyParameter _ServerPublicKey;
        private CCNETDevice _CcnetDevice;
        private bool _Init;
        private bool _AuthTerminal;
        private bool _EncashmentMode;
        private Terminal _TerminalInfo;
        private LocalDb _Db;
        private bool _Running = true;
        private delegate void PanelShowDelegate(object item);
        private delegate void PanelHideDelegate(object item);

        private delegate void SetStackedAmountDelegate(object item);

        private delegate void EnableMoneyNextButtonDelegate(object item);

        private int _SelectedProduct;
        private CashInServer _Server;
        private TextBox _PanelActivationFocus;
        private TextBox _PanelDebitInfoActivationFocus;
        private TextBox _PanelClientCodeFocus;
        private long _PaymentId;
        private int _OrderNumber;
        private string _TransactionId;
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

                Log.Debug("Local public key: " + Settings.Default.PublicKey);
                _LocalKeys = Wrapper.GetKeys(Settings.Default.PrivateKey, Settings.Default.PublicKey);

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
                _CcnetDevice.ReadCommand += CcnetDeviceReadCommand;
                //_CcnetDevice.Reset();

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
                var publicKey = _Server.GetPublicKey();
                Log.Debug("Public key " + publicKey);
                Settings.Default.ServerPublicKey = publicKey;
                _ServerPublicKey = Wrapper.GetKey(Settings.Default.ServerPublicKey);
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

            _CheckCurrencyTimer = new System.Threading.Timer(CheckCurrencyTimer, null, 0, CHECK_CURRENCY_TIMER);
            _CheckProductsTimer = new System.Threading.Timer(CheckProductsTimer, null, 0, CHECK_PRODUCTS_TIMER);
            _CheckInactivityTimer = new System.Threading.Timer(CheckInactivityTimer, null, 0, CHECK_INACTIVITY);
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

        private void EnableMoneyNextButton(object param)
        {
            if (btnMoneyNext.InvokeRequired)
            {
                btnMoneyNext.Invoke(new EnableMoneyNextButtonDelegate(EnableMoneyNextButton), param);
            }
            else
            {
                btnMoneyNext.Enabled = true;
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
            txtDebitClientCode.Text = String.Empty;
            txtDebitClientCodePassport.Text = String.Empty;
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
            ChangePannel(pnlDebitClientCode);
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

        #region Client Code buttons

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

        #endregion

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
            btnMoneyNext.Enabled = false;
            SetStackedAmount("0");
            try
            {
                _OrderNumber = 0;
                _PaymentId = _Db.InsertTransaction(_SelectedProduct, _CurrentCurrency, 1, 0,
                                                   Convert.ToInt32(Settings.Default.TerminalCode), false);
                _TransactionId = String.Format("{0}{1}{2}", Settings.Default.TerminalCode,
                                               DateTime.Now.DayOfYear.ToString("000"), _PaymentId);

                _Db.UpdateTransactionId(_PaymentId, _TransactionId);
                _Db.InsertPaymentValue(_PaymentId, txtClientCodeClient.Text, 0);
                _Db.InsertPaymentValue(_PaymentId, txtClientCodePassport.Text, 1);

                Log.Info("Starting transId: {0}, PaymentId: {1}", _TransactionId, _PaymentId);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            _CcnetDevice.Poll();
            _CcnetDevice.Enable(_CurrentCurrency.ToLower());
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
                var msg = new StringBuilder();
                foreach (var currency in _Currencies)
                {
                    var total = _Db.GetCasseteTotal(currency.Name);

                    msg.Append(total).Append(" ").Append(currency.Name).Append("\n");
                }

                Log.Info(msg.ToString());
                MessageBox.Show(msg.ToString());
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void BtnEncashmentFinishClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //_CcnetDevice.Reset();

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

            _CheckCurrencyTimer.Dispose();
            _CheckProductsTimer.Dispose();
            _CheckInactivityTimer.Dispose();

            Thread.Sleep(250);

            _CcnetDevice.Close();
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

                //CheckSignature(result);

                if (result.ResultCodes == ResultCodes.Ok)
                {
                    Log.Info("Success activation!");
                    Settings.Default.TerminalCode = txtActivationTerminal.Text;
                    Settings.Default.Save();

                    _AuthTerminal = true;
                    ChangePannel(pnlProducts);
                    btnActivation.Enabled = true;
                    Cursor.Current = Cursors.Default;

                    try
                    {
                        GetTerminalInfo();

                        var checkCurrencyTimerCaller = new CheckCurrencyTimerCaller(CheckCurrencyTimer);
                        checkCurrencyTimerCaller.Invoke(null);

                        var checkProductsTimerCaller = new CheckProductsTimerCaller(CheckProductsTimer);
                        checkProductsTimerCaller.Invoke(null);
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                    }

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

        #region Activate Buttons

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

        #endregion

        private void PnlClientCodeVisibleChanged(object sender, EventArgs e)
        {
            _PanelClientCodeFocus = txtClientCodeClient;
        }

        private void CcnetDeviceBillStacked(CCNETDeviceState e)
        {
            Log.Info(String.Format("Stacked {0} {1}", e.Nominal, e.Currency));
            EnableMoneyNextButton(null);
            SetStackedAmount(e.Amount);

            try
            {
                _Db.InsertBanknote(_PaymentId, e.Nominal, e.Currency, _OrderNumber++);
                _Db.InsertTransactionBanknotes(e.Nominal, e.Currency, _TransactionId);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
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
            btnMoneyNext.Enabled = false;
            try
            {
                StopCashcode();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                _Db.UpdateAmount(_PaymentId, lblMoneyCurrency.Text, 1, Convert.ToInt32(lblMoneyTotal.Text));
                _Db.ConfirmTransaction(_PaymentId);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            lblSuccessTotalAmount.Text = lblMoneyTotal.Text + @" " + lblMoneyCurrency.Text;
            PrintCheck();

            btnMoneyNext.Enabled = true;
            ChangePannel(pnlPaySuccess);
        }

        private void PrintCheck()
        {
            //throw new NotImplementedException();
        }

        private void FormMainClick(object sender, EventArgs e)
        {
            //if (_SelectedPanel == "pnlClientCode")
            //{
            //    Log.Debug("Click");
            //}
        }

        private void PnlClientCodeClick(object sender, EventArgs e)
        {
            //Log.Debug("Click");
        }

        private void FormMain_MouseClick(object sender, MouseEventArgs e)
        {
            //if (_SelectedPanel == "pnlClientCode")
            //{
            //    Log.Debug("Click");
            //}
        }

        private void pnlClientCode_MouseHover(object sender, EventArgs e)
        {
            Log.Debug("Click");
        }

        private void BtnSuccessNextClick(object sender, EventArgs e)
        {
            ChangePannel(pnlProducts);
        }

        private void txtDebitClientCode_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus = txtDebitClientCode;
        }

        private void txtDebitClientCodePassport_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus = txtDebitClientCodePassport;
        }

        private void btnDebitClientCodeBackspace_Click(object sender, EventArgs e)
        {
            if (_PanelDebitInfoActivationFocus.Text.Length > 0)
            {
                _PanelDebitInfoActivationFocus.Text = _PanelDebitInfoActivationFocus.Text.Substring(0, _PanelDebitInfoActivationFocus.Text.Length - 1);
            }
        }

        private void btnDebitClientCode0_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "0";
        }

        private void btnDebitClientCodeClear_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text = string.Empty;
        }

        private void btnDebitClientCode1_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "1";
        }

        private void btnDebitClientCode2_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "2";
        }

        private void btnDebitClientCode3_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "3";
        }

        private void btnDebitClientCode4_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "4";
        }

        private void btnDebitClientCode5_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "5";
        }

        private void btnDebitClientCode6_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "6";
        }

        private void btnDebitClientCode7_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "7";
        }

        private void btnDebitClientCode8_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "8";
        }

        private void btnDebitClientCode9_Click(object sender, EventArgs e)
        {
            _PanelDebitInfoActivationFocus.Text += "9";
        }

        private void btnDebitClientCodePrev_Click(object sender, EventArgs e)
        {
            _SelectedProduct = 0;
            ChangePannel(pnlProducts);
        }

        private void btnDebitClientCodeNext_Click(object sender, EventArgs e)
        {
            if (txtDebitClientCode.Text.Length == 0 || txtDebitClientCodePassport.Text.Length == 0)
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
    }
}
