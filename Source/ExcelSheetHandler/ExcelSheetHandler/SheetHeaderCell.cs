using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSheetHandler
{
    /// <summary>
    /// ��Ʈ���� �ϳ��� ��� ������ ������ Ŭ����
    /// </summary>
    public class SheetHeaderCell
    {
        /// <summary>
        /// ���� �̸�
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��Ʈ���� ���� �̸� �ߺ� ����
        /// </summary>
        public bool IsDuplicatedName { get; set; }

        /// <summary>
        /// ���� Ÿ��
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