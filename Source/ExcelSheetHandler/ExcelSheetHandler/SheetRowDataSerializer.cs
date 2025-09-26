using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroFormatter;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ExcelSheetHandler
{
    public class SheetRowDataSerializer
    {
        public static SheetRowDataSerializer Instance { get; private set; } = new SheetRowDataSerializer();

        public byte[] Serialize(List<SheetRowData> rowDatas, byte[] secretKey)
        {
            // Step1 : Byte Serialize Encoding
            byte[] encodedBytes = ZeroFormatterSerializer.Serialize(rowDatas);

            // Step2 : Compress Bytes
            byte[] compressedBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal, leaveOpen: true))
                {
                    gzipStream.Write(encodedBytes, 0, encodedBytes.Length);
                }
                compressedBytes = memoryStream.ToArray();
            }

            // Step3 : Encrypt Bytes
            byte[] encryptedBytes;
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = secretKey;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(compressedBytes, 0, compressedBytes.Length);
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            // Step4 : Anti Tampering Bytes
            byte[] hmac;
            using (var hmacsha256 = new HMACSHA256(secretKey))
            {
                hmac = hmacsha256.ComputeHash(encryptedBytes);
            }

            var finalBytes = new byte[encryptedBytes.Length + hmac.Length];
            Buffer.BlockCopy(encryptedBytes, 0, finalBytes, 0, encryptedBytes.Length);
            Buffer.BlockCopy(hmac, 0, finalBytes, encryptedBytes.Length, hmac.Length);

            return finalBytes;
        }

        public List<SheetRowData> Deserialize(byte[] protectedBytes, byte[] secretKey)
        {
            if (protectedBytes == null) throw new ArgumentNullException(nameof(protectedBytes));
            if (secretKey == null) throw new ArgumentNullException(nameof(secretKey));
            if (protectedBytes.Length < 16 + 1 + 32) // IV(16) + at least 1 byte ciphertext + HMAC(32)
                throw new ArgumentException("Invalid protected data length.", nameof(protectedBytes));

            // Step1 : Split Bytes
            const int hmacLength = 32; // HMAC-SHA256
            int dataLength = protectedBytes.Length - hmacLength;
            var encryptedBytes = new byte[dataLength];
            var receivedHmac = new byte[hmacLength];
            Buffer.BlockCopy(protectedBytes, 0, encryptedBytes, 0, dataLength);
            Buffer.BlockCopy(protectedBytes, dataLength, receivedHmac, 0, hmacLength);

            // Step2 : Verify Anti Tampering Bytes
            byte[] computedHmac;
            using (var hmacsha256 = new HMACSHA256(secretKey))
            {
                computedHmac = hmacsha256.ComputeHash(encryptedBytes);
            }
            if (!ConstantTimeEquals(computedHmac, receivedHmac))
                throw new CryptographicException("HMAC validation failed.");

            // Step3 : Decrypt Bytes
            const int ivLength = 16; // AES block size (128-bit)
            if (encryptedBytes.Length <= ivLength)
                throw new CryptographicException("Encrypted payload too short.");

            var iv = new byte[ivLength];
            Buffer.BlockCopy(encryptedBytes, 0, iv, 0, ivLength);
            var cipherText = new byte[encryptedBytes.Length - ivLength];
            Buffer.BlockCopy(encryptedBytes, ivLength, cipherText, 0, cipherText.Length);

            byte[] compressedBytes;
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = secretKey;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(cipherText, 0, cipherText.Length);
                    cs.FlushFinalBlock();
                    compressedBytes = ms.ToArray();
                }
            }

            // Step4 : Decompress Bytes
            byte[] encodedBytes;
            using (var inputMs = new MemoryStream(compressedBytes))
            using (var gzip = new GZipStream(inputMs, CompressionMode.Decompress))
            using (var outputMs = new MemoryStream())
            {
                gzip.CopyTo(outputMs);
                encodedBytes = outputMs.ToArray();
            }

            // Step5 : Byte Deserialize Decoding
            var rowDatas = ZeroFormatterSerializer.Deserialize<List<SheetRowData>>(encodedBytes);
            return rowDatas;
        }

        /// <summary>
        /// 두 바이트 배열이 동일한지 확인
        /// </summary>
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