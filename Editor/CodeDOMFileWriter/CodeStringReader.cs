using Microsoft.CSharp;
using System.CodeDom;
using System.IO;

namespace CodeGeneration
{

    public class CodeStringReader
    {
        public CodeCompileUnit Parse(string source)
        {
            var csharpcodeprovider = new CSharpCodeProvider();
            CodeCompileUnit ccu;
            using (var stream = new StringReader(source))
            {
                ccu = csharpcodeprovider.Parse(stream);
            }
            return ccu;
        }
    }

}