﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Containers.Enums
{
    [Serializable]
    [ComVisible(true)]
    [Flags]
    public enum TerminalCodes
    {
        [Description("OK")]
        Ok = 0,

        [Description("Authorizated")]
        Auth = 1,

        [Description("Encashment")]
        Encashment = 2,

        [Description("Out of order")]
        OutOfOrder = 3,

        [Description("Not initiated")]
        NotInitiated = 4,

        [Description("Test mode")]
        TestMode = 32,

        [Description("Unknown Error")]
        UnknownError = 128,

        [Description("System Error")]
        SystemError = 256
    }
}
