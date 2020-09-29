using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

namespace CodeGeneration
{

    public class StringCompiler
    {
        public static string CompileToString(CodeCompileUnit ccu)
        {
            StringBuilder sb = new StringBuilder();
            using (var stream = new StringWriter(sb))
            {
                CSharpCodeProvider csharpcodeprovider = new CSharpCodeProvider();
                IndentedTextWriter tw1 = new IndentedTextWriter(stream, "     ");
                csharpcodeprovider.GenerateCodeFromCompileUnit(ccu, tw1, new CodeGeneratorOptions());
                tw1.Close();
            }
            return sb.ToString();
        }
    }

}