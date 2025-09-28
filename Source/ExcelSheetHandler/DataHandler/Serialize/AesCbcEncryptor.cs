using System;
using System.IO;
using System.Security.Cryptography;

namespace DataHandler.Serialize
{
    public class AesCbcEncryptor
    {
        public byte[] Encrypt(byte[] plain, byte[] key)
        {
            byte[] encryptedBytes;
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plain, 0, plain.Length);
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        public byte[] Decrypt(byte[] encryptedWithIv, byte[] key)
        {
            const int ivLength = 16;
            if (encryptedWithIv == null || encryptedWithIv.Length <= ivLength)
                throw new CryptographicException("Encrypted payload too short.");

            var iv = new byte[ivLength];
            Buffer.BlockCopy(encryptedWithIv, 0, iv, 0, ivLength);
            var cipherText = new byte[encryptedWithIv.Length - ivLength];
            Buffer.BlockCopy(encryptedWithIv, ivLength, cipherText, 0, cipherText.Length);

            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(cipherText, 0, cipherText.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }
}


