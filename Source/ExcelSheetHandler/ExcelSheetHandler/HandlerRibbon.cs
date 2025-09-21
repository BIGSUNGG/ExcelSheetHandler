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
            var result = SheetHandler.Instance.ParseSheet(activeSheet);
        }
    }
}
