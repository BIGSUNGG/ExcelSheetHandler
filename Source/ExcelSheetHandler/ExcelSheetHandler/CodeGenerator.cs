using Cysharp.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelSheetHandler
{
    public class CodeGenerator
    {
        public static CodeGenerator Instance { get; private set; } = new CodeGenerator();

        public string GenerateClassCode(string className, SheetHeaderData headerData)
        {
            using(var zs = ZString.CreateStringBuilder())
            {
                zs.AppendLine($@"using System.Collections.Generic;
            
public class {className}");
                zs.AppendLine("{");
                foreach (var header in headerData.HeaderDatas)
                {
                    zs.AppendLine($"\tpublic {header.Type} {header.Name} {{ get; set; }}");
                }
                zs.AppendLine("}");

                return zs.ToString();
            }
        }
    }
}