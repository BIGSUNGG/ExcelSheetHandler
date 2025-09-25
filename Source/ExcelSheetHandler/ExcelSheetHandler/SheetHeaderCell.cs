using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSheetHandler
{
    /// <summary>
    /// 시트에서 하나의 헤더 정보를 가지는 클래스
    /// </summary>
    public class SheetHeaderCell
    {
        /// <summary>
        /// 변수 이름
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 시트에서 변수 이름 중복 여부
        /// </summary>
        public bool IsDuplicatedName { get; set; }

        /// <summary>
        /// 변수 타입
        /// </summary>
        public string Type { get; set; }

        public SheetHeaderCell(string name, string type)
        {
            Name = name;
            Type = type;
            IsDuplicatedName = false;
        }
    }
}