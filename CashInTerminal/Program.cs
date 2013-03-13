using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CashInTerminal.Properties;
using NLog;

namespace CashInTerminal
{
    static class Program
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //bool isRestart = false;
            //string restartProcessId = String.Empty;
            //if (args != null && args.Length > 0)
            //{
            //    foreach (string s in args)
            //    {
            //        if (!String.IsNullOrEmpty(s))
            //        {
            //            switch (s)
            //            {
            //                case "/restart":
            //                    isRestart = true;
            //                    break;

            //                default:
            //                    restartProcessId = s;
            //                    break;
            //            }
            //        }
            //    }
            //}

            //if (isRestart)
            //{
            //bool exceptionOccured = false;
            //try
            //{
            //    // Find old proc and wait
            //    Process oldProcess = Process.GetProcessById(Convert.ToInt32(restartProcessId));
            //    oldProcess.WaitForExit(30000);
            //}
            //catch (Exception exp)
            //{
            //    Log.ErrorException(exp.Message, exp);
            //    exceptionOccured = true;
            //}

            //if (exceptionOccured)
            //{
            try
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        process.WaitForExit(30000);
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
            //    }
            //}


            bool mutexCreated = true;
            using (var mutex = new Mutex(true, "CashInTerminal", out mutexCreated))
            {
                if (mutexCreated)
                {
                    //MakePortable(Properties.Settings.Default);

                    if (!Directory.Exists(Settings.Default.ApplicationPath))
                    {
                        Directory.CreateDirectory(Settings.Default.ApplicationPath);
                    }

                    if (!File.Exists(Settings.Default.DbPath))
                    {

                        File.Copy(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + "Terminal.s3db", Settings.Default.DbPath);
                    }

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMdiMain());
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            Log.Error("Another instance of is already running.");
                            break;
                        }
                    }
                }
            }


        }

        //private static void MakePortable(ApplicationSettingsBase settings)
        //{
        //    var portableSettingsProvider =
        //        new PortableSettingsProvider(settings.GetType().Name + ".config");
        //    settings.Providers.Add(portableSettingsProvider);
        //    foreach (SettingsProperty prop in settings.Properties)
        //        prop.Provider = portableSettingsProvider;
        //    settings.Reload();
        //}
    }
}
