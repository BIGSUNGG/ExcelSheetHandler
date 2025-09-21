using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json;
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

            // SerializeDatas 메서드 사용
            string json = SheetRawDataJsonConvert.SerializeDatas(rowDatas);

            ExcelSheetHandler.TextDisplayDialog.Show("JSON 데이터", json);
        }

        private void GenerateClassCodeBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Excel.Worksheet activeSheet = Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet;
            SheetHeaderData headerData = SheetHandler.Instance.ParseHeader(activeSheet);
            string code = CodeGenerator.Instance.GenerateClassCode(activeSheet.Name, headerData);
            ExcelSheetHandler.TextDisplayDialog.Show("생성된 클래스 코드", code);
        }
    }
}
