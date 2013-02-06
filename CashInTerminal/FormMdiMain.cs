using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using CashInTerminal.CashIn;
using CashInTerminal.Properties;
using Containers.Enums;
using NLog;
using Org.BouncyCastle.Crypto;
using crypto;
using ResultCodes = CashInTerminal.CashIn.ResultCodes;
using Timer = System.Threading.Timer;

namespace CashInTerminal
{
    public partial class FormMdiMain : Form
    {
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

        private ClientInfo _ClientInfo = new ClientInfo();

        public AsymmetricCipherKeyPair LocalKeys
        {
            get { return _LocalKeys; }
            set { _LocalKeys = value; }
        }

        public AsymmetricKeyParameter ServerPublicKey
        {
            get { return _ServerPublicKey; }
            set { _ServerPublicKey = value; }
        }

        public CCNETDevice CcnetDevice
        {
            get { return _CcnetDevice; }
            set { _CcnetDevice = value; }
        }

        public bool Init
        {
            get { return _Init; }
            set { _Init = value; }
        }

        public bool AuthTerminal
        {
            get { return _AuthTerminal; }
            set { _AuthTerminal = value; }
        }

        public bool EncashmentMode
        {
            get { return _EncashmentMode; }
            set { _EncashmentMode = value; }
        }

        public Terminal TerminalInfo
        {
            get { return _TerminalInfo; }
            set { _TerminalInfo = value; }
        }

        public LocalDb Db
        {
            get { return _Db; }
            set { _Db = value; }
        }

        public bool Running
        {
            get { return _Running; }
            set { _Running = value; }
        }

        public ClientInfo ClientInfo
        {
            get { return _ClientInfo; }
            set { _ClientInfo = value; }
        }

        public CashInServer Server
        {
            get { return _Server; }
            set { _Server = value; }
        }
       
        public List<Currency> Currencies
        {
            get { return _Currencies; }
        }

        public List<Product> Products
        {
            get { return _Products; }
        }

        private CashInServer _Server;

        //private TextBox _PanelActivationFocus;
        //private TextBox _PanelDebitInfoActivationFocus;
        //private TextBox _PanelClientCodeFocus;
        private long _PaymentId;
        private int _OrderNumber;
        private string _TransactionId;
        private String _CurrentCurrency;
        private Form _CurrentForm;

        #region Threads

        private Thread _PingThread;
        private Thread _SendPaymentThread;
        private Timer _CheckCurrencyTimer;
        private Timer _CheckProductsTimer;
        private Timer _CheckInactivityTimer;
        private const int PING_TIMEOUT = 30 * 1000;
        private const int SEND_PAYMENT_TIMEOUT = 10000;
        private const int CHECK_CURRENCY_TIMER = 30 * 60 * 1000;
        private const int CHECK_PRODUCTS_TIMER = 60 * 60 * 1000;
        private const int CHECK_INACTIVITY = 10 * 1000;
        private const int MAX_INACTIVITY_PERIOD = 2 * 60; // Seconds!
        private readonly List<Currency> _Currencies = new List<Currency>();
        private readonly List<Product> _Products = new List<Product>();

        private uint _LastActivity;

        public delegate void CheckCurrencyTimerCaller(object param);
        public delegate void CheckProductsTimerCaller(object param);

        #endregion

        public FormMdiMain()
        {
            InitializeComponent();
            Log.Info("Started");
        }

        private void FormMdiMain_Load(object sender, EventArgs e)
        {

            OpenForm(new FormOutOfOrder());

            foreach (Control control in Controls)
            {
                var client = control as MdiClient;
                if (client != null)
                {
                    client.BackColor = Control.DefaultBackColor;
                    break;
                }
            }

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
                //_CcnetDevice.BillStacked += CcnetDeviceBillStacked;
                //_CcnetDevice.ReadCommand += CcnetDeviceReadCommand;
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
                OpenForm(new FormActivation());
            }
            else if (!_Init)
            {
                OpenForm(new FormOutOfOrder());
            }

            _CheckCurrencyTimer = new Timer(CheckCurrencyTimer, null, 0, CHECK_CURRENCY_TIMER);
            _CheckProductsTimer = new Timer(CheckProductsTimer, null, 0, CHECK_PRODUCTS_TIMER);
            _CheckInactivityTimer = new Timer(CheckInactivityTimer, null, 0, CHECK_INACTIVITY);
        }

