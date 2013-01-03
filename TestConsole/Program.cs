using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using crypto;

namespace TestConsole
{
    class Program
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        static void Main(string[] args)
        {
            var keys = Wrapper.SaveToString(Wrapper.GenerateKeys(1024));
            var PrivateKey1 = keys[0];
            var PublicKey1 = keys[1];

            var KeyPair1 = Wrapper.GetKeys(PrivateKey1, PublicKey1);

            keys = Wrapper.SaveToString(Wrapper.GenerateKeys(1024));
            var PrivateKey2 = keys[0];
            var PublicKey2 = keys[1];

            var KeyPair2 = Wrapper.GetKeys(PrivateKey2, PublicKey2);


            DateTime now = DateTime.Now;

            var encrypted = Sign("1", now, KeyPair2);

            CheckSignature("1", now, encrypted, KeyPair2);

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        public static string Sign(String terminalId, DateTime now, AsymmetricCipherKeyPair keys)
        {
            try
            {
                var correctString = terminalId + now.ToString(CultureInfo.InvariantCulture);

                var encrypted = Wrapper.Encrypt(Encoding.ASCII.GetBytes(correctString), keys.Public);
                return Encoding.ASCII.GetString(UrlBase64.Encode(encrypted));
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return String.Empty;
        }

        private static bool CheckSignature(String terminalId, DateTime terminalDate, String signature, AsymmetricCipherKeyPair keys)
        {
            var correctString = terminalId + terminalDate.ToString(CultureInfo.InvariantCulture);
            //var keyPair = new AsymmetricCipherKeyPair(GetKey(publicCert), GetPrivateKey());

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
    }
}
