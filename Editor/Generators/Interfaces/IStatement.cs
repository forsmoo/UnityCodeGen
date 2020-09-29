using System.CodeDom;

namespace CodeGeneration
{

    public interface IStatement
    {
        CodeExpression GetExpression();
    }

}