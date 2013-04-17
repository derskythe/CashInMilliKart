using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using crypto;

namespace CashInTerminalWpf
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

        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        public extern static void Win32GetSystemTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool Win32SetSystemTime(ref SystemTime sysTime);

        private static readonly CultureInfo _Info = new CultureInfo(0x042C);

        public static String FormatDate(DateTime value)
        {
            return value.ToString("HH:mm:ss dd.MM.yyyy");
        }

        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var percent = (newWidth / 100) * 5;
            newWidth -= percent;
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

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
            newDate = newDate.ToUniversalTime();
            var updatedTime = new SystemTime
                {
                    Year = (ushort) newDate.Year,
                    Month = (ushort) newDate.Month,
                    Day = (ushort) newDate.Day,
                    Hour = (ushort) newDate.Hour,
                    Minute = (ushort) newDate.Minute,
                    Second = (ushort) newDate.Second
                };

            //Log.Info(newDate.ToLongTimeString());

            if (!Win32SetSystemTime(ref updatedTime))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static String FirstUpper(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }            

            var values = value.Split(' ');
            var buffer = new StringBuilder();
            foreach (var s in values)
            {
                buffer.Append(s.Substring(0, 1).ToUpper(_Info) + s.Substring(1).ToLower(_Info)).Append(" ");
            }

            return buffer.ToString().Trim();
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

    public struct SystemTime
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Millisecond;
    };
    // ReSharper restore MemberCanBePrivate.Local
    // ReSharper restore FieldCanBeMadeReadOnly.Local
    // ReSharper restore InconsistentNaming
}
