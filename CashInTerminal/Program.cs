using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace CashInTerminal
{
    static class Program
    {
        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool mutexCreated = true;
            using (var mutex = new Mutex(true, "CashInTerminal", out mutexCreated))
            {
                if (mutexCreated)
                {
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
                            //MessageBox.Show("Another instance of eCS is already running.", "eCS already running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }

            
        }
    }
}
