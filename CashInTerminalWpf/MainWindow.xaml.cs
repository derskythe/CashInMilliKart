using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CashInTerminal.Enums;
using CashInTerminalWpf.CashIn;
using CashInTerminalWpf.Enums;
using CashInTerminalWpf.Properties;
using Containers.Enums;
using NLog;
using Org.BouncyCastle.Crypto;
using crypto;
using Path = System.IO.Path;
using PrinterStatus = CashInTerminalWpf.CashIn.PrinterStatus;
using ResultCodes = CashInTerminalWpf.CashIn.ResultCodes;
using ThreadState = System.Threading.ThreadState;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        private AsymmetricCipherKeyPair _LocalKeys;
        private AsymmetricKeyParameter _ServerPublicKey;
        private CCNETDevice _CcnetDevice;
        private readonly PrinterStatus _PrinterStatus = new PrinterStatus();
        private volatile bool _Init;
        private volatile bool _AuthTerminal;
        private TerminalCodes _TerminalStatus;
        private Terminal _TerminalInfo = new Terminal();
        private LocalDb _Db;
        private volatile bool _Running = true;
        private CashIn.ClientInfo[] _Clients;
        private StandardResult _InfoResponse;
        private Dictionary<int, List<ds.TemplateFieldRow>> _CheckTemplates = new Dictionary<int, List<ds.TemplateFieldRow>>();
        private String _SelectedLanguage = InterfaceLanguages.Az;
        private StandardRequest _InfoRequest;
        private LongRequestType _LongRequestType;
        //private const int TOTAL_CHECK_COUNT = 2727;
        //private const int LOW_LEVEL_CHECK_COUNT = 2454;
        private String _CurrentForm = FormEnum.OutOfOrder;
        private String _PrevForm = FormEnum.OutOfOrder;

        private ClientInfo _ClientInfo = new ClientInfo();

        //private delegate void CloseFormDelegate(object item);

        public delegate void ProductUpdateHandler();

        public event ProductUpdateHandler ProductUpdate;

        //private delegate void SetFormParentDelegate(object item);

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

        public LongRequestType LongRequestType
        {
            get { return _LongRequestType; }
            set { _LongRequestType = value; }
        }

        public Dictionary<int, List<ds.TemplateFieldRow>> CheckTemplates
        {
            get { return _CheckTemplates; }
            set { _CheckTemplates = value; }
        }

        public string SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set { _SelectedLanguage = value; }
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

        public TerminalCodes TerminalStatus
        {
            get { return _TerminalStatus; }
            set { _TerminalStatus = value; }
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

        public CashIn.ClientInfo[] Clients
        {
            get { return _Clients; }
            set { _Clients = value; }
        }

        public StandardRequest InfoRequest
        {
            get { return _InfoRequest; }
            set { _InfoRequest = value; }
        }

        public StandardResult InfoResponse
        {
            get { return _InfoResponse; }
            set { _InfoResponse = value; }
        }

        public List<PaymentCategory> KomtekPaymentCategories
        {
            get { return _KomtekPaymentCategories; }
        }

        public List<PaymentCategory> MoneyMoversPaymentCategories
        {
            get { return _MoneyMoversPaymentCategories; }
        }

        public string PrevForm
        {
            get { return _PrevForm; }
            set { _PrevForm = value; }
        }

        private CashInServer _Server;

        #region Threads

        private Thread _PingThread;
        private Thread _SendPaymentThread;
        private Thread _TerminationThread;
        private Timer _CheckCurrencyTimer;
        private Timer _CheckBillTableTimer;
        private Timer _CheckProductsTimer;
        private Timer _CheckCheckTemplateTimer;
        private Timer _CheckPaymentCategoriesTimer;
        private Timer _CheckInactivityTimer;
        private Timer _CheckApplicationUpdateTimer;
        private const int PING_TIMEOUT = 30 * 1000;
        private const int SEND_PAYMENT_TIMEOUT = 10 * 1000;
        private const int CHECK_CURRENCY_TIMER = 30 * 60 * 1000;
        private const int CHECK_BILL_TABLE_TIMER = 10 * 60 * 1000;
        private const int CHECK_BILL_TABLE_TIMER_FIRST_TIME = 45 * 1000;
        private const int CHECK_CHECK_TEMPLATE_TIMER = 60 * 1000;
        private const int CHECK_PRODUCTS_TIMER = 60 * 60 * 1000;
        private const int CHECK_INACTIVITY = 10 * 1000;

        private const int CHECK_APPLICATION_UPDATE_TIMER = 60 * 1000;
        private const int MAX_INACTIVITY_PERIOD = 2 * 60; // Value in seconds!
        private readonly List<Currency> _Currencies = new List<Currency>();
        private readonly List<Product> _Products = new List<Product>();
        private readonly List<PaymentCategory> _KomtekPaymentCategories = new List<PaymentCategory>(5);
        private readonly List<PaymentCategory> _MoneyMoversPaymentCategories = new List<PaymentCategory>(5);

        private uint _LastActivity;

        public delegate void CheckCurrencyTimerCaller(object param);
        public delegate void CheckProductsTimerCaller(object param);

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            ShowsNavigationUI = false;
            ProductUpdate += delegate { };
        }

        private void NavigationWindowLoaded(object sender, RoutedEventArgs e)
        {
            Log.Info("Window loaded");
            Cursor = Cursors.None;

            string currentDeployment = String.Empty;

            try
            {
                currentDeployment = ApplicationDeployment.CurrentDeployment.DataDirectory;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            Log.Info(String.Format("Trying to start system.\nTerminalId: {0},\nComPort: {1},\nData dir: {2},\nExec path: {3}\nCurrent build: {4}\nFile Version: {5}",
                                   Settings.Default.TerminalCode,
                                   Settings.Default.DevicePort,
                                   currentDeployment,
                                   Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                   assembly.GetName().Version,
                                   version
                                   ));

            var startUpThread = new Thread(DoStartUp);
            startUpThread.Start();
        }

        private void DoStartUp()
        {
            Log.Debug("Started thread: " + Thread.CurrentThread.ManagedThreadId);

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
                _Init = false;
            }

            // Init cashcode
            Log.Info("Init Cashcode");
            try
            {
                InitCashCode(Settings.Default.DevicePort);
                //_CcnetDevice = new CCNETDevice();
                //_CcnetDevice.Open(Settings.Default.DevicePort, CCNETPortSpeed.S9600);
                //_CcnetDevice.Init();
                //_CcnetDevice.Reset();
                //_CcnetDevice.BillStacked += CcnetDeviceBillStacked;
                //_CcnetDevice.ReadCommand += CcnetDeviceReadCommand;
                //_CcnetDevice.Reset();

                _Init &= true;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
                _Init = false;
            }

            // Init DB
            Log.Info("Init Db");
            try
            {
                _Db = new LocalDb();

                if (!_Db.HasTable())
                {
                    _Db.CreateOtherTable();
                }

                _Db.CountCasseteBanknotes();
                LoadCheckTemplates();
                UpdateOrphanedTransactions();
            }
            catch (Exception exp)
            {
                _Init = false;
                Log.ErrorException(exp.Message, exp);
            }

            Log.Info("Init server connection");
            try
            {
                _Server = new CashInServerClient();
                GetPublicKey();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            //Settings.Default.TerminalCode = String.Empty;
            //Settings.Default.Save();

            _AuthTerminal = !String.IsNullOrEmpty(Settings.Default.TerminalCode);

            if (_Init && !_AuthTerminal)
            {
                _TerminalStatus = TerminalCodes.Ok;
                OpenForm(FormEnum.Activation);
            }
            else if (!_Init)
            {
                _TerminalStatus = TerminalCodes.OutOfOrder;
                OpenForm(FormEnum.OutOfOrder);
            }
            else
            {
                try
                {
                    GetTerminalInfo();
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }

                //OpenForm(typeof(FormLanguage));
                //OpenForm(typeof(FormProducts));
            }

            _PingThread = new Thread(PingThread);
            _PingThread.Start();

            _SendPaymentThread = new Thread(SendPaymentThread);
            _SendPaymentThread.Start();

            _CheckCurrencyTimer = new Timer(CheckCurrencyTimer, null, 0, CHECK_CURRENCY_TIMER);
            _CheckProductsTimer = new Timer(CheckProductsTimer, null, 0, CHECK_PRODUCTS_TIMER);
            _CheckPaymentCategoriesTimer = new Timer(CheckPaymentCategoriesTimer, null, 0, CHECK_CURRENCY_TIMER);
            _CheckInactivityTimer = new Timer(CheckInactivityTimer, null, CHECK_INACTIVITY, CHECK_INACTIVITY);
            _CheckCheckTemplateTimer = new Timer(CheckTemplateTimer, null, 0, CHECK_CHECK_TEMPLATE_TIMER);
            _CheckApplicationUpdateTimer = new Timer(ApplicationUpdateTimer, null, 0, CHECK_APPLICATION_UPDATE_TIMER);
            _CheckBillTableTimer = new Timer(CheckBillTableTimer, null, CHECK_BILL_TABLE_TIMER_FIRST_TIME, CHECK_BILL_TABLE_TIMER);
        }

        private void UpdateOrphanedTransactions()
        {
            try
            {
                _Db.UpdateOrphanedTransactions();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        #region Application Update

        private void ApplicationUpdateTimer(object state)
        {
            try
            {
                Log.Debug(_CurrentForm);
                if (CanChangeViewOnCommand)
                {
                    Log.Debug("Check update");
                    UpdateCheckInfo info;

                    // Check if the application was deployed via ClickOnce.
                    if (!ApplicationDeployment.IsNetworkDeployed)
                    {
                        Log.Warn("Is this NOT deployed via ClickOnce");
                        return;
                    }

                    ApplicationDeployment updateCheck = ApplicationDeployment.CurrentDeployment;

                    try
                    {
                        info = updateCheck.CheckForDetailedUpdate();
                    }
                    catch (DeploymentDownloadException exp)
                    {
                        Log.Error("Couldn't retrieve info on this app. " + exp.Message);
                        return;
                    }
                    catch (InvalidDeploymentException exp)
                    {
                        Log.Error("Cannot check for a new version. ClickOnce deployment is corrupt! " + exp.Message);
                        return;
                    }
                    catch (InvalidOperationException exp)
                    {
                        Log.Error("Cannot check for a new version. ClickOnce deployment is corrupt!" + exp.Message);
                        return;
                    }

                    if (info.UpdateAvailable)
                    {
                        if (info.IsUpdateRequired)
                        {
                            Log.Info("A required update is available, which will be installed now");
                            UpdateApplication();
                        }
                        else
                        {
                            Log.Info("An update is available");
                            UpdateApplication();
                        }

                        return;
                    }

                    Log.Info("There's no update");
                }
                else
                {
                    Log.Info("Can't check update this time");
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void UpdateApplication()
        {
            try
            {
                if (CanChangeViewOnCommand && !_CcnetDevice.DeviceState.BillEnable)
                {
                    OpenForm(FormEnum.OutOfOrder);
                    _TerminalStatus = TerminalCodes.Updating;
                    ApplicationDeployment updateCheck = ApplicationDeployment.CurrentDeployment;
                    updateCheck.Update();
                    Log.Info("The application has been upgraded, and will now restart.");

                    RestartApplication();
                }
                else
                {
                    Log.Info("CanChangeViewOnCommand return false! Can't reboot this time");
                }
            }
            catch (DeploymentDownloadException exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void RestartApplication()
        {
            //// Shut down the current app instance.
            //Application.Exit();

            //// Restart the app passing "/restart [processId]" as cmd line args
            //Process.Start(Application.ExecutablePath, "/restart " + Process.GetCurrentProcess().Id);
            OpenForm(FormEnum.OutOfOrder);
            _Running = false;

            if (_TerminationThread == null || _TerminationThread.ThreadState != ThreadState.Running)
            {
                _TerminationThread = new Thread(TerminationThread);
                _TerminationThread.Start();
            }
        }

        private void TerminationThread()
        {
            Log.Debug("Started thread: " + Thread.CurrentThread.ManagedThreadId);

            Thread.Sleep(100);
            DoClose();
            Log.Debug("Thread.Sleep(150)");
            Thread.Sleep(150);
            Log.Debug("Application.Restart");
            //System.Windows.Forms.Application.Restart();

            Thread.Sleep(2500);
            try
            {
                System.Windows.Forms.Application.Restart();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(ShutdownInvoke));
        }

        private void ShutdownInvoke()
        {
            try
            {
                if (Application.Current != null)
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        #endregion

        private void GetPublicKey()
        {
            var publicKey = _Server.GetPublicKey();
            Log.Debug("Public key " + publicKey);
            Settings.Default.ServerPublicKey = publicKey;
            _ServerPublicKey = Wrapper.GetKey(Settings.Default.ServerPublicKey);
            Settings.Default.Save();
        }

        public void InitCashCode(int port)
        {
            if (_CcnetDevice != null)
            {
                _CcnetDevice.Close();
                Thread.Sleep(150);
            }

            _CcnetDevice = new CCNETDevice();
            _CcnetDevice.BillStacked += CcnetDeviceOnBillStacked;
            _CcnetDevice.GetBills += CcnetDeviceOnGetBills;
            //_CcnetDevice.ReadCommand += CcnetDeviceOnReadCommand;
            _CcnetDevice.Open(port, CCNETPortSpeed.S9600);
            _CcnetDevice.Init();
        }

        private void CcnetDeviceOnGetBills(CCNETDeviceState ccnetDeviceState)
        {
            try
            {
                if (ccnetDeviceState.AvailableCurrencies != null && ccnetDeviceState.AvailableCurrencies.Count > 0)
                {
                    Log.Info(String.Join(";", ccnetDeviceState.AvailableCurrencies));
                }
                else
                {
                    ccnetDeviceState.AvailableCurrencies = new List<string>();
                    Log.Warn("Currency is null");
                }

                var now = DateTime.Now;
                var versionRequest = new TerminalVersionExtRequest
                {
                    TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                    SystemTime = DateTime.Now,
                    Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                    Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    AvailableCurrencies = ccnetDeviceState.AvailableCurrencies.ToArray(),
                    CashcodeVersion = ccnetDeviceState.Identification,
                    Ticks = now.Ticks
                };

                var versionResponse = _Server.UpdateTerminalVersionExt(versionRequest);
                if (versionResponse == null || versionResponse.ResultCodes != ResultCodes.Ok)
                {
                    Log.Error("Can't update version");
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void CcnetDeviceOnBillStacked(CCNETDeviceState ccnetDeviceState)
        {
            Log.Debug(ccnetDeviceState.ToString());
        }

        public void OpenForm(String f)
        {
            lock (_CurrentForm)
            {
                Log.Debug(String.Format("Current From: {0}, New form: {1}", _CurrentForm, f));
                _CurrentForm = f;
            }

            Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action<string>(Navigate),
                    f);
        }

        private void Navigate(string f)
        {
            try
            {
                _PrevForm = CurrentSource.OriginalString;
                Log.Debug(String.Format("PrevUri: {0}", _PrevForm));
                Navigate(new Uri(f, UriKind.RelativeOrAbsolute));
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        #region Check Template Stuff

        private void CheckTemplateTimer(object state)
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
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                        Ticks = now.Ticks
                    };

                    var response = _Server.ListCheckTemplateDigest(request);

                    CheckSignature(response);

                    if (response.ResultCodes == ResultCodes.Ok)
                    {
                        bool oldData = false;
                        foreach (var template in response.Templates)
                        {
                            var dbValue = _Db.GetCheckTemplate(template.Id);

                            if (dbValue == null)
                            {
                                oldData = true;
                                break;
                            }

                            var fields = _Db.ListTemplateFields(dbValue.Id);
                            if (dbValue.UpdateDate < template.UpdateDate || fields == null || fields.Count != template.Fields.Length)
                            {
                                oldData = true;
                                break;
                            }
                        }

                        if (oldData)
                        {
                            // Updating Db values
                            now = DateTime.Now;
                            request = new StandardRequest
                            {
                                SystemTime = now,
                                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                                Ticks = now.Ticks
                            };

                            response = _Server.ListCheckTemplate(request);

                            foreach (var template in response.Templates)
                            {
                                var dbValue = _Db.GetCheckTemplate(template.Id);

                                if (dbValue == null)
                                {
                                    // If value is new, we must delete all prev data by this language
                                    _Db.DeleteTemplateByType(template.CheckType.Id, template.Language.ToLower());

                                    _Db.InsertTemplate(template.Id, template.CheckType.Id, template.Language,
                                                       template.UpdateDate);
                                    foreach (var field in template.Fields)
                                    {
                                        _Db.InsertCheckTemplateField(field.Id, field.CheckId, field.FieldType,
                                                                     field.Value, field.Image, field.Order);
                                    }
                                }
                                else if (dbValue.UpdateDate < template.UpdateDate)
                                {
                                    // Data updated, first, update fields. 
                                    _Db.DeleteByCheckTemplateId(dbValue.Id);

                                    foreach (var field in template.Fields)
                                    {
                                        _Db.InsertCheckTemplateField(field.Id, field.CheckId, field.FieldType,
                                                                     field.Value, field.Image, field.Order);
                                    }

                                    _Db.UpdateTemplate(template.Id, template.UpdateDate);
                                }
                            }

                            LoadCheckTemplates();
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

        private void LoadCheckTemplates()
        {
            Log.Info("Start update CheckTemplates");
            lock (_CheckTemplates)
            {
                _CheckTemplates.Clear();

                UpdateCheckTemplateKey((int)CheckTemplateTypes.CreditPayment, InterfaceLanguages.Az);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.CreditPayment, InterfaceLanguages.En);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.CreditPayment, InterfaceLanguages.Ru);

                UpdateCheckTemplateKey((int)CheckTemplateTypes.DebitPayment, InterfaceLanguages.Az);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.DebitPayment, InterfaceLanguages.En);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.DebitPayment, InterfaceLanguages.Ru);

                UpdateCheckTemplateKey((int)CheckTemplateTypes.EncashmentFull, InterfaceLanguages.Az);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.EncashmentFull, InterfaceLanguages.En);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.EncashmentFull, InterfaceLanguages.Ru);

                UpdateCheckTemplateKey((int)CheckTemplateTypes.CreditWithBonus, InterfaceLanguages.Az);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.CreditWithBonus, InterfaceLanguages.En);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.CreditWithBonus, InterfaceLanguages.Ru);

                UpdateCheckTemplateKey((int)CheckTemplateTypes.OtherPayments, InterfaceLanguages.Az);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.OtherPayments, InterfaceLanguages.En);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.OtherPayments, InterfaceLanguages.Ru);
            }

            Log.Info("Finish update CheckTemplates");
        }

        private void UpdateCheckTemplateKey(int productId, String language)
        {
            try
            {
                var templateList = _Db.GetCheckTemplateByType(productId, language);

                foreach (var row in templateList)
                {
                    var key = GetCheckTemplateHashCode((int)row.Type, row.Language);
                    var fields = _Db.ListTemplateFields(row.Id);

                    _CheckTemplates.Add(key, fields);
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        public int GetCheckTemplateHashCode(int type, String language)
        {
            if (String.IsNullOrEmpty(language))
            {
                return type;
            }

            return language.GetHashCode() + type;
        }

        #endregion

        #region Timers

        private void CheckBillTableTimer(object sender)
        {
            try
            {
                if (!Utilities.AlphaNumber(_CcnetDevice.DeviceState.Identification) ||
                    _CcnetDevice.DeviceState.AvailableCurrencies == null ||
                    _CcnetDevice.DeviceState.AvailableCurrencies.Count == 0 ||
                    !HasDefaultCurrency())
                {
                    Log.Debug(String.Format("ID: {0}, AvailableCurrencies: {1}, IsAlphaNumber: {2}",
                                            _CcnetDevice.DeviceState.Identification,
                                            _CcnetDevice.DeviceState.AvailableCurrencies != null
                                                ? _CcnetDevice.DeviceState.AvailableCurrencies.Count
                                                : 0, Utilities.AlphaNumber(_CcnetDevice.DeviceState.Identification)));
                    _CcnetDevice.Identification();
                    Thread.Sleep(1000);
                    Log.Warn("Bill Table is null. Checkout");
                    _CcnetDevice.GetBillTable();
                }
                else
                {
                    Log.Debug(String.Format("ID: {0}, AvailableCurrencies: {1}, IsAlphaNumber: {2}", _CcnetDevice.DeviceState.Identification,
                                  _CcnetDevice.DeviceState.AvailableCurrencies != null
                                      ? _CcnetDevice.DeviceState.AvailableCurrencies.Count
                                      : 0, Utilities.AlphaNumber(_CcnetDevice.DeviceState.Identification)));
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private bool HasDefaultCurrency()
        {
            try
            {
                foreach (var currency in _CcnetDevice.DeviceState.AvailableCurrencies)
                {
                    foreach (var backEndCurrency in _Currencies)
                    {
                        if (backEndCurrency.DefaultCurrency && backEndCurrency.Id == currency)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return false;
        }

        private void CheckInactivityTimer(object sender)
        {
            try
            {
                if (CanChangeViewOnIdle)
                {
                    _LastActivity = Utilities.GetLastInputTime();
                    //Log.Debug(_LastActivity);
                    if (_LastActivity > MAX_INACTIVITY_PERIOD)
                    {
                        OpenForm(FormEnum.Products);
                    }
                }
                else
                {
                    //Log.Debug(_LastActivity);
                    _LastActivity = 0;
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void CheckCurrencyTimer(object param)
        {
            Log.Debug("Started thread: " + Thread.CurrentThread.ManagedThreadId);

            if (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    var now = DateTime.Now;
                    var request = new StandardRequest
                    {
                        SystemTime = now,
                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                        Ticks = now.Ticks
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

        public void ForceCheckProducts()
        {
            try
            {
                var now = DateTime.Now;
                var request = new StandardRequest
                {
                    SystemTime = now,
                    TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                    Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                    Ticks = now.Ticks
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

                        //// TODO: Удалить потом
                        //var virtualProduct = new Product
                        //    {
                        //        Assembly = "PageOtherPaymentsCategories.xaml",
                        //        CheckType = (int)CheckTemplateTypes.OtherPayments,
                        //        Id = 65535,
                        //        NameAz = "Other payments",
                        //        NameRu = "Other payments",
                        //        NameEn = "Other payments",
                        //        Name = "OtherPayments"
                        //    };

                        //_Products.Add(virtualProduct);
                    }
                    ProductUpdate();
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

        private void CheckPaymentCategoriesTimer(object param)
        {
            Log.Debug("Started thread: " + Thread.CurrentThread.ManagedThreadId);

            if (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    var now = DateTime.Now;
                    var request = new PaymentCategoriesRequest
                    {
                        SystemTime = now,
                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                        Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                        ProviderName = PaymentProviders.Komtek,
                        Ticks = now.Ticks
                    };

                    var response = _Server.ListPaymentCategoriesExt(request);

                    CheckSignature(response);

                    if (response.ResultCodes == ResultCodes.Ok)
                    {
                        lock (_KomtekPaymentCategories)
                        {
                            _KomtekPaymentCategories.Clear();

                            foreach (var item in response.Categories)
                            {
                                _KomtekPaymentCategories.Add(item);
                            }
                        }
                    }

                    request.ProviderName = PaymentProviders.MoneyMovers;

                    response = _Server.ListPaymentCategoriesExt(request);
                    CheckSignature(response);
                    if (response.ResultCodes == ResultCodes.Ok)
                    {
                        lock (_MoneyMoversPaymentCategories)
                        {
                            _MoneyMoversPaymentCategories.Clear();

                            foreach (var item in response.Categories)
                            {
                                _MoneyMoversPaymentCategories.Add(item);
                            }
                        }
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
            Log.Debug("Started thread: " + Thread.CurrentThread.ManagedThreadId);

            if (_Running && _Init && _AuthTerminal)
            {
                ForceCheckProducts();
            }
        }

        #endregion

        #region PingThread

        private void PingThread()
        {
            Log.Debug("Started thread: " + Thread.CurrentThread.ManagedThreadId);
            while (_Running && _Init && _AuthTerminal)
            {
                try
                {
                    try
                    {
                        // Check public key
                        if (_ServerPublicKey == null)
                        {
                            GetPublicKey();
                        }

                        if (_ServerPublicKey == null)
                        {
                            Log.Warn("Server public key is null!");
                            Thread.Sleep(PING_TIMEOUT);
                            continue;
                        }

                        UpdatePrinterStatus();
                        UpdateTerminalStatusByDevices();

                        var now = DateTime.Now;
                        //var terminalStatus = (int) TerminalCodes.Ok;

                        //if (_TerminalStatus)
                        //{
                        //    terminalStatus = (int) TerminalCodes.Encashment;
                        //} else if ( _TerminalInfo.)
                        var sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey);
                        var request = new PingRequest
                        {
                            CashCodeStatus = _CcnetDevice.DeviceState.ToCashCodeDeviceStatus(),
                            PrinterStatus = _PrinterStatus,
                            SystemTime = now,
                            TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                            TerminalStatus = (int)_TerminalStatus,
                            Sign = sign,
                            CheckCount = 0, //Settings.Default.CheckCounter
                            Ticks = now.Ticks
                        };
                        var result = _Server.Ping(request);

                        Log.Debug(String.Format("Current terminal status: {0}", _TerminalStatus.ToString()));

                        if (result != null && CheckSignature(result))
                        {
                            if (result.ResultCodes != ResultCodes.Ok)
                            {
                                Log.Warn(String.Format("Server return: {0}, {1}", result.ResultCodes, result.Description));

                                if (CanChangeViewOnCommand)
                                {
                                    _TerminalStatus = TerminalCodes.NetworkError;
                                    Log.Warn("Out of order");
                                    OpenForm(FormEnum.OutOfOrder);
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (result.SystemTime > now.AddHours(+1) || result.SystemTime < now.AddHours(-1))
                                    {
                                        Log.Warn("System time changing");
                                    }
                                    Utilities.UpdateSystemTime(result.SystemTime);
                                }
                                catch (Exception exp)
                                {
                                    Log.ErrorException(exp.Message, exp);
                                }

                                switch ((TerminalCommands)result.Command)
                                {
                                    case TerminalCommands.Restart:
                                        Log.Warn("Received command " +
                                                 TerminalCommands.Restart.ToString());
                                        if (CanChangeViewOnCommand)
                                        {
                                            CommandReceived();
                                            var stdRequest = new StandardRequest
                                            {
                                                SystemTime = now,
                                                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                                Ticks = now.Ticks,
                                                Sign =
                                                    Utilities.Sign(Settings.Default.TerminalCode, now,
                                                                   _ServerPublicKey)
                                            };

                                            _Server.TerminalRestarted(stdRequest);
                                            RestartApplication();
                                            return;
                                        }
                                        break;

                                    case TerminalCommands.OutOfService:
                                    case TerminalCommands.Stop:
                                        Log.Warn("Received command " +
                                                 TerminalCommands.OutOfService.ToString());
                                        _TerminalStatus = TerminalCodes.OutOfOrder;
                                        if (CanChangeViewOnCommand)
                                        {
                                            OpenForm(FormEnum.OutOfOrder);
                                            CommandReceived();
                                        }
                                        break;

                                    case TerminalCommands.TestMode:
                                        Log.Warn("Received command " +
                                                 TerminalCommands.TestMode.ToString());
                                        _TerminalStatus = TerminalCodes.TestMode;
                                        if (CanChangeViewOnCommand)
                                        {
                                            OpenForm(FormEnum.TestMode);
                                            CommandReceived();
                                        }
                                        break;

                                    case TerminalCommands.Encash:
                                        Log.Warn("Received command " +
                                                 TerminalCommands.Encash.ToString());
                                        _TerminalStatus = TerminalCodes.Encashment;

                                        if (CanChangeViewOnCommand)
                                        {
                                            DoEncashment();
                                        }
                                        break;

                                    //case TerminalCommands.ResetCheckCounter:
                                    //    Settings.Default.CheckCounter = 0;
                                    //    if ((_CurrentForm == FormEnum.OutOfOrder ||
                                    //         _CurrentForm == FormEnum.TestMode) && _TerminalStatus != TerminalCodes.SystemError)
                                    //    {
                                    //        _TerminalStatus = TerminalCodes.Ok;
                                    //        Log.Warn("Received command " +
                                    //                 ((TerminalCommands)result.Command).ToString());
                                    //        GetPublicKey();
                                    //        GetTerminalInfo();
                                    //        //OpenForm(typeof(FormLanguage));
                                    //        OpenForm(FormEnum.Products);
                                    //    }

                                    //    CommandReceived();
                                    //    break;

                                    case TerminalCommands.ResetCheckCounter:
                                    case TerminalCommands.NormalMode:
                                    case TerminalCommands.Idle:
                                        /* if (_TerminalStatus == TerminalCodes.NetworkError)
                                        {
                                            _TerminalStatus = TerminalCodes.Ok;

                                            Log.Warn("Received command " +
                                                     ((Containers.Enums.TerminalCommands)result.Command).ToString());
                                            GetPublicKey();
                                            GetTerminalInfo();
                                            OpenForm(typeof(FormLanguage));
                                        } else  */
                                        if ((_CurrentForm == FormEnum.OutOfOrder ||
                                             _CurrentForm == FormEnum.TestMode)
                                             && _TerminalStatus != TerminalCodes.SystemError
                                             && _TerminalStatus != TerminalCodes.Updating)
                                        {
                                            _TerminalStatus = TerminalCodes.Ok;
                                            Log.Warn("Received command " +
                                                     ((TerminalCommands)result.Command).ToString());
                                            GetPublicKey();
                                            GetTerminalInfo();
                                            //OpenForm(typeof(FormLanguage));
                                            OpenForm(FormEnum.Products);
                                        }

                                        CommandReceived();
                                        break;
                                }
                                // TODO : Добавить обработчик этого
                            }
                        }
                        else
                        {
                            Log.Error("Result is null. " + (result != null ? result.ResultCodes.ToString() : String.Empty));

                            if (CanChangeViewOnCommand)
                            {
                                _TerminalStatus = TerminalCodes.NetworkError;
                                Log.Warn("Out of order");
                                OpenForm(FormEnum.OutOfOrder);
                            }
                        }
                    }
                    catch (System.ServiceModel.EndpointNotFoundException exp)
                    {
                        // Out of order, if exception
                        if (CanChangeViewOnCommand)
                        {
                            _TerminalStatus = TerminalCodes.NetworkError;
                            Log.Warn("Network error. Out of order");
                            OpenForm(FormEnum.OutOfOrder);
                        }
                        Log.Error(exp.Message);
                    }
                    catch (ThreadAbortException exp)
                    {
                        Log.Error("Aborting Ping Thread. " + exp.Message);
                    }
                    catch (Exception exp)
                    {
                        // Out of order, if exception
                        if (CanChangeViewOnCommand)
                        {
                            _TerminalStatus = TerminalCodes.OutOfOrder;
                            Log.Warn("Out of order");
                            OpenForm(FormEnum.OutOfOrder);
                        }
                        Log.ErrorException(exp.Message, exp);
                    }

                    if (_Running)
                    {
                        Thread.Sleep(PING_TIMEOUT);
                    }
                }
                catch (ThreadAbortException exp)
                {
                    Log.Error("Aborting Ping Thread. " + exp.Message);
                }
                catch (Exception exp)
                {
                    Log.ErrorException(exp.Message, exp);
                }
            }
        }

        private void UpdateTerminalStatusByDevices()
        {
            if (_CurrentForm != FormEnum.MoneyInput)
            {
                Log.Debug(_CcnetDevice.DeviceState);
                if (_CcnetDevice.DeviceState.FatalError)
                {
                    Log.Error(_CcnetDevice.DeviceState.DeviceStateDescription);

                    Log.Warn(_CcnetDevice.DeviceState);
                    _TerminalStatus = TerminalCodes.SystemError;

                    if (CanChangeViewOnError)
                    {
                        //_Init = false;
                        OpenForm(FormEnum.OutOfOrder);
                    }
                }
            }

            //if (Settings.Default.CheckCounter >= TOTAL_CHECK_COUNT)
            //{
            //    _TerminalStatus = TerminalCodes.NoPaper;

            //    Log.Error("No paper");
            //    if (CanChangeViewOnError)
            //    {
            //        //_Init = false;
            //        OpenForm(FormEnum.OutOfOrder);
            //    }
            //}
            //else if (Settings.Default.CheckCounter >= LOW_LEVEL_CHECK_COUNT)
            //{
            //    _TerminalStatus = TerminalCodes.LowPaper;
            //    Log.Warn("Low paper");
            //}
            // 

            //if (/*_PrinterStatus.ErrorState > 0 || */_CcnetDevice.DeviceState.FatalError)
            //{
            //    Log.Warn(_CcnetDevice.DeviceState);
            //    _TerminalStatus = TerminalCodes.SystemError;

            //    if (_CurrentForm != FormEnum.MoneyInput &&
            //    _CurrentForm != FormEnum.PaySuccess &&
            //    _CurrentForm != FormEnum.Encashment &&
            //    _CurrentForm != FormEnum.Activation
            //    )
            //    {
            //        if (_CurrentForm != FormEnum.OutOfOrder)
            //        {
            //            _Init = false;
            //            OpenForm(typeof(FormOutOfOrder));
            //        }
            //    }
            //}
        }

        #endregion

        #region UpdatePrinterStatus

        private void UpdatePrinterStatus()
        {
            int printerStatus = -1, detectedErrorState = -1, extendedDetectedErrorState = -1, extendedPrinterStatus = -1;
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Printer");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if ((bool)queryObj["Default"])
                    {
                        foreach (var data in queryObj.Properties)
                        {
                            switch (data.Name.ToLower())
                            {
                                case "printerstatus":
                                    printerStatus = Convert.ToInt32(data.Value);
                                    break;

                                case "detectederrorstate":
                                    detectedErrorState = Convert.ToInt32(data.Value);
                                    break;

                                case "extendeddetectederrorstate":
                                    extendedDetectedErrorState = Convert.ToInt32(data.Value);
                                    break;

                                case "extendedprinterstatus":
                                    extendedPrinterStatus = Convert.ToInt32(data.Value);
                                    break;

                                //default:
                                //    return data;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            lock (_PrinterStatus)
            {
                _PrinterStatus.ErrorState = detectedErrorState;
                _PrinterStatus.ExtendedErrorState = extendedDetectedErrorState;
                _PrinterStatus.ExtendedStatus = extendedPrinterStatus;
                _PrinterStatus.Status = printerStatus;

                //try
                //{
                //    Log.Debug(String.Format("ErrorState: {0}, ExtendedErrorState: {1}, ExtendedStatus: {2}, Status: {3}",
                //    ((DetectedErrorState)detectedErrorState).ToString(),
                //    ((ExtendedDetectedErrorState)extendedDetectedErrorState).ToString(),
                //    ((ExtendedPrinterStatus)extendedPrinterStatus).ToString(),
                //    ((ExtendedPrinterStatus)printerStatus).ToString()));
                //}
                //catch (Exception exp)
                //{
                //    Log.ErrorException(exp.Message, exp);
                //}

            }
        }

        #endregion

        #region GetTerminalInfo

        public void GetTerminalInfo()
        {
            var now = DateTime.Now;
            var sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey);
            Log.Debug(sign);
            var cmd = new StandardRequest
            {
                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                SystemTime = now,
                Sign = sign,
                Ticks = now.Ticks
            };
            var response = _Server.GetTerminalInfo(cmd);

            if (response != null && response.ResultCodes == ResultCodes.Ok)
            {
                _TerminalInfo = response.Terminal;
            }
            else
            {
                Log.Error("Terminal info is null");
            }

            try
            {
                if (_CcnetDevice.DeviceState.AvailableCurrencies != null && _CcnetDevice.DeviceState.AvailableCurrencies.Count > 0)
                {
                    Log.Info(String.Join(";", _CcnetDevice.DeviceState.AvailableCurrencies));
                }
                else
                {
                    _CcnetDevice.DeviceState.AvailableCurrencies = new List<string>();
                    Log.Warn("Currency is null");
                }

                now = DateTime.Now;
                var versionRequest = new TerminalVersionExtRequest
                {
                    TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                    SystemTime = now,
                    Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                    Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    AvailableCurrencies = _CcnetDevice.DeviceState.AvailableCurrencies.ToArray(),
                    CashcodeVersion = _CcnetDevice.DeviceState.Identification,
                    Ticks = now.Ticks
                };

                var versionResponse = _Server.UpdateTerminalVersionExt(versionRequest);
                if (versionResponse == null || versionResponse.ResultCodes != ResultCodes.Ok)
                {
                    Log.Error("Can't update version");
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private StandardResult CommandReceived()
        {
            Log.Debug(((int)_TerminalStatus));
            var now = DateTime.Now;
            var cmd = new StandardRequest
            {
                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                SystemTime = now,
                CommandResult = ((int)_TerminalStatus),
                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                Ticks = now.Ticks
            };
            return _Server.CommandReceived(cmd);
        }

        #endregion

        #region DoEncashment

        private void DoEncashment()
        {
            Log.Debug("Started thread: " + Thread.CurrentThread.ManagedThreadId);

            try
            {
                OpenForm(FormEnum.Encashment);

                var secondResult = CommandReceived();
                CheckSignature(secondResult);

                var request = new Encashment();

                var currList = new List<EncashmentCurrency>();
                if (!App.TestVersion)
                {
                    foreach (var currency in _Currencies)
                    {
                        var total = _Db.GetCasseteTotal(currency.Name);

                        var item = new EncashmentCurrency { Amount = total, Currency = currency.Id };
                        Log.Info(String.Format("Encashment. {0} {1}", total, currency.Id));
                        currList.Add(item);
                    }
                }

                var now = DateTime.Now;
                request.SystemTime = now;
                request.TerminalId = Convert.ToInt32(Settings.Default.TerminalCode);
                request.Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey);
                request.Ticks = now.Ticks;

                request.Currencies = currList.ToArray();

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

        #endregion

        #region SendPaymentThread

        private void SendPaymentThread()
        {
            Log.Debug("Started thread: " + Thread.CurrentThread.ManagedThreadId);
            var listToDeletePayments = new Queue<long>();
            var listToDeleteCreditRequests = new Queue<long>();

            try
            {
                while (_Running && _Init && _AuthTerminal)
                {
                    try
                    {
                        _Db.UpdateNonConfirmed(DateTime.Now.AddMinutes(-5));

                        var list = _Db.GetTransactions();

                        foreach (var row in list)
                        {
                            Log.Info(
                                String.Format("Trying to send. TransactionId: {0}, Amount: {1} {2}, InsertDate: {3}",
                                              row.TransactionId, row.Amount, row.Currency, row.InsertDate));

                            if (!App.TestVersion)
                            {
                                var values = _Db.GetPaymentValues(row.Id);
                                var banknotes = _Db.GetPaymentBanknotes(row.Id);

                                var dateTime = DateTime.Now;
                                try
                                {
                                    dateTime = DateTime.Parse(row.InsertDate as String);
                                }
                                catch (Exception exp)
                                {
                                    Log.ErrorException(exp.Message, exp);
                                }

                                var request = new TerminalPaymentInfo
                                    {
                                        TerminalDate = dateTime,
                                        SystemTime = dateTime,
                                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                        TransactionId = row.TransactionId,
                                        ProductId = (int)row.ProductId,
                                        Currency = row.Currency,
                                        CurrencyRate = (float)row.CurrencyRate,
                                        Amount = (int)row.Amount,
                                        CreditNumber = row.IsCreditNumberNull() ? String.Empty : row.CreditNumber,
                                        Sign = Utilities.Sign(Settings.Default.TerminalCode, dateTime, _ServerPublicKey),
                                        OperationType = Convert.ToInt32(row.OperationType),
                                        PaymentServiceId = Convert.ToInt32(row.IsServiceIdNull() ? 0 : row.ServiceId),
                                        Ticks = dateTime.Ticks
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

                                var response = _Server.Pay(request);

                                if (response != null && response.ResultCodes == ResultCodes.Ok && CheckSignature(response))
                                {
                                    Log.Info("Send to server is okay");
                                    listToDeletePayments.Enqueue(row.Id);
                                }
                                else if (response != null)
                                {
                                    Log.Warn(String.Format("Server answer: {0}, {1}", response.ResultCodes,
                                                           response.Description));
                                }
                                else
                                {
                                    Log.Warn("Response is null");
                                }
                            }
                            else
                            {
                                Log.Info("Test version. No sending to server");
                                listToDeletePayments.Enqueue(row.Id);
                            }
                        }


                        var listRequests = _Db.ListCreditRequest();
                        if (listRequests != null)
                        {
                            foreach (var row in listRequests)
                            {
                                var dateTime = DateTime.Now;
                                Log.Info("Trying to send phone number: " + row.Value);

                                var request = new CreditRequest
                                    {
                                        SystemTime = dateTime,
                                        TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                        Phone = row.Value,
                                        Sign = Utilities.Sign(Settings.Default.TerminalCode, dateTime, _ServerPublicKey),
                                        Ticks = dateTime.Ticks
                                    };

                                var response = _Server.CreditRequest(request);

                                if (response != null && response.ResultCodes == ResultCodes.Ok &&
                                    CheckSignature(response))
                                {
                                    Log.Info("Send to server is okay");
                                    listToDeleteCreditRequests.Enqueue(row.Id);
                                }
                                else if (response != null)
                                {
                                    Log.Warn(String.Format("Server answer: {0}, {1}", response.ResultCodes,
                                                           response.Description));
                                }
                                else
                                {
                                    Log.Warn("Response is null");
                                }
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
                        if (listToDeletePayments.Count > 0)
                        {
                            foreach (var item in listToDeletePayments)
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

                    try
                    {
                        if (listToDeleteCreditRequests.Count > 0)
                        {
                            foreach (var item in listToDeleteCreditRequests)
                            {
                                _Db.DeleteCreditRequest(item);
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

                    if (_Running)
                    {
                        Thread.Sleep(SEND_PAYMENT_TIMEOUT);
                    }


                }
            }
            catch (ThreadAbortException exp)
            {
                Log.Error("Aborting SendPayment Thread. " + exp.Message);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        #endregion

        #region CheckSignature

        private bool CheckSignature(StandardResult request)
        {
            try
            {
                if (String.IsNullOrEmpty(Settings.Default.TerminalCode))
                {
                    Log.Debug("Terminal code is null");
                }
                if (String.IsNullOrEmpty(request.Sign))
                {
                    Log.Debug("Sign is null");
                }
                if (_LocalKeys == null)
                {
                    Log.Debug("LocalKeys is null");
                }

                if (Utilities.CheckSignature(Settings.Default.TerminalCode, request.Ticks, request.Sign, _LocalKeys))
                {
                    return true;
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return false;
        }

        #endregion

        public void StartTimers()
        {
            try
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<object>(CheckCurrencyTimer), null);
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<object>(CheckBillTableTimer), null);
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<object>(CheckProductsTimer), null);
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<object>(CheckPaymentCategoriesTimer), null);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        #region DoClose

        private void DoClose()
        {
            Log.Debug("Started thread: " + Thread.CurrentThread.ManagedThreadId);

            _Running = false;

            try
            {
                if (_CheckCurrencyTimer != null)
                {
                    _CheckCurrencyTimer.Dispose();
                }
                if (_CheckProductsTimer != null)
                {
                    _CheckProductsTimer.Dispose();
                }
                if (_CheckPaymentCategoriesTimer != null)
                {
                    _CheckPaymentCategoriesTimer.Dispose();
                }
                if (_CheckInactivityTimer != null)
                {
                    _CheckInactivityTimer.Dispose();
                }
                if (_CheckApplicationUpdateTimer != null)
                {
                    _CheckApplicationUpdateTimer.Dispose();
                }
                if (_CheckCheckTemplateTimer != null)
                {
                    _CheckCheckTemplateTimer.Dispose();
                }
                if (_CheckBillTableTimer != null)
                {
                    _CheckBillTableTimer.Dispose();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }


            try
            {
                _CcnetDevice.Close();
                _CcnetDevice.Dispose();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                if (_PingThread != null)
                {
                    _PingThread.Join(1000);
                    _PingThread.Abort();
                    Log.Debug("Aborting");
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            Log.Debug("SendPaymentThread");

            try
            {
                if (_SendPaymentThread != null)
                {
                    Log.Debug("Join");
                    _SendPaymentThread.Join(1000);
                    Log.Debug("Abort");
                    _SendPaymentThread.Abort();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }

        private void NavigationWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DoClose();
        }

        #endregion

        #region Can do that

        private bool CanChangeViewOnError
        {
            get
            {
                lock (_CurrentForm)
                {
                    if (_CurrentForm != FormEnum.MoneyInput &&
                        _CurrentForm != FormEnum.PaySuccess &&
                        _CurrentForm != FormEnum.Encashment &&
                        _CurrentForm != FormEnum.Activation &&
                        _CurrentForm != FormEnum.OutOfOrder &&
                        _CurrentForm != FormEnum.Progress
                        )
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private bool CanChangeViewOnCommand
        {
            get
            {
                lock (_CurrentForm)
                {
                    if (_CurrentForm != FormEnum.MoneyInput
                        && _CurrentForm != FormEnum.PaySuccess
                        && _CurrentForm != FormEnum.Encashment
                        && _CurrentForm != FormEnum.Progress
                        && _TerminalStatus != TerminalCodes.Updating)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool CanChangeViewOnIdle
        {
            get
            {
                lock (_CurrentForm)
                {
                    if (_CurrentForm != FormEnum.MoneyInput &&
                        _CurrentForm != FormEnum.Products &&
                        _CurrentForm != FormEnum.TestMode &&
                        _CurrentForm != FormEnum.Encashment &&
                        _CurrentForm != FormEnum.OutOfOrder &&
                        _CurrentForm != FormEnum.Activation &&
                         _CurrentForm != FormEnum.Progress &&
                        _CurrentForm != FormEnum.Language)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion
    }
}
