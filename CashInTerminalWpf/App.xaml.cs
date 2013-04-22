using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using CashInTerminalWpf.Properties;
using NLog;

namespace CashInTerminalWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        // ReSharper disable ConvertToConstant.Local
        private static readonly bool _TestVersion = true;
        // ReSharper restore ConvertToConstant.Local

        public static bool TestVersion
        {
            get { return _TestVersion; }
        }

        private void ApplicationLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            Process current;
            try
            {
                current = Process.GetCurrentProcess();
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

            current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id != current.Id)
                {
                    Log.Error("Another instance of is already running.");
                    Current.Shutdown();
                    break;
                }
            }
            //MakePortable(Properties.Settings.Default);

            if (!Directory.Exists(Settings.Default.ApplicationPath))
            {
                Directory.CreateDirectory(Settings.Default.ApplicationPath);
            }

            if (!File.Exists(Settings.Default.DbPath))
            {
                File.Copy(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + Path.DirectorySeparatorChar +
                    "Terminal.s3db", Settings.Default.DbPath);
            }

            ChangeCulture("az-Latn-AZ");
        }

        private static void ChangeCulture(String name)
        {
            try
            {
                var culture = new CultureInfo(name);

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }
        }
    }
}
