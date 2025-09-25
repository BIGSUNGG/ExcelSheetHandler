using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelSheetHandler
{
    public class SheetHandler
    {
        public static SheetHandler Instance { get; private set; } = new SheetHandler();

        public SheetHeaderData ParseHeader(Excel.Worksheet activeSheet)
        {
            SheetHeaderData result = new SheetHeaderData();

            Excel.Range usedRange = activeSheet.UsedRange;

            int columnCount = usedRange.Columns.Count;

            for (int column = 1; column <= columnCount; column++)
            {
                Excel.Range nameCell = usedRange.Cells[1, column];
                Excel.Range typeCell = usedRange.Cells[2, column];
                string name = nameCell.Value2;
                string type = typeCell.Value2;
                result.AddHeaderCell(name, type);
            }

            return result;
        }

        public List<SheetRowData> ParseRows(Excel.Worksheet activeSheet)
        {
            SheetHeaderData headerData = ParseHeader(activeSheet);
            List<SheetRowData> result = new List<SheetRowData>();

            Excel.Range usedRange = activeSheet.UsedRange;

            int rowCount = usedRange.Rows.Count;
            int colCount = usedRange.Columns.Count;

            // 각 셀의 데이터 출력
            for (int row = 3; row <= rowCount; row++)
            {
                SheetRowData rawData = new SheetRowData();

                for (int col = 1; col <= colCount; col++)
                {
                    Excel.Range cell = usedRange.Cells[row, col];
                    SheetHeaderCell curHeaderCell = headerData.HeaderDatas[col - 1];
                    rawData.SetData(curHeaderCell.Type, curHeaderCell.Name, cell.Value2);
                }

                result.Add(rawData);
            }

            return result;
        }
    }
}