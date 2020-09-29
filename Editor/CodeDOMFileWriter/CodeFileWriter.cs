using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace CodeGeneration
{
    public class CodeFileWriter
    {
        public void Write(CodeCompileUnit ccu, string sourceFile)
        {
            var csharpcodeprovider = new CSharpCodeProvider();
            using (var stream = new StreamWriter(sourceFile, false))
            {
                IndentedTextWriter tw1 = new IndentedTextWriter(stream, "     ");
                csharpcodeprovider.GenerateCodeFromCompileUnit(ccu, tw1, new CodeGeneratorOptions() { BlankLinesBetweenMembers = false });
                tw1.Close();
            }
        }
    }


}
