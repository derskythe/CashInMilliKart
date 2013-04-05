using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CashInTerminal.CashIn;
using CashInTerminal.Enums;
using CashInTerminal.Properties;
using NLog;
using Org.BouncyCastle.Crypto;
using crypto;
using PrinterStatus = CashInTerminal.CashIn.PrinterStatus;
using ResultCodes = CashInTerminal.CashIn.ResultCodes;
using Timer = System.Threading.Timer;
using TerminalCodes = Containers.Enums.TerminalCodes;
using ThreadState = System.Threading.ThreadState;

namespace CashInTerminal
{
    public partial class FormMdiMain : Form
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        // 
        private AsymmetricCipherKeyPair _LocalKeys;
        private AsymmetricKeyParameter _ServerPublicKey;
        private CCNETDevice _CcnetDevice;
        private readonly PrinterStatus _PrinterStatus = new PrinterStatus();
        private bool _Init;
        private bool _AuthTerminal;
        private TerminalCodes _TerminalStatus;
        private Terminal _TerminalInfo = new Terminal();
        private LocalDb _Db;
        private bool _Running = true;
        private CashIn.ClientInfo[] _Clients;
        private Dictionary<int, List<ds.TemplateFieldRow>> _CheckTemplates = new Dictionary<int, List<ds.TemplateFieldRow>>();
        private String _SelectedLanguage = InterfaceLanguages.Az;

        private ClientInfo _ClientInfo = new ClientInfo();

        //private delegate void CloseFormDelegate(object item);

        private delegate void OpenFormDelegate(Type item);

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

        private CashInServer _Server;

        //private TextBox _PanelActivationFocus;
        //private TextBox _PanelDebitInfoActivationFocus;
        //private TextBox _PanelClientCodeFocus;
        private Form _CurrentForm;

        #region Threads

        private Thread _PingThread;
        private Thread _SendPaymentThread;
        private Thread _TerminationThread;
        private Timer _CheckCurrencyTimer;
        private Timer _CheckProductsTimer;
        private Timer _CheckCheckTemplateTimer;
        private Timer _CheckInactivityTimer;
        private Timer _CheckApplicationUpdateTimer;
        private const int PING_TIMEOUT = 30 * 1000;
        private const int SEND_PAYMENT_TIMEOUT = 10 * 1000;
        private const int CHECK_CURRENCY_TIMER = 30 * 60 * 1000;
        private const int CHECK_CHECK_TEMPLATE_TIMER = 60 * 1000;
        private const int CHECK_PRODUCTS_TIMER = 60 * 60 * 1000;
        private const int CHECK_INACTIVITY = 10 * 1000;
        private const int CHECK_APPLICATION_UPDATE_TIMER = 60 * 1000;
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

