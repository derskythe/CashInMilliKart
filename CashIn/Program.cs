using System;
using System.ServiceModel;
using CashIn.Properties;
using CashInCore;
using Db;
using NLog;
using crypto;

namespace CashIn
{
    class Program
    {
        private static ServiceHost _CashInTerminalService;
        private static ServiceHost _CashInAdminService;

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        static void Main(string[] args)
        {
            Log.Info("Starting");

            OracleDb.Init(Settings.Default.OracleUser, Settings.Default.OraclePassword, Settings.Default.OracleDb);
            OracleDb.Instance.CheckConnection();

            

            var user = OracleDb.Instance.GetUser("skif");

            String salt = String.Empty;
            String encPass = String.Empty;


            salt = Wrapper.GenerateSalt();
            encPass = Wrapper.ComputeHash("dfl90nhl", salt);
            
            OracleDb.Instance.SaveUser(user.Id, user.Username, encPass, salt);
            CashInServer.InitMultiPaymentService(Settings.Default.MultiPaymentUsername,
                                                            Settings.Default.MultiPaymentPassword);

            _CashInTerminalService = new ServiceHost(typeof(CashInServer));
            _CashInTerminalService.Open();

            _CashInAdminService = new ServiceHost(typeof(CashInAdminServer));
            _CashInAdminService.Open();

            Log.Info("Press any key to stop");
            Console.Read();

            _CashInTerminalService.Close();
            _CashInAdminService.Close();
        }
    }
}
