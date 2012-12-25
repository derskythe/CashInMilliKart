using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CashInTerminal
{
    public partial class FormMain
    {
        private Thread _PingThread;
        private Thread _SendPaymentThread;
        private const int PING_TIMEOUT = 60*1000;
        private const int SEND_PAYMENT_TIMEOUT = 1000;

        private void PingThread()
        {
            while (_Running && _Init && _AuthTerminal)
            {
                Thread.Sleep(PING_TIMEOUT);
            }
        }

        private void SendPaymentThread()
        {
            while (_Running && _Init && _AuthTerminal)
            {
                Thread.Sleep(SEND_PAYMENT_TIMEOUT);
            }
        }
    }
}
