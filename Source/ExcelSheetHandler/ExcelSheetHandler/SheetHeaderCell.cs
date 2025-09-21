using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSheetHandler
{
    public struct SheetHeaderCell
    {
        public string Name {get; private set;}
        public string Type {get; private set;}

        public SheetHeaderCell(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}