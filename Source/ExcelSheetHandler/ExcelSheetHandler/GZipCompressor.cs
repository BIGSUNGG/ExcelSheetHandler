using System.IO;
using System.IO.Compression;

namespace ExcelSheetHandler
{
    public class GZipCompressor
    {
        public byte[] Compress(byte[] input)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal, leaveOpen: true))
                {
                    gzipStream.Write(input, 0, input.Length);
                }
                return memoryStream.ToArray();
            }
        }

        public byte[] Decompress(byte[] input)
        {
            using (var inputMs = new MemoryStream(input))
            using (var gzip = new GZipStream(inputMs, CompressionMode.Decompress))
            using (var outputMs = new MemoryStream())
            {
                gzip.CopyTo(outputMs);
                return outputMs.ToArray();
            }
        }
    }
}