        public void OpenForm(Form f)
        {
            if (_CurrentForm != null)
            {
                _CurrentForm.Close();
            }

            _CurrentForm = f;
            f.MdiParent = this;
            f.Show();
            f.WindowState = FormWindowState.Maximized;
        }

        public void MinimizeAllChildren()
        {
            foreach (Form f in this.MdiChildren)
            {
                f.WindowState = FormWindowState.Minimized;
            }
        }

        private void CheckInactivityTimer(object param)
        {
            if (_CurrentForm.Name != FormEnum.MoneyInput && _CurrentForm.Name != FormEnum.Language && _CurrentForm.Name != FormEnum.TestMode && _CurrentForm.Name != FormEnum.Encashment)
            {
                _LastActivity = Utilities.GetLastInputTime();
                if (_LastActivity > MAX_INACTIVITY_PERIOD)
                {
                    OpenForm(new FormLanguage());
                }
            }
            else
            {
                _LastActivity = 0;
            }
        }

        private void CheckCurrencyTimer(object param)
        {
            if (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    var now = DateTime.Now;
                    var request = new StandardRequest
                    {
                        SystemTime = now,
                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                    };

                    var response = _Server.ListCurrencies(request);

                    CheckSignature(response);

                    if (response.ResultCodes == ResultCodes.Ok)
                    {
                        lock (_Currencies)
                        {
                            _Currencies.Clear();
                            foreach (var item in response.Currencies)
                            {
                                _Currencies.Add(item);
                            }
                        }
                    }
                    else
                    {
                        Log.Warn(response.ResultCodes + " " + response.Description);
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
            }
        }

        private void CheckProductsTimer(object param)
        {
            if (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    var now = DateTime.Now;
                    var request = new StandardRequest
                    {
                        SystemTime = now,
                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                    };

                    var response = _Server.ListProducts(request);

                    CheckSignature(response);

                    if (response.ResultCodes == ResultCodes.Ok)
                    {
                        lock (_Products)
                        {
                            _Products.Clear();
                            foreach (var item in response.Products)
                            {
                                _Products.Add(item);
                            }
                        }
                    }
                    else
                    {
                        Log.Warn(response.ResultCodes + " " + response.Description);
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
            }
        }

        private void PingThread()
        {
            while (_Running)
            {
                while (_Init && _AuthTerminal)
                {
                    try
                    {
                        var now = DateTime.Now;
                        var request = new PingRequest
                        {
                            CashCodeStatus = (int)_CcnetDevice.DeviceState.StateCode,
                            SystemTime = now,
                            TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                            TerminalStatus = !_EncashmentMode ? (int)TerminalCodes.Ok : (int)TerminalCodes.Encashment,
                            Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                        };
                        var result = _Server.Ping(request);

                        CheckSignature(result);

                        if (result != null)
                        {
                            if (result.ResultCodes != ResultCodes.Ok)
                            {
                                Log.Warn(String.Format("Server return: {0}, {1}", result.ResultCodes, result.Description));
                            }
                            else
                            {
                                switch ((TerminalCommands)result.Command)
                                {
                                    case TerminalCommands.OutOfService:
                                        Log.Warn("Received command " + TerminalCommands.OutOfService.ToString());

                                        if (_CurrentForm.Name != FormEnum.MoneyInput && _CurrentForm.Name != FormEnum.PaySuccess)
                                        {
                                            OpenForm(new FormOutOfOrder());
                                            CommandReceived();
                                        }
                                        break;

                                    case TerminalCommands.TestMode:
                                        Log.Warn("Received command " + TerminalCommands.TestMode.ToString());
                                        if (_CurrentForm.Name != FormEnum.MoneyInput && _CurrentForm.Name != FormEnum.PaySuccess)
                                        {
                                            OpenForm(new FormTestMode());
                                            CommandReceived();
                                        }
                                        break;

                                    case TerminalCommands.Encash:
                                        Log.Warn("Received command " + TerminalCommands.Encash.ToString());

                                        if (_CurrentForm.Name != FormEnum.MoneyInput && _CurrentForm.Name != FormEnum.PaySuccess)
                                        {
                                            DoEncashment();
                                        }
                                        break;

                                    case TerminalCommands.Idle:
                                    case TerminalCommands.NormalMode:
                                        if ((_CurrentForm.Name == FormEnum.OutOfOrder || _CurrentForm.Name == FormEnum.TestMode))
                                        {
                                            GetTerminalInfo();
                                            OpenForm(new FormLanguage());
                                        }

                                        CommandReceived();
                                        break;
                                }
                                // TODO : Добавить обработчик этого
                            }
                        }
                        else
                        {
                            Log.Error("Result is null");
                        }
                    }
                    catch (Exception exp)
                    {
                        Log.ErrorException(exp.Message, exp);
                    }
                    Thread.Sleep(PING_TIMEOUT);
                }

                Thread.Sleep(PING_TIMEOUT);
            }
        }

        private void GetTerminalInfo()
        {
            DateTime now = DateTime.Now;
            var cmd = new StandardRequest
            {
                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                SystemTime = now,
                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
            };
            var response = _Server.GetTerminalInfo(cmd);

            if (response != null)
            {
                _TerminalInfo = response.Terminal;
            }
        }

        private StandardResult CommandReceived()
        {
            DateTime now = DateTime.Now;
            var cmd = new StandardRequest
            {
                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                SystemTime = now,
                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
            };
            return _Server.CommandReceived(cmd);
        }

        private void DoEncashment()
        {
            OpenForm(new FormEncashment());

            try
            {
                var secondResult = CommandReceived();
                CheckSignature(secondResult);

                var request = new Encashment();

                var amountList = new List<int>();
                var curList = new List<string>();
                foreach (var currency in _Currencies)
                {
                    var total = _Db.GetCasseteTotal(currency.Name);

                    amountList.Add(total);
                    curList.Add(currency.Name);
                }

                var now = DateTime.Now;
                request.SystemTime = now;
                request.TerminalId = Convert.ToInt32(Settings.Default.TerminalCode);
                request.Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey);
                request.Amounts = amountList.ToArray();
                request.Currencies = curList.ToArray();

                var result = _Server.Encashment(request);

                if (result != null)
                {
                    Log.Info(String.Format("Server answer {0} {1}", result.ResultCodes, result.Description));
                    CheckSignature(secondResult);
                }
                else
                {
                    Log.Warn("Result is null");
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            catch
            {
                Log.Error("Unknown error");
            }
        }


        private void SendPaymentThread()
        {
            var listToDelete = new Queue<long>();

            while (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    var list = _Db.GetTransactions();

                    foreach (var row in list)
                    {
                        var values = _Db.GetPaymentValues(row.Id);
                        var banknotes = _Db.GetPaymentBanknotes(row.Id);

                        var now = DateTime.Now;
                        var request = new PaymentInfoByProducts
                        {
                            TerminalDate = now,
                            SystemTime = now,
                            TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                            TransactionId = row.TransactionId,
                            ProductId = (int)row.ProductId,
                            Currency = row.Currency,
                            CurrencyRate = (float)row.CurrencyRate,
                            Amount = (int)row.Amount,
                            Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                        };

                        var valuesList = new List<String>();
                        var banknotesList = new List<int>();

                        foreach (var value in values)
                        {
                            valuesList.Add(value.Value);
                        }

                        foreach (var banknote in banknotes)
                        {
                            banknotesList.Add((int)banknote.Amount);
                        }

                        request.Banknotes = banknotesList.ToArray();
                        request.Values = valuesList.ToArray();

                        var response = _Server.Payment(request);

                        if (response != null && response.ResultCodes == ResultCodes.Ok)
                        {
                            listToDelete.Enqueue(row.Id);
                        }
                        else if (response != null)
                        {
                            Log.Warn(String.Format("Server answer: {0}, {1}", response.ResultCodes, response.Description));
                        }
                        else
                        {
                            Log.Warn("Response is null");
                        }
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
                catch
                {
                    Log.Error("Non cachable exception");
                }

                try
                {
                    if (listToDelete.Count > 0)
                    {
                        foreach (var item in listToDelete)
                        {
                            _Db.DeleteTransaction(item);
                        }
                    }
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
                catch
                {
                    Log.Error("Non cachable exception");
                }
                Thread.Sleep(SEND_PAYMENT_TIMEOUT);
            }
        }

        private void CheckSignature(StandardResult request)
        {
            if (!Utilities.CheckSignature(Settings.Default.TerminalCode, request.SystemTime, request.Sign, _LocalKeys))
            {
                throw new Exception("Invalid signature");
            }
        }

        private void FormMdiMainFormClosing(object sender, FormClosingEventArgs e)
        {
            _Running = false;

            _CheckCurrencyTimer.Dispose();
            _CheckProductsTimer.Dispose();
            _CheckInactivityTimer.Dispose();

            Thread.Sleep(250);

            _CcnetDevice.Close();
            _CcnetDevice.Dispose();
        }
    }
}
