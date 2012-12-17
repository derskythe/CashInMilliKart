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
    public enum ResultCodes
    {
        [Description("OK")]
        Ok = 0,

        [Description("Invalid number")]
        InvalidNumber = 1,

        [Description("Invalid parameters")]
        InvalidParameters = 2,

        [Description("Invalid terminal")]
        InvalidTerminal = 3,

        [Description("Invalid terminal key")]
        InvalidKey = 4,

        [Description("Unknown Error")]
        UnknownError = 128,

        [Description("System Error")]
        SystemError = 256
    }
}
