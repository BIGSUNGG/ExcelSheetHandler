using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroFormatter;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace ExcelSheetHandler
{
    public class SheetRowDataSerializer
    {
        public static SheetRowDataSerializer Instance { get; private set; } = new SheetRowDataSerializer();

        public byte[] Serialize(List<SheetRowData> rowDatas)
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
            return compressedBytes;
        }
    }
}