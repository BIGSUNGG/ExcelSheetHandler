using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelSheetHandler
{
    public class SheetHeaderData
    {
        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 타입
        /// </summary>
        private List<SheetHeaderCell> _headerDatas = new List<SheetHeaderCell>();
        public IList<SheetHeaderCell> HeaderDatas => _headerDatas;

        public void AddHeaderData(string name, string type)
        {
            _headerDatas.Add(new SheetHeaderCell(name, type));
        }
    }
}