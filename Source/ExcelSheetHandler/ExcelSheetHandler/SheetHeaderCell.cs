using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSheetHandler
{
    public class SheetHeaderCell
    {
        public string Name { get; set; }
        public bool IsDuplicatedName { get; set; }
        public string Type { get; set; }

        public SheetHeaderCell(string name, string type)
        {
            Name = name;
            Type = type;
            IsDuplicatedName = false;
        }
    }
}