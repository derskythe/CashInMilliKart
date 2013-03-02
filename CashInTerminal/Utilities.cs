using System;
using System.Globalization;
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
       
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetSystemTime")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        private static extern bool SetSystemTime([InAttribute] ref SYSTEMTIME lpSystemTime);

        public static string Sign(String terminalId, DateTime now, AsymmetricKeyParameter keys)
        {
            try
            {
                var correctString = terminalId + now.ToString(CultureInfo.InvariantCulture);

                var encrypted = Wrapper.Encrypt(Encoding.ASCII.GetBytes(correctString), keys);
                return Encoding.ASCII.GetString(UrlBase64.Encode(encrypted));
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return String.Empty;
        }

        public static bool CheckSignature(String terminalId, DateTime terminalDate, String signature, AsymmetricCipherKeyPair keys)
        {
            var correctString = terminalId + terminalDate.ToString(CultureInfo.InvariantCulture);
            var raw = Wrapper.Decrypt(UrlBase64.Decode(signature), keys.Private);

            if (raw == null || raw.Length == 0)
            {
                return false;
            }

            if (correctString == Encoding.UTF8.GetString(raw))
            {
                return true;
            }

            return false;
        }

        public static bool CheckServerSign(String signature, String terminalId, DateTime terminalDate, AsymmetricCipherKeyPair keys)
        {
            try
            {
                var correctString = terminalId + terminalDate.ToString(CultureInfo.InvariantCulture);
                var raw = Wrapper.Decrypt(UrlBase64.Decode(signature), keys.Private);

                if (raw == null || raw.Length == 0)
                {
                    return false;
                }

                if (correctString == Encoding.UTF8.GetString(raw))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return false;
        }

        public static uint GetLastInputTime()
        {
            uint idleTime = 0;
            var lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            var envTicks = (uint)Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;
                idleTime = envTicks - lastInputTick;
            }

            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }

        public static void UpdateSystemTime(DateTime newDate)
        {
            var systime = new SYSTEMTIME
                {
                    wYear = (ushort) newDate.Year,
                    wMonth = (ushort) newDate.Month,
                    wDay = (ushort) newDate.Day,
                    wDayOfWeek = (ushort) newDate.DayOfWeek,
                    wHour = (ushort) newDate.Hour,
                    wMinute = (ushort) newDate.Minute,
                    wSecond = (ushort) newDate.Second,
                    wMilliseconds = (ushort) newDate.Millisecond
                };

            SetSystemTime(ref systime);
        }
    }

    // ReSharper disable FieldCanBeMadeReadOnly.Local
    // ReSharper disable MemberCanBePrivate.Local
    // ReSharper disable InconsistentNaming
    [StructLayout(LayoutKind.Sequential)]
    struct LASTINPUTINFO
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dwTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }
    // ReSharper restore MemberCanBePrivate.Local
    // ReSharper restore FieldCanBeMadeReadOnly.Local
    // ReSharper restore InconsistentNaming
}
