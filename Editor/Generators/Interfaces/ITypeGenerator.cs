using System.CodeDom;

namespace CodeGeneration
{

    public interface ITypeGenerator
    {
        CodeTypeDeclaration GetTypeDeclaration();
    }

}