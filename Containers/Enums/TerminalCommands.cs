using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Containers.Enums
{
    [Serializable]
    [ComVisible(true)]
    [Flags]
    public enum TerminalCommands
    {
        [Description("Do nothing")]
        Idle = 0,
        [Description("Encash command to terminal")]
        Encash = 1,
        [Description("Start test mode for testing printer and cashcode")]
        TestMode = 2,
        [Description("Network error")]
        NetworkError = 3,
        [Description("Start normal mode")]
        NormalMode = 32,
        [Description("Stop working")]
        Stop = 128,
        [Description("Show out of service screen")]
        OutOfService = 256
    }
}
