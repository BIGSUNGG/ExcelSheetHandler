using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelSheetHandler
{
    public partial class HandlerRibbon
    {        
        private void HandlerRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void ToJsonBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Excel.Worksheet activeSheet = Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet;
            List<SheetRawData> rowDatas = SheetHandler.Instance.ParseRows(activeSheet);

            List<string> rowJsons = new List<string>(rowDatas.Count);
            foreach (var rowData in rowDatas)
            {
                rowJsons.Add(SheetRawDataJsonConvert.SerializeData(rowData));
            }
        }
    }
}
