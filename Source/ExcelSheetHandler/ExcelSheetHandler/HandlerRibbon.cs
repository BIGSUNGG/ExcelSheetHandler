using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cysharp.Text;
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

        private void GenerateClassCodeBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Excel.Worksheet activeSheet = Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet;
                SheetHeaderData headerData = SheetHandler.Instance.ParseHeader(activeSheet);
                string code = CodeGenerator.Instance.GenerateClassCode(activeSheet.Name, headerData);
                ExcelSheetHandler.TextDisplayDialog.Show("생성된 클래스 코드", code);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GenerateJsonBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Excel.Worksheet activeSheet = Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet;
                var rows = SheetHandler.Instance.ParseRows(activeSheet);
                string json = JsonConvert.SerializeObject(rows, Formatting.Indented);
                ExcelSheetHandler.TextDisplayDialog.Show("생성된 JSON", json);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GenerateBinaryBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Excel.Worksheet activeSheet = Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet;
                var rows = SheetHandler.Instance.ParseRows(activeSheet);
                byte[] bytes = SheetRowDataSerializer.Instance.Serialize(rows, Convert.FromBase64String(AesKeyEditBox.Text));
                using (var zs = ZString.CreateStringBuilder())
                {
                    foreach (var b in bytes)
                        zs.Append(Convert.ToString(b, 8));

                    ExcelSheetHandler.TextDisplayDialog.Show("생성된 바이너리 데이터", zs.ToString());
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
