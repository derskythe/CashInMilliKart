using System;
using System.Globalization;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using CashInTerminal.Enums;
using Containers.Enums;
using Db;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using TestConsole.Properties;
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
        static Regex _Regex = new Regex(@"^(0|\+994)(.+?)$");
        private static String _Pattern = "$2";

        static void Main(string[] args)
        {
            float val = 124354.0f;
            Console.WriteLine(val.ToString("0.00"));
            var sample = new[] {"+994503312380", "0503312380"};

            foreach (var s in sample)
            {
                var m = _Regex.Replace(s, _Pattern);
                Console.WriteLine(m);
            }

            var reg = new Regex("^[a-z0-9]+$", RegexOptions.IgnoreCase);
            var m2 = reg.Match("SKIF1CH");

            if (m2.Success)
            {
                
            }
            

            Console.WriteLine(FirstUpper("HƏSƏNOVA ZüLFIYYƏ XUDU QIZI"));
            //Console.WriteLine(MediaTypeNames.Application.ExecutablePath);

            OracleDb.Init(Settings.Default.OracleUser, Settings.Default.OraclePassword, Settings.Default.OracleDb);
            OracleDb.Instance.CheckConnection();

            OracleDb.Instance.GetBonusAmount("LS000293/13", 50, "AZN");

            //var terminal = OracleDb.Instance.GetTerminal(3);

            //var list1 = OracleDb.Instance.ListUsers(UsersColumns.Username, SortType.Asc);
            //var list2 = OracleDb.Instance.ListUsers(UsersColumns.Username, SortType.Asc);

            //GetValueFromDescription<ExtendedPrinterStatus>(String.Empty);

            //var salt = Wrapper.GenerateSalt();
            //var encPassword = Wrapper.ComputeHash("this is realy, realy, realy, realy, realy big password!", salt);


            //var keys = Wrapper.SaveToString(Wrapper.GenerateKeys(1024));
            //var PrivateKey1 = keys[0];
            //var PublicKey1 = keys[1];

            //var KeyPair1 = Wrapper.GetKeys(PrivateKey1, PublicKey1);

            //keys = Wrapper.SaveToString(Wrapper.GenerateKeys(1024));
            //var PrivateKey2 = keys[0];
            //var PublicKey2 = keys[1];

            //var KeyPair2 = Wrapper.GetKeys(PrivateKey2, PublicKey2);


            //DateTime now = DateTime.Now;

            //var encrypted = Sign("1", now, KeyPair2);

            //CheckSignature("1", now, encrypted, KeyPair2);

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        public static void GetValueFromDescription<T>(string description)
        {
            var str = new StringBuilder();
            var str2 = new StringBuilder();
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                try
                {                    
                    //str.Append(field.Name + "=" + field.GetRawConstantValue()).Append("\r\n");
                    Console.WriteLine(field.Name + "=" + field.GetRawConstantValue());

                    str.Append("insert into PRINTER_STATUS (id, name, desc_id) values (");
                    str.Append(field.GetRawConstantValue())
                       .Append(", '")
                       .Append(field.Name)
                       .Append("', 'print.status.code.")
                       .Append(field.Name.ToLower())
                       .Append("');\r\n");

                    str2.Append("insert into descriptions (id, lang, value) values (")
                        .Append("'print.status.code.")
                        .Append(field.Name.ToLower())
                        .Append("', 'en', '").Append(field.Name)
                        .Append("');\r\n");
                }
                catch
                {
                }                
            }

            Console.WriteLine(str.ToString());
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

        private static String FirstUpper(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            var values = value.Split(' ');
            var buffer = new StringBuilder();
            foreach (var s in values)
            {
                buffer.Append(s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant()).Append(" ");
            }

            return buffer.ToString().Trim();
        }
    }
}
