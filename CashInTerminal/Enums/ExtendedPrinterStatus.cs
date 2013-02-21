using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashInTerminal.Enums
{
    public enum ExtendedPrinterStatus
    {
        Other = 0x1,
        Unknown = 0x2,
        Idle = 0x3,
        Printing = 0x4,
        WarmingUp = 0x5,
        StoppedPrinting = 0x6,
        Offline = 0x7,
        Paused = 0x8,
        Error = 0x9,
        Busy = 0xA,
        NotAvailable = 0xB,
        Waiting = 0xC,
        Processing = 0xD,
        Initialization = 0xE,
        PowerSave = 0xF,
        PendingDeletion = 0x10,
        IOActive = 0x11,
        ManualFeed = 0x12
    }
}
