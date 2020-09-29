using System.CodeDom;

namespace CodeGeneration
{
    public interface ITarget
    {
        CodeExpression Expression { get; }
    }
}