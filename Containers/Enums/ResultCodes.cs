using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Containers.Enums
{
    [Serializable]
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

        [Description("Invalid signature of message")]
        InvalidSignature = 5,

        [Description("Unknown Error")]
        UnknownError = 128,

        [Description("System Error")]
        SystemError = 256
    }
}
