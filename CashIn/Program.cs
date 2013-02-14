using System;
using System.ServiceModel;
using CashIn.Properties;
using CashInCore;
using Db;
using NLog;

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
