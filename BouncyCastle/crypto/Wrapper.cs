using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace crypto
{
    public class Wrapper
    {
        public static AsymmetricCipherKeyPair GenerateKeys(int keySizeInBits)
        {
            var r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), keySizeInBits));
            AsymmetricCipherKeyPair keys = r.GenerateKeyPair();

            return keys;
        }

        public static string[] SaveToString(AsymmetricCipherKeyPair keys)
        {
            TextWriter textWriter = new StringWriter();
            var pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(keys.Private);
            pemWriter.Writer.Flush();

            var result = new string[2];
            result[0] = textWriter.ToString();

            textWriter = new StringWriter();
            pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(keys.Public);
            pemWriter.Writer.Flush();

            result[1] = textWriter.ToString();

            return result;
        }

        public static AsymmetricCipherKeyPair GetKeys(string privateKeyString, string publicKeyString)
        {
            var publicStream = new MemoryStream(Encoding.ASCII.GetBytes(publicKeyString));
            var publicKeyStreamReader = new StreamReader(publicStream);
            var pr = new PemReader(publicKeyStreamReader);

            var privateKeyStream = new MemoryStream(Encoding.ASCII.GetBytes(privateKeyString));
            var privateKeyStreamReader = new StreamReader(privateKeyStream);
            var privateKeyReader = new PemReader(privateKeyStreamReader);

            var publicKey = (RsaKeyParameters)pr.ReadObject();
            var privateKey = ((AsymmetricCipherKeyPair)privateKeyReader.ReadObject()).Private;

            var keyPair = new AsymmetricCipherKeyPair(publicKey, privateKey);

            publicStream.Close();
            publicKeyStreamReader.Close();

            privateKeyStream.Close();
            privateKeyStreamReader.Close();

            return keyPair;
        }

        public static AsymmetricKeyParameter GetKey(string keyString)
        {
            var privateKeyStream = new MemoryStream(Encoding.ASCII.GetBytes(keyString));
            var privateKeyStreamReader = new StreamReader(privateKeyStream);
            var privateKeyReader = new PemReader(privateKeyStreamReader);

            var privateKey = ((AsymmetricCipherKeyPair)privateKeyReader.ReadObject()).Private;

            privateKeyStream.Close();
            privateKeyStreamReader.Close();

            return privateKey;
        }

        public static byte[] Encrypt(byte[] data, AsymmetricKeyParameter key)
        {
            var e = new RsaEngine();
            e.Init(true, key);
            int blockSize = e.GetInputBlockSize();

            var output = new List<byte>();

            for (int chunkPosition = 0; chunkPosition < data.Length; chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, data.Length - (chunkPosition * blockSize));
                output.AddRange(e.ProcessBlock(data, chunkPosition, chunkSize));
            }

            return output.ToArray();
        }

        public static byte[] Decrypt(byte[] data, AsymmetricKeyParameter key)
        {
            var e = new RsaEngine();
            e.Init(false, key);

            int blockSize = e.GetInputBlockSize();

            var output = new List<byte>();

            for (int chunkPosition = 0; chunkPosition < data.Length;
                chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, data.Length -
                  (chunkPosition * blockSize));
                output.AddRange(e.ProcessBlock(data, chunkPosition,
                  chunkSize));
            }
            return output.ToArray();
        }
    }
}
