using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace CodeGeneration
{

    public class AssemblyWriter
    {
        public void Build(CodeCompileUnit ccu, string outputName)
        {
            var csharpcodeprovider = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();
            CompilerResults cr;
            cp.GenerateExecutable = true;
            cp.OutputAssembly = outputName;//"CSharpSample.exe";
            cp.GenerateInMemory = true;
            cr = csharpcodeprovider.CompileAssemblyFromDom(cp, ccu);
        }
    }

}