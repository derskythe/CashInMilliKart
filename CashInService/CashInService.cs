using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using CashInCore;
using CashInService.Properties;
using Db;
using NLog;

namespace CashInService
{
    public partial class CashInService : ServiceBase
    {
        private static ServiceHost _CashInTerminalService;
        private static ServiceHost _CashInAdminService;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private PaymentServiceSender _PaymentServiceSender;

        public CashInService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("Starting");

            try
            {
                Log.Info("Init DB");
                OracleDb.Init(Settings.Default.OracleUser, Settings.Default.OraclePassword, Settings.Default.OracleDb);
                OracleDb.Instance.CheckConnection();

                CashInServer.InitMultiPaymentService(Settings.Default.MultiPaymentUsername,
                                                           Settings.Default.MultiPaymentPassword);

                Log.Info("Open CashInTerminalService");
                _CashInTerminalService = new ServiceHost(typeof(CashInServer));
                _CashInTerminalService.Open();

                Log.Info("Open CashInAdminService");
                _CashInAdminService = new ServiceHost(typeof(CashInAdminServer));
                _CashInAdminService.Open();

                _PaymentServiceSender = new PaymentServiceSender(Settings.Default.MultiPaymentUsername,
                                                                 Settings.Default.MultiPaymentPassword);
                _PaymentServiceSender.Start();
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
                Thread.Sleep(1000);
                throw;
            }
        }

        protected override void OnStop()
        {
            Log.Info("Stoping CashInService");
            try
            {
                if (_CashInTerminalService != null)
                {
                    _CashInTerminalService.Close();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            try
            {
                if (_CashInAdminService != null)
                {
                    _CashInAdminService.Close();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }           

            try
            {
                if (_PaymentServiceSender != null)
                {
                    _PaymentServiceSender.Stop();
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            Log.Info("Stopped CashInService");
        }
    }
}
