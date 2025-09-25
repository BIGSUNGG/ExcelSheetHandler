using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelSheetHandler
{
    /// <summary>
    /// 시트 하나의 모든 헤더 셀 저장|관리하는 클래스
    /// </summary>
    public class SheetHeaderData
    {
        private List<SheetHeaderCell> _headerDatas = new List<SheetHeaderCell>();
        public IList<SheetHeaderCell> HeaderDatas => _headerDatas;

        /// <summary>
        /// 중복 이름 확인을 위한 추가된 헤더 이름 집합
        /// Key : 헤더 이름
        /// Value : 헤더 타입
        /// </summary>
        private Dictionary<string, string> _cellChache = new Dictionary<string, string>();

        public void AddHeaderCell(string name, string type)
        {
            var newCellData = new SheetHeaderCell(name, type);
            _headerDatas.Add(newCellData);

            // 중복된 이름이 있다면 이름 중복 처리
            if (_cellChache.TryGetValue(name, out var typeName))
            {
                if (typeName != type)
                    throw new InvalidDataException($"{name} 변수의 데이터 타입이 {typeName}와 {type}으로 서로 다름, 데이터 테이블 헤더 확인 필요");

                // TODO : 이 부분 최적화 필요
                _headerDatas.Where(d => d.Name == name)
                    .ToList()
                    .ForEach(d => d.IsDuplicatedName = true);
            }
            else
            {
                _cellChache.Add(name, type);
            }
        }
    }
}