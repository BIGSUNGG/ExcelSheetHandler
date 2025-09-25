using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelSheetHandler
{
    public class SheetHeaderData
    {
        private List<SheetHeaderCell> _headerDatas = new List<SheetHeaderCell>();
        public IList<SheetHeaderCell> HeaderDatas => _headerDatas;

        /// <summary>
        /// 중복 이름 확인을 위한 추가된 헤더 이름 집합
        /// </summary>
        private HashSet<string> _nameChache = new HashSet<string>();

        public void AddHeaderData(string name, string type)
        {
            var newCellData = new SheetHeaderCell(name, type);
            _headerDatas.Add(newCellData);

            // 중복된 이름이 있다면 이름 중복 처리
            if (_nameChache.Contains(name))
            {
                _headerDatas.Where(d => d.Name == name)
                    .ToList()
                    .ForEach(d => d.IsDuplicatedName = true);
            }
            else
            {
                _nameChache.Add(name);
            }
        }
    }
}