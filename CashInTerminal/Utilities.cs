using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using crypto;

namespace CashInTerminal
{
    public class Utilities
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static string Sign(String terminalId, DateTime now, ref AsymmetricCipherKeyPair keys)
        {
            try
            {
                var correctString = terminalId + now.ToString(CultureInfo.InvariantCulture);

                var encrypted = Wrapper.Encrypt(Encoding.ASCII.GetBytes(correctString), keys.Private);
                return Encoding.ASCII.GetString(UrlBase64.Encode(encrypted));
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return String.Empty;
        }

        public static uint GetLastInputTime()
        {
            uint idleTime = 0;
            var lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;
                idleTime = envTicks - lastInputTick;
            }

            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct LASTINPUTINFO
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dwTime;
    }
}
