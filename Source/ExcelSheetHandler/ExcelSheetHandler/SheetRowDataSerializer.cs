using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroFormatter;

namespace ExcelSheetHandler
{
    public class SheetRowDataSerializer
    {
        public static SheetRowDataSerializer Instance { get; private set; } = new SheetRowDataSerializer();

        public byte[] Serialize(List<SheetRowData> rowDatas)
        {
            return ZeroFormatterSerializer.Serialize(rowDatas);
        }
    }
}