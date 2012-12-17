using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Containers
{
    [SerializableAttribute]
    [ComVisible(true)]
    [Flags]
    public enum TerminalCodes
    {
        [Description("OK")]
        Ok = 0,

        [Description("Authorizated")]
        Auth = 1,

        [Description("Unknown Error")]
        UnknownError = 128,

        [Description("System Error")]
        SystemError = 256
    }
}
