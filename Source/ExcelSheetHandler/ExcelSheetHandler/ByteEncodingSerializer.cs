using System.Collections.Generic;
using ZeroFormatter;

namespace ExcelSheetHandler
{
    public class ByteEncodingSerializer
    {
        public byte[] Serialize(List<SheetRowData> rows)
        {
            return ZeroFormatterSerializer.Serialize(rows);
        }

        public List<SheetRowData> Deserialize(byte[] bytes)
        {
            return ZeroFormatterSerializer.Deserialize<List<SheetRowData>>(bytes);
        }
    }
}


