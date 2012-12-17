using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Utilities.Encoders;
using crypto;

namespace CashInCore
{
    public partial class CashInServer
    {
        private AsymmetricKeyParameter _PrivateKey;

        private bool CheckSignature(String terminalId, DateTime terminalDate, String signature, byte[] publicCert)
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

            return false;
        }

        private string DoSign(String terminalId, DateTime serverTime, byte[] publicCert)
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

        private AsymmetricKeyParameter GetPrivateKey()
        {
            if (_PrivateKey != null)
            {
                return _PrivateKey;
            }

            var privateKeyStream = new StreamReader(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "cert_private.pem");
            var privateKeyReader = new PemReader(privateKeyStream);

            return ((AsymmetricCipherKeyPair)privateKeyReader.ReadObject()).Private;
        }
    }
}
