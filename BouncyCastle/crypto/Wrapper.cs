using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
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
