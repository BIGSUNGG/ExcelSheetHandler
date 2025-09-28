using System;
using System.Security.Cryptography;

namespace DataHandler.Serialize
{
    public class HmacIntegrityValidator
    {
        public const int HmacLength = 32; // SHA-256

        public byte[] AppendHmac(byte[] data, byte[] key)
        {
            byte[] hmac;
            using (var hmacsha256 = new HMACSHA256(key))
            {
                hmac = hmacsha256.ComputeHash(data);
            }

            var finalBytes = new byte[data.Length + hmac.Length];
            Buffer.BlockCopy(data, 0, finalBytes, 0, data.Length);
            Buffer.BlockCopy(hmac, 0, finalBytes, data.Length, hmac.Length);
            return finalBytes;
        }

        public byte[] VerifyAndStripHmac(byte[] dataWithHmac, byte[] key)
        {
            if (dataWithHmac == null || dataWithHmac.Length <= HmacLength)
                throw new CryptographicException("Invalid protected data length.");

            int dataLength = dataWithHmac.Length - HmacLength;
            var data = new byte[dataLength];
            var receivedHmac = new byte[HmacLength];
            Buffer.BlockCopy(dataWithHmac, 0, data, 0, dataLength);
            Buffer.BlockCopy(dataWithHmac, dataLength, receivedHmac, 0, HmacLength);

            byte[] computedHmac;
            using (var hmacsha256 = new HMACSHA256(key))
            {
                computedHmac = hmacsha256.ComputeHash(data);
            }
            if (!ConstantTimeEquals(computedHmac, receivedHmac))
                throw new CryptographicException("HMAC validation failed.");

            return data;
        }

        private static bool ConstantTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}


