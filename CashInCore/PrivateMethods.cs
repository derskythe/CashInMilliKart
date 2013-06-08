using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CashInCore.Properties;
using Containers;
using Containers.Enums;
using Db;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Utilities.Encoders;
using crypto;

namespace CashInCore
{
    public partial class CashInServer
    {
        private static AsymmetricKeyParameter _PrivateKey;

        private bool CheckSignature(int terminalId, long terminalDate, String signature)
        {
            //return true;
            return CheckSignature(terminalId.ToString(CultureInfo.InvariantCulture), terminalDate, signature);
        }

        private String NormalizeString(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }
            var pRegex = new Regex(@"[^0-9a-z\;\,\.\-\+\s]", RegexOptions.IgnoreCase);
            return pRegex.Replace(value, String.Empty).Trim();
        }

        private bool CheckSignature(String terminalId, long terminalDate, String signature)
        {
            var correctString = terminalId + terminalDate.ToString(CultureInfo.InvariantCulture);
            //var keyPair = new AsymmetricCipherKeyPair(GetKey(publicCert), GetPrivateKey());

            var raw = Wrapper.Decrypt(UrlBase64.Decode(signature), GetPrivateKey());

            if (raw == null || raw.Length == 0)
            {
                return false;
            }

            if (correctString == Encoding.UTF8.GetString(raw))
            {
                return true;
            }

            Log.Debug(String.Format("Correct string: {0}, sent string: {1}", correctString, Encoding.UTF8.GetString(raw)));

            return false;
        }

        private string DoSign(int terminalId, long serverTime, byte[] publicCert)
        {
            //return String.Empty;
            return DoSign(terminalId.ToString(CultureInfo.InvariantCulture), serverTime, publicCert);
        }

        private string DoSign(String terminalId, long serverTime, byte[] publicCert)
        {
            var correctString = terminalId + serverTime.ToString(CultureInfo.InvariantCulture);
            var raw = Wrapper.Encrypt(Encoding.UTF8.GetBytes(correctString), GetKey(publicCert));

            return Encoding.ASCII.GetString(UrlBase64.Encode(raw));
        }

        private RsaKeyParameters GetKey(byte[] key)
        {
            var stream = new MemoryStream(key);
            var keyStream = new StreamReader(stream);
            var reader = new PemReader(keyStream);

            return (RsaKeyParameters)reader.ReadObject();
        }

        private static AsymmetricKeyParameter GetPrivateKey()
        {
            if (_PrivateKey != null)
            {
                return _PrivateKey;
            }

            Log.Debug(Settings.Default.PrivateKey);
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(Settings.Default.PrivateKey)))
            {
                //var privateKeyStream = new StreamReader(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "cert_private.pem");
                var privateKeyStream = new StreamReader(stream);
                var privateKeyReader = new PemReader(privateKeyStream);

                _PrivateKey = ((AsymmetricCipherKeyPair)privateKeyReader.ReadObject()).Private;
            }
            return _PrivateKey;
        }

        private StandardResult AuthTerminal(StandardResult response, StandardRequest request, out Terminal terminalInfo)
        {
            if (request == null || String.IsNullOrEmpty(request.Sign) || request.TerminalId <= 0)
            {
                response.Code = ResultCodes.InvalidParameters;
                throw new ArgumentNullException("No data");
            }

            if (!CheckSignature(request.TerminalId, request.Ticks, request.Sign))
            {
                response.Code = ResultCodes.InvalidSignature;
                throw new Exception("Invalid signature. " + request);
            }

            terminalInfo = OracleDb.Instance.GetTerminal(request.TerminalId);

            if (terminalInfo == null)
            {
                throw new Exception("Not found terminal in DB. " + request);
            }

            return response;
        }
    }
}
