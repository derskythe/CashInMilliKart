using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CashInTerminalWpf;
using CashInTerminalWpf.Enums;
using Containers;
using Containers.Enums;
using Db;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
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

        private static String sign =
            "Wk8qB1ND3U9imC1rnsJLjQA06cJAvrKYDivZmV9cYsltqA0NMy_yX-_adT-xuzOBYs8wnvRsJgIzp2XCxTlQdLDSNlTWSiMcXZ2-n6V0oLDuA2jGuOX89AlWk9cztCWPHgwrHbFk5pEaexSgJpKKiW9p9w-dAVwqI5kOvWMvZ6E.";

        private static String localKey = "-----BEGIN PUBLIC KEY-----\nMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCPSyUpRLFxicrkHhvL/fc7kBVi\nHkYLb3Q9J2H0YKdeZnZwA9om3OkuupdQ1Agb1gQTCAbVi4jJ1pHMl5WrSPX8aenC\nAJteq6HzIF6hn25E+usbsSxsLi5tDTPPDwkmmjjDkKtQ/fccxq6q3jOklYT1f0k6\nSX3oN2xGCyQdafiqLwIDAQAB\n-----END PUBLIC KEY-----";
        private static String publicKey = "-----BEGIN PUBLIC KEY-----\nMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCPSyUpRLFxicrkHhvL/fc7kBVi\nHkYLb3Q9J2H0YKdeZnZwA9om3OkuupdQ1Agb1gQTCAbVi4jJ1pHMl5WrSPX8aenC\nAJteq6HzIF6hn25E+usbsSxsLi5tDTPPDwkmmjjDkKtQ/fccxq6q3jOklYT1f0k6\nSX3oN2xGCyQdafiqLwIDAQAB\n-----END PUBLIC KEY-----";

        private static String privateKey =
            "-----BEGIN RSA PRIVATE KEY-----\nMIICXAIBAAKBgQCPSyUpRLFxicrkHhvL/fc7kBViHkYLb3Q9J2H0YKdeZnZwA9om\n3OkuupdQ1Agb1gQTCAbVi4jJ1pHMl5WrSPX8aenCAJteq6HzIF6hn25E+usbsSxs\nLi5tDTPPDwkmmjjDkKtQ/fccxq6q3jOklYT1f0k6SX3oN2xGCyQdafiqLwIDAQAB\nAoGAYtzVpsNeKZeIBBtB0lxGVzHxjuCUMw+Sgx7I1nJZByhqTp5ZxLZlq3fRLlMb\nxRjDdt3y2SKMHbWMojtzZ9nO3G7bOyRX9rtDMMviBv0B+WD1zxfd18yAD3x8sEiJ\nAKpq4vu7XbLY+bblWYH6wYnfFcUcR+GlGp/TKcza1BKCrgECQQDMhcpkUTihKUlI\nIwoHsnx21PLJArOEdgWbG1CzGI3diSL+ZQVddCI8yCkSDknorbMR9Vr0u9V72Uxx\n3Vs1Y0SJAkEAs1wevyXxB2z//59QCLxhl5FEjJixW3+4FoiRiLpfjposOD10ffXF\nuhkkodFE6HGfWJJN3AWsKOz24Yn4Hra69wJAVAG0Y0Y1U4Uo05eI3CaFFy5a1xPj\n9smffdlXaWjxhIh6tjF6Zat5EKxKql7yHr+SKRM1nAa3JprX2oFIoII4uQJAPnsC\nwvfGpR2VeEjZKpHlNVWHmaq/be5qBH+Coyy5iQWwDc9qu05YmOGVX0F1Tbv3FHWy\n5cicFo2l2x+i7aAeNQJBALvcvxhaJB5fXNBrvMXoVkn+ha4oB8p2nwf3jI2uCeIr\nft9z5ArKfUy/jQSSaAICy4GBv+8nCaSJ3IDq3Wazl+Q=\n-----END RSA PRIVATE KEY-----";
        private static String correctValue = "59206/08/2013 10:28:45";

        private static bool CheckSignature(int terminalId, DateTime terminalDate, String signature)
        {
            //return true;
            return CheckSignature(terminalId.ToString(CultureInfo.InvariantCulture),
                                  terminalDate.ToString(CultureInfo.InvariantCulture), signature);
        }

        private static bool CheckSignature(String terminalId, String terminalDate, String signature)
        {
            var correctString = terminalId + terminalDate;
            //var keyPair = new AsymmetricCipherKeyPair(GetKey(publicCert), GetPrivateKey());

            var raw = Wrapper.Decrypt(UrlBase64.Decode(signature), GetPrivateKey());

            if (raw == null || raw.Length == 0)
            {
                return false;
            }

            Log.Debug(correctString);
            Log.Debug(Encoding.UTF8.GetString(raw));
            if (correctString == Encoding.ASCII.GetString(raw))
            {
                return true;
            }

            return false;
        }

        private static string DoSign(int terminalId, DateTime serverTime, byte[] publicCert)
        {
            //return String.Empty;
            return DoSign(terminalId.ToString(CultureInfo.InvariantCulture), serverTime, publicCert);
        }

        private static string DoSign(String terminalId, DateTime serverTime, byte[] publicCert)
        {
            var correctString = terminalId + serverTime.ToString(CultureInfo.InvariantCulture);
            var raw = Wrapper.Encrypt(Encoding.UTF8.GetBytes(correctString), GetKey(publicCert));

            return Encoding.ASCII.GetString(UrlBase64.Encode(raw));
        }

        private static RsaKeyParameters GetKey(byte[] key)
        {
            var stream = new MemoryStream(key);
            var keyStream = new StreamReader(stream);
            var reader = new PemReader(keyStream);

            return (RsaKeyParameters)reader.ReadObject();
        }

        private static AsymmetricKeyParameter GetPrivateKey()
        {
            AsymmetricKeyParameter result = null;
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(privateKey)))
            {
                var privateKeyStream = new StreamReader(stream);
                var privateKeyReader = new PemReader(privateKeyStream);

                result = ((AsymmetricCipherKeyPair)privateKeyReader.ReadObject()).Private;
            }
            return result;
        }

        static void Main(string[] args)
        {
            var billmask = new BitArray(48);

            var tz = TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time");
            var coll = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo info in coll)
            {
                Console.WriteLine(String.Format("info: {0}, id: {1}", info, info.Id));
            }
            //var tz = TimeZoneInfo.FindSystemTimeZoneById("Caucasus Standard Time");
                
            var requestTime = new DateTimeWithZone(new DateTime(2013, 06, 08, 10, 28, 45), tz);
            Console.WriteLine(CheckSignature(592, requestTime.LocalTime, sign));

            Console.ReadKey();
            return;

            //Regex pRegex = new Regex("[^0-9a-z]", RegexOptions.IgnoreCase);
            //String val = "Пе56656565кк$$#$#&@$@!*#/!$!)%@*($!(~~~)@~!ееTEST";

            //var t =
            //    string.Concat(val.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries)).Trim();

            //val = pRegex.Replace(val, "");

            //TimeSpan _MaxTime = new TimeSpan(700 * 10000);
            //TimeSpan _MaxTime2 = new TimeSpan(0, 0, 0, 1);

            //Console.WriteLine(_MaxTime.TotalMilliseconds);

            //OracleDb.Init(Settings.Default.OracleUser, Settings.Default.OraclePassword, Settings.Default.OracleDb);
            //OracleDb.Instance.CheckConnection();
            //int count = 0;
            //OracleDb.Instance.ListTerminals(TerminalColumns.Id, SortType.Asc, 0, 1000, out count);

            //var currencies = new List<int>();
            //for (int i = 0; i < 30; i++)
            //{
            //    currencies.Add(i);
            //}

            //for (int i = (billmask.Count - 1); i >= 0; i++)
            //{
                
            //}

            //var billmask = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //var currencies = new List<int>();

            //for (int i = 0; i < 30; i++)
            //{
            //    currencies.Add(i);
            //}

            //int j = 0;
            //byte tmp = 0x0;
            //byte bit = 0x0;
            //for (int i = currencies.Count; i >= 0 && j < billmask.Length; i--)
            //{
            //    if (bit >= 8)
            //    {
            //        bit = 0x0;
            //        billmask[j] = tmp;
            //        j++;
            //    }
            //    else
            //    {
            //        tmp |= (byte)(0x01 << bit);
            //        bit++;
            //        billmask[j] = tmp;
            //    }
            //}
            ////

            //int val = 0x7D | 0x02;
            //val = 0x7D << 0x01;
            //val = 0x7D >> 0x01;

            var device = new CCNETDevice();
            device.BillStacked += DeviceOnBillStacked;
            device.Open(3, CCNETPortSpeed.S9600);

            device.Init();

            
            

            //Console.WriteLine(DateTime.Parse("2013-05-06 19:54:30"));
            //float val = 124354.0f;
            //Console.WriteLine(val.ToString("0.00"));
            //var sample = new[] {"+994503312380", "0503312380"};

            //foreach (var s in sample)
            //{
            //    var m = _Regex.Replace(s, _Pattern);
            //    Console.WriteLine(m);
            //}

            //var reg = new Regex("^[a-z0-9]+$", RegexOptions.IgnoreCase);
            //var m2 = reg.Match("SKIF1CH");

            //if (m2.Success)
            //{

            //}


            //Console.WriteLine(FirstUpper("HƏSƏNOVA ZüLFIYYƏ XUDU QIZI"));
            ////Console.WriteLine(MediaTypeNames.Application.ExecutablePath);



            //OracleDb.Init(Settings.Default.OracleUser, Settings.Default.OraclePassword, Settings.Default.OracleDb);
            //OracleDb.Instance.CheckConnection();
            //int billGroup = 36471;

            var list = new List<TerminalPaymentInfo>();
            //{
            //    {
            //        var info = new TerminalPaymentInfo();
            //        info.TerminalId = 512;
            //        info.TransactionId = "512121";
            //        info.ProductId = 1;
            //        info.Currency = "AZN";
            //        info.CurrencyRate = 1;
            //        info.Amount = 200;
            //        info.OperationType = 11;
            //        info.Values = new[] {"3801000CC0018221U", "AZE01743332"};
            //        info.CreditNumber = "CC0018221U/11A";
            //        info.PaymentServiceId = 0;
            //        info.Banknotes = new[] {50, 50, 50, 50};
            //        info.TerminalDate = DateTime.Parse("11-05-2013 10:15:29");
            //        info.SystemTime = DateTime.Parse("11-05-2013 10:15:29");

            //        list.Add(info);
            //    }

            //    {
            //        var info = new TerminalPaymentInfo();
            //        info.TerminalId = 512;
            //        info.TransactionId = "512122";
            //        info.ProductId = 1;
            //        info.Currency = "AZN";
            //        info.CurrencyRate = 1;
            //        info.Amount = 120;
            //        info.OperationType = 21;
            //        info.Values = new[] { "3811001200024821", "AZE05501126" };
            //        info.CreditNumber = "3811001200024821";
            //        info.PaymentServiceId = 0;
            //        info.Banknotes = new[] { 100, 20 };
            //        info.TerminalDate = DateTime.Parse("11-05-2013 10:16:42");
            //        info.SystemTime = DateTime.Parse("11-05-2013 10:16:42");

            //        list.Add(info);
            //    }

            //    {
            //        var info = new TerminalPaymentInfo();
            //        info.TerminalId = 512;
            //        info.TransactionId = "512123";
            //        info.ProductId = 1;
            //        info.Currency = "AZN";
            //        info.CurrencyRate = 1;
            //        info.Amount = 16;
            //        info.OperationType = 11;
            //        info.Values = new[] { "3801000BL0395415", "AZE02805150" };
            //        info.CreditNumber = "BAL0395415/12";
            //        info.PaymentServiceId = 0;
            //        info.Banknotes = new[] { 10, 5, 1 };
            //        info.TerminalDate = DateTime.Parse("11-05-2013 10:17:57");
            //        info.SystemTime = DateTime.Parse("11-05-2013 10:17:57");

            //        list.Add(info);
            //    }

            //    {
            //        var info = new TerminalPaymentInfo();
            //        info.TerminalId = 512;
            //        info.TransactionId = "512124";
            //        info.ProductId = 1;
            //        info.Currency = "AZN";
            //        info.CurrencyRate = 1;
            //        info.Amount = 21;
            //        info.OperationType = 11;
            //        info.Values = new[] { "3801000BL058514", "AZE09980317" };
            //        info.CreditNumber = "BAL058514/12";
            //        info.PaymentServiceId = 0;
            //        info.Banknotes = new[] { 10, 10, 1 };
            //        info.TerminalDate = DateTime.Parse("11-05-2013 10:17:57");
            //        info.SystemTime = DateTime.Parse("11-05-2013 10:17:57");

            //        list.Add(info);
            //    }
            //}


            //foreach (var request in list)
            //{
            //    OracleDb.Instance.SavePayment(request);
            //    string bills = String.Join(";", request.Banknotes);
            //    OracleDb.Instance.CommitPaymentManual(request.CreditNumber, request.Amount, bills, request.TerminalId,
            //                                    request.OperationType, request.TerminalDate, request.Currency, billGroup);
            //}

            //OracleDb.Instance.GetBonusAmount("LS000293/13", 50, "AZN");

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

            device.DeviceState.Currency = "AZN";
            device.CurrentCurrency = "AZN";
            device.Poll();
            device.EnableAll();
            device.StartPool = true;

            Console.ReadLine();

            device.Poll();
            device.Disable();
            device.StartPool = false;

            Console.ReadLine();

            device.Close();
            device.Dispose();
        }

        private static void DeviceOnBillStacked(CCNETDeviceState ccnetDeviceState)
        {
            Log.Debug(ccnetDeviceState.ToString());
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