        private void FormMdiMainLoad(object sender, EventArgs e)
        {
            OpenForm(typeof(FormOutOfOrder));
            ProductUpdate += delegate { };            

            foreach (Control control in Controls)
            {
                var client = control as MdiClient;
                if (client != null)
                {
                    client.BackColor = DefaultBackColor;
                    break;
                }
            }
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
                                   Path.GetDirectoryName(Application.ExecutablePath),
                                   assembly.GetName().Version,
                                   version
                                   ));

            var startUpThread = new Thread(DoStartUp);
            startUpThread.Start();
        }

        private void DoStartUp()
        {
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
                _Db.CountCasseteBanknotes();
                LoadCheckTemplates();
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
                OpenForm(typeof(FormActivation));
            }
            else if (!_Init)
            {
                _TerminalStatus = TerminalCodes.OutOfOrder;
                OpenForm(typeof(FormOutOfOrder));
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
            _CheckInactivityTimer = new Timer(CheckInactivityTimer, null, CHECK_INACTIVITY, CHECK_INACTIVITY);
            _CheckCheckTemplateTimer = new Timer(CheckTemplateTimer, null, 0, CHECK_CHECK_TEMPLATE_TIMER);
            _CheckApplicationUpdateTimer = new Timer(ApplicationUpdateTimer, null, 0, CHECK_APPLICATION_UPDATE_TIMER);

        }

        #region Application Update

        private void ApplicationUpdateTimer(object state)
        {
            try
            {
                if (_CurrentForm != null)
                {
                    if (_CurrentForm.Name != FormEnum.MoneyInput &&
                        _CurrentForm.Name != FormEnum.TestMode && _CurrentForm.Name != FormEnum.Encashment &&
                        _CurrentForm.Name != FormEnum.OutOfOrder && _CurrentForm.Name != FormEnum.PaySuccess)
                    {
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
                        catch (DeploymentDownloadException)
                        {
                            Log.Error("Couldn't retrieve info on this app");
                            return;
                        }
                        catch (InvalidDeploymentException)
                        {
                            Log.Error("Cannot check for a new version. ClickOnce deployment is corrupt!");
                            return;
                        }
                        catch (InvalidOperationException)
                        {
                            Log.Error("Cannot check for a new version. ClickOnce deployment is corrupt!");
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
                ApplicationDeployment updateCheck = ApplicationDeployment.CurrentDeployment;
                updateCheck.Update();
                Log.Info("The application has been upgraded, and will now restart.");

                RestartApplication();
            }
            catch (DeploymentDownloadException exp)
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
            OpenForm(typeof(FormOutOfOrder));
            _Running = false;

            if (_TerminationThread == null || _TerminationThread.ThreadState != ThreadState.Running)
            {
                _TerminationThread = new Thread(TerminationThread);
                _TerminationThread.Start();
            }
        }

        private void TerminationThread()
        {
            Thread.Sleep(5);
            DoClose();
            Log.Debug("Thread.Sleep(150)");
            Thread.Sleep(150);
            Log.Debug("Application.Restart");
            Application.Restart();
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
            //_CcnetDevice.ReadCommand += CcnetDeviceOnReadCommand;
            _CcnetDevice.Open(port, CCNETPortSpeed.S9600);
            _CcnetDevice.Init();


        }

        private void CcnetDeviceOnBillStacked(CCNETDeviceState ccnetDeviceState)
        {
            Log.Debug(ccnetDeviceState.ToString());
        }

        //private void CcnetDeviceOnReadCommand(CCNETDeviceState ccnetDeviceState)
        //{
        //    Log.Debug(ccnetDeviceState);
        //}

        //private void CloseForm(object form)
        //{
        //    if (_CurrentForm != null && _CurrentForm.InvokeRequired)
        //    {
        //        _CurrentForm.Invoke(new CloseFormDelegate(CloseForm), form);
        //    }
        //    else
        //    {
        //        if (_CurrentForm != null)
        //        {
        //            _CurrentForm.Close();
        //        }

        //        _CurrentForm = (Form)form;
        //    }
        //}

        //private void OpenFormI(object form)
        //{
        //    var f = (Form)form;
        //    if (f == null)
        //    {
        //        return;
        //    }
        //    if (f.InvokeRequired)
        //    {
        //        f.Invoke(new OpenFormDelegate(OpenFormI), f);
        //    }
        //    else
        //    {
        //        f.WindowState = FormWindowState.Maximized;
        //        f.Show();
        //    }
        //}

        //private void SetFormParent(object form)
        //{
        //    var f = (Form)form;
        //    if (f == null)
        //    {
        //        return;
        //    }

        //    if (InvokeRequired)
        //    {
        //        Invoke(new SetFormParentDelegate(SetFormParent), f);
        //    }
        //    else
        //    {
        //        f.MdiParent = this;
        //    }
        //}

        public void OpenForm(Type f)
        {
            Log.Debug("OpenForm. New form " + f.Name);
            var t = new StackTrace();
            if (t.GetFrames() != null)
            {
                var stack = new StringBuilder();
                foreach (StackFrame frame in t.GetFrames())
                {
                    stack.Append("Method: ")
                         .Append(frame.GetMethod())
                         .Append(" \tLine: ")
                         .Append(frame.GetFileLineNumber()).Append(" ");
                }

                Log.Debug(stack.ToString());
            }
            if (InvokeRequired)
            {
                var form = new OpenFormDelegate(OpenForm);
                Invoke(form, f);
            }
            else
            {
                //if (_CurrentForm != null && f.Name != _CurrentForm.Name)
                //{
                var inst = Activator.CreateInstance(f);
                var child = (Form)inst;
                child.MdiParent = this;
                child.Show();
                child.Visible = true;

                if (_CurrentForm != null)
                {
                    Log.Debug("CurrentForm: " + _CurrentForm.Name);
                    _CurrentForm.Close();
                }

                _CurrentForm = (Form)inst;
                //}
            }

            //CloseForm(f);
            //SetFormParent(f);
            //OpenFormI(f);

            //if (_CurrentForm != null)
            //{
            //    _CurrentForm.Close();
            //}

            //_CurrentForm = f;
            //f.MdiParent = this;
            //f.Show();
            //f.WindowState = FormWindowState.Maximized;
        }

        public void MinimizeAllChildren()
        {
            foreach (var f in MdiChildren)
            {
                f.WindowState = FormWindowState.Minimized;
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
                            Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                        };

                    var response = _Server.ListCheckTemplateDigest(request);

                    CheckSignature(response);

                    if (response.ResultCodes == ResultCodes.Ok)
                    {
                        //lock (_Currencies)
                        //{
                        //    _Currencies.Clear();
                        //    foreach (var item in response.Currencies)
                        //    {
                        //        _Currencies.Add(item);
                        //    }
                        //}
                        bool oldData = false;
                        foreach (var template in response.Templates)
                        {
                            var dbValue = _Db.GetCheckTemplate(template.Id);

                            if (dbValue == null || dbValue.UpdateDate < template.UpdateDate)
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
                                    Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
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

                UpdateCheckTemplateKey((int)CheckTemplateTypes.Encashment, InterfaceLanguages.Az);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.Encashment, InterfaceLanguages.En);
                UpdateCheckTemplateKey((int)CheckTemplateTypes.Encashment, InterfaceLanguages.Ru);
            }
        }

        private void UpdateCheckTemplateKey(int productId, String language)
        {
            var templateList = _Db.GetCheckTemplateByType(productId, language);

            foreach (var row in templateList)
            {
                var key = GetCheckTemplateHashCode((int)row.Type, row.Language);
                var fields = _Db.ListTemplateFields(row.Id);

                _CheckTemplates.Add(key, fields);
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

        private void CheckInactivityTimer(object param)
        {
            try
            {
                if (_CurrentForm != null)
                {
                    if (_CurrentForm.Name != FormEnum.MoneyInput && _CurrentForm.Name != FormEnum.Products &&
                        _CurrentForm.Name != FormEnum.TestMode && _CurrentForm.Name != FormEnum.Encashment &&
                        _CurrentForm.Name != FormEnum.OutOfOrder && _CurrentForm.Name != FormEnum.Activation &&
                        _CurrentForm.Name != FormEnum.Language)
                    {
                        _LastActivity = Utilities.GetLastInputTime();
                        if (_LastActivity > MAX_INACTIVITY_PERIOD)
                        {
                            OpenForm(typeof(FormProducts));
                        }
                    }
                    else
                    {
                        _LastActivity = 0;
                    }
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
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

        public void ForceCheckProducts()
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

        private void CheckProductsTimer(object param)
        {
            if (_Running && _Init && _AuthTerminal)
            {
                ForceCheckProducts();
            }
        }

        #region PingThread

        private void PingThread()
        {
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
                            Thread.Sleep(PING_TIMEOUT);
                            continue;
                        }

                        UpdatePrinterStatus();


                        var now = DateTime.Now;
                        //var terminalStatus = (int) TerminalCodes.Ok;

                        //if (_TerminalStatus)
                        //{
                        //    terminalStatus = (int) TerminalCodes.Encashment;
                        //} else if ( _TerminalInfo.)
                        var request = new PingRequest
                            {
                                CashCodeStatus = _CcnetDevice.DeviceState.ToCashCodeDeviceStatus(),
                                PrinterStatus = _PrinterStatus,
                                SystemTime = now,
                                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                TerminalStatus = (int)_TerminalStatus,
                                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
                            };
                        var result = _Server.Ping(request);

                        Log.Debug(String.Format("Current terminal status: {0}", _TerminalStatus.ToString()));

                        if (result != null && CheckSignature(result))
                        {
                            if (result.ResultCodes != ResultCodes.Ok)
                            {
                                Log.Warn(String.Format("Server return: {0}, {1}", result.ResultCodes, result.Description));

                                if (_CurrentForm.Name != FormEnum.MoneyInput && _CurrentForm.Name != FormEnum.PaySuccess &&
                                    _CurrentForm.Name != FormEnum.Encashment)
                                {
                                    _TerminalStatus = TerminalCodes.NetworkError;
                                    Log.Warn("Out of order");
                                    OpenForm(typeof(FormOutOfOrder));
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

                                switch ((Containers.Enums.TerminalCommands)result.Command)
                                {
                                    case Containers.Enums.TerminalCommands.Restart:
                                        Log.Warn("Received command " +
                                                 Containers.Enums.TerminalCommands.Restart.ToString());
                                        if (_CurrentForm.Name != FormEnum.MoneyInput &&
                                            _CurrentForm.Name != FormEnum.PaySuccess)
                                        {
                                            CommandReceived();
                                            var stdRequest = new StandardRequest
                                                {
                                                    SystemTime = now,
                                                    TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                                                    Sign =
                                                        Utilities.Sign(Settings.Default.TerminalCode, now,
                                                                       _ServerPublicKey)
                                                };

                                            _Server.TerminalRestarted(stdRequest);
                                            RestartApplication();
                                            return;
                                        }
                                        break;

                                    case Containers.Enums.TerminalCommands.OutOfService:
                                    case Containers.Enums.TerminalCommands.Stop:
                                        Log.Warn("Received command " +
                                                 Containers.Enums.TerminalCommands.OutOfService.ToString());
                                        _TerminalStatus = TerminalCodes.OutOfOrder;
                                        if (_CurrentForm.Name != FormEnum.MoneyInput &&
                                            _CurrentForm.Name != FormEnum.PaySuccess)
                                        {
                                            OpenForm(typeof(FormOutOfOrder));
                                            CommandReceived();
                                        }
                                        break;

                                    case Containers.Enums.TerminalCommands.TestMode:
                                        Log.Warn("Received command " +
                                                 Containers.Enums.TerminalCommands.TestMode.ToString());
                                        _TerminalStatus = TerminalCodes.TestMode;
                                        if (_CurrentForm.Name != FormEnum.MoneyInput &&
                                            _CurrentForm.Name != FormEnum.PaySuccess)
                                        {
                                            OpenForm(typeof(FormTestMode));
                                            CommandReceived();
                                        }
                                        break;

                                    case Containers.Enums.TerminalCommands.Encash:
                                        Log.Warn("Received command " +
                                                 Containers.Enums.TerminalCommands.Encash.ToString());
                                        _TerminalStatus = TerminalCodes.Encashment;

                                        if (_CurrentForm.Name != FormEnum.MoneyInput &&
                                            _CurrentForm.Name != FormEnum.PaySuccess)
                                        {
                                            DoEncashment();
                                        }
                                        break;

                                    case Containers.Enums.TerminalCommands.NormalMode:
                                    case Containers.Enums.TerminalCommands.Idle:
                                        /* if (_TerminalStatus == TerminalCodes.NetworkError)
                                        {
                                            _TerminalStatus = TerminalCodes.Ok;

                                            Log.Warn("Received command " +
                                                     ((Containers.Enums.TerminalCommands)result.Command).ToString());
                                            GetPublicKey();
                                            GetTerminalInfo();
                                            OpenForm(typeof(FormLanguage));
                                        } else  */
                                        if ((_CurrentForm.Name == FormEnum.OutOfOrder ||
                                             _CurrentForm.Name == FormEnum.TestMode))
                                        {
                                            _TerminalStatus = TerminalCodes.Ok;
                                            Log.Warn("Received command " +
                                                     ((Containers.Enums.TerminalCommands)result.Command).ToString());
                                            GetPublicKey();
                                            GetTerminalInfo();
                                            //OpenForm(typeof(FormLanguage));
                                            OpenForm(typeof(FormProducts));
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

                            if (_CurrentForm.Name != FormEnum.MoneyInput && _CurrentForm.Name != FormEnum.PaySuccess &&
                                _CurrentForm.Name != FormEnum.Encashment)
                            {
                                _TerminalStatus = TerminalCodes.NetworkError;
                                Log.Warn("Out of order");
                                OpenForm(typeof(FormOutOfOrder));
                            }
                        }
                    }
                    catch (System.ServiceModel.EndpointNotFoundException exp)
                    {
                        // Out of order, if exception
                        if (_CurrentForm.Name != FormEnum.MoneyInput
                            && _CurrentForm.Name != FormEnum.PaySuccess
                            && _CurrentForm.Name != FormEnum.Encashment)
                        {
                            _TerminalStatus = TerminalCodes.NetworkError;
                            Log.Warn("Network error. Out of order");
                            OpenForm(typeof(FormOutOfOrder));
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
                        if (_CurrentForm.Name != FormEnum.MoneyInput && _CurrentForm.Name != FormEnum.PaySuccess && _CurrentForm.Name != FormEnum.Encashment)
                        {
                            _TerminalStatus = TerminalCodes.OutOfOrder;
                            Log.Warn("Out of order");
                            OpenForm(typeof(FormOutOfOrder));
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

        public void GetTerminalInfo()
        {
            var now = DateTime.Now;
            var cmd = new StandardRequest
            {
                TerminalId = Convert.ToInt32(Settings.Default.TerminalCode),
                SystemTime = now,
                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
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

            var versionRequest = new TerminalVersionRequest
                {
                    TerminalId = cmd.TerminalId,
                    SystemTime = now,
                    Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                    Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                };

            var versionResponse = _Server.UpdateTerminalVersion(versionRequest);
            if (versionResponse == null || versionResponse.ResultCodes != ResultCodes.Ok)
            {
                Log.Error("Can't update version");
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
                Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey)
            };
            return _Server.CommandReceived(cmd);
        }

        #region DoEncashment

        private void DoEncashment()
        {
            try
            {
                OpenForm(typeof(FormEncashment));

                var secondResult = CommandReceived();
                CheckSignature(secondResult);

                var request = new Encashment();

                var currList = new List<EncashmentCurrency>();
                foreach (var currency in _Currencies)
                {
                    var total = _Db.GetCasseteTotal(currency.Name);

                    var item = new EncashmentCurrency { Amount = total, Currency = currency.Id };
                    Log.Info(String.Format("Encashment. {0} {1}", total, currency.Id));
                    currList.Add(item);
                }

                var now = DateTime.Now;
                request.SystemTime = now;
                request.TerminalId = Convert.ToInt32(Settings.Default.TerminalCode);
                request.Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey);

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
            var listToDelete = new Queue<long>();

            try
            {
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
                                    CreditNumber = _ClientInfo.Client.CreditNumber,
                                    Sign = Utilities.Sign(Settings.Default.TerminalCode, now, _ServerPublicKey),
                                    OperationType = (int)_ClientInfo.PaymentOperationType                                    
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
                                Log.Info("Send to server is okay");
                                listToDelete.Enqueue(row.Id);
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

                if (Utilities.CheckSignature(Settings.Default.TerminalCode, request.SystemTime, request.Sign, _LocalKeys))
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

        public void StartTimers()
        {
            var checkCurrencyTimerCaller = new CheckCurrencyTimerCaller(CheckCurrencyTimer);
            checkCurrencyTimerCaller.Invoke(null);

            var checkProductsTimerCaller = new CheckProductsTimerCaller(CheckProductsTimer);
            checkProductsTimerCaller.Invoke(null);
        }

        private void FormMdiMainFormClosing(object sender, FormClosingEventArgs e)
        {
            DoClose();
        }

        #region DoClose

        private void DoClose()
        {
            _Running = false;

            _CheckCurrencyTimer.Dispose();
            _CheckProductsTimer.Dispose();
            _CheckInactivityTimer.Dispose();
            _CheckApplicationUpdateTimer.Dispose();
            _CheckCheckTemplateTimer.Dispose();

            _CcnetDevice.Close();
            _CcnetDevice.Dispose();

            if (_PingThread != null)
            {
                _PingThread.Join(1000);
                _PingThread.Abort();
                Log.Debug("Aborting");
            }

            Log.Debug("SendPaymentThread");
            if (_SendPaymentThread != null)
            {
                Log.Debug("Join");
                _SendPaymentThread.Join(1000);
                Log.Debug("Abort");
                _SendPaymentThread.Abort();
            }
        }

        #endregion


        #region Paint 

        private readonly Color _Color1 = Color.FromArgb(215, 232, 248);
        private readonly Color _Color2 = Color.FromArgb(207, 226, 246);
        private readonly Color _Color3 = Color.FromArgb(196, 219, 244);
        private readonly Color _Color4 = Color.FromArgb(185, 212, 241);
        private readonly Color _Color5 = Color.FromArgb(178, 208, 240);
        private readonly Color _Color6 = Color.FromArgb(171, 202, 238);
        private readonly Color _Color7 = Color.FromArgb(164, 197, 236);
        private readonly Color _Color8 = Color.FromArgb(157, 192, 233);
        private readonly Color _Color9 = Color.FromArgb(154, 188, 231);
        private readonly Color _Color10 = Color.FromArgb(151, 185, 229);
        private readonly Color _Color11 = Color.FromArgb(148, 183, 228);

        private void FormMdiMainPaint(object sender, PaintEventArgs e)
        {
            using (var br = new LinearGradientBrush(ClientRectangle, _Color1, _Color11, 0, false))
            {
                var cb = new ColorBlend
                {
                    Positions = new[] { 0f, 1 / 10f, 2 / 10f, 3 / 10f, 4 / 10f, 5 / 10f, 6 / 10f, 7 / 10f, 8 / 10f, 9 / 10f, 1f },
                    Colors = new[]
                            {
                                _Color1,
                                _Color2,
                                _Color3,
                                _Color4,
                                _Color5,
                                _Color6,
                                _Color7,
                                _Color8,
                                _Color9,
                                _Color10,
                                _Color11
                            }
                };

                br.InterpolationColors = cb;
                // rotate
                //br.RotateTransform(45);
                // paint
                e.Graphics.FillRectangle(br, ClientRectangle);
            }
        }

        #endregion
    }
}
