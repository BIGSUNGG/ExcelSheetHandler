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
            HashSet<string> generatedName = new HashSet<string>();

            using(var zs = ZString.CreateStringBuilder())
            {
                zs.AppendLine($@"using System.Collections.Generic;
            
public class {className}");
                zs.AppendLine("{");
                foreach (var header in headerData.HeaderDatas)
                {
                    // 이미 만든 코드라면 건너뛰기
                    if (generatedName.Contains(header.Name))
                        continue;

                    if(header.IsDuplicatedName)
                        zs.AppendLine($"\tpublic List<{header.Type}> {header.Name} {{ get; set; }}");
                    else
                        zs.AppendLine($"\tpublic {header.Type} {header.Name} {{ get; set; }}");

                    generatedName.Add(header.Name);
                }
                zs.AppendLine("}");

                return zs.ToString();
            }
        }
    }
}