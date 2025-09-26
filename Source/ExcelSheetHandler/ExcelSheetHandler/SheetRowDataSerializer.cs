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
        private readonly ByteEncodingSerializer _byteEncoding = new ByteEncodingSerializer();
        private readonly GZipCompressor _gzip = new GZipCompressor();
        private readonly AesCbcEncryptor _aes = new AesCbcEncryptor();
        private readonly HmacIntegrityValidator _hmac = new HmacIntegrityValidator();

        public byte[] Serialize(List<SheetRowData> rowDatas, byte[] secretKey)
        {
            // Step1 : Byte Serialize Encoding
            byte[] encodedBytes = _byteEncoding.Serialize(rowDatas);

            // Step2 : Compress Bytes
            byte[] compressedBytes = _gzip.Compress(encodedBytes);

            // Step3 : Encrypt Bytes
            byte[] encryptedBytes = _aes.Encrypt(compressedBytes, secretKey);

            // Step4 : Anti Tampering Bytes
            return _hmac.AppendHmac(encryptedBytes, secretKey);
        }

        public List<SheetRowData> Deserialize(byte[] protectedBytes, byte[] secretKey)
        {
            // Step1 : Split/Verify Anti Tampering Bytes
            var encryptedBytes = _hmac.VerifyAndStripHmac(protectedBytes, secretKey);

            // Step2 : Decrypt Bytes
            byte[] compressedBytes = _aes.Decrypt(encryptedBytes, secretKey);

            // Step3 : Decompress Bytes
            byte[] encodedBytes = _gzip.Decompress(compressedBytes);

            // Step4 : Byte Deserialize Decoding
            return _byteEncoding.Deserialize(encodedBytes);
        }
    }
}