using System.CodeDom;
using System.Collections.Generic;

namespace CodeGeneration
{

    public class CodeUnitGenerator
    {
        public List<string> ReferencedAssemblies = new List<string>();
        public List<string> NamespaceImports = new List<string>();
        public string Namespace
        {
            get { return currentNamespace.Name; }
        }
        private readonly CodeNamespace currentNamespace;

        public CodeUnitGenerator(string namespaceName)
        {
            currentNamespace = new CodeNamespace(namespaceName);
        }

        public void AddType(ITypeGenerator type)
        {
            currentNamespace.Types.Add(type.GetTypeDeclaration());
        }

        public CodeCompileUnit GenerateCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            foreach (var reference in ReferencedAssemblies)
            {
                compileUnit.ReferencedAssemblies.Add(reference);
            }

            currentNamespace.Imports.Clear();
            var imports = new List<CodeNamespaceImport>();
            foreach (var ns in NamespaceImports)
            {
                var cuNamespace = new CodeNamespaceImport(ns);
                imports.Add(cuNamespace);

                currentNamespace.Imports.Add(cuNamespace);
            }

            compileUnit.Namespaces.Add(currentNamespace);
            return compileUnit;
        }
    }

}