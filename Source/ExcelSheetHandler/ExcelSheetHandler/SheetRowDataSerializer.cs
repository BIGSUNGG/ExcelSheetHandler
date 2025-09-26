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

        public byte[] Serialize(List<SheetRowData> rowDatas, byte[] aesKey)
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
                aes.Key = aesKey;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    // prepend IV
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
            using (var hmacsha256 = new HMACSHA256(aesKey))
            {
                hmac = hmacsha256.ComputeHash(encryptedBytes);
            }

            var finalBytes = new byte[encryptedBytes.Length + hmac.Length];
            Buffer.BlockCopy(encryptedBytes, 0, finalBytes, 0, encryptedBytes.Length);
            Buffer.BlockCopy(hmac, 0, finalBytes, encryptedBytes.Length, hmac.Length);

            return finalBytes;
        }
    }
}