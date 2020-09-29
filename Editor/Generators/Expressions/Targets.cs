using System.CodeDom;

namespace CodeGeneration
{

    public class Target
    {
        public CodeExpression Expression { get; protected set; }
    }

    public class BaseTarget : ITarget
    {
        public CodeExpression Expression { get; protected set; }

        public BaseTarget()
        {
            Expression = new CodeBaseReferenceExpression();
        }
    }

    public class ThisTarget : ITarget
    {
        public CodeExpression Expression { get; protected set; }

        public ThisTarget()
        {
            Expression = new CodeThisReferenceExpression();
        }
    }

    public class FieldTarget : ITarget
    {
        public CodeExpression Expression { get; protected set; }
        public FieldTarget(string name)
        {
            Expression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name);
        }

        public FieldTarget(FieldGenerator field)
        {
            Expression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
        }

        public FieldTarget(string fieldContainer, string field)
        {
            Expression = new CodeFieldReferenceExpression(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldContainer), field);
        }
    }


    public class ClassTarget : ITarget
    {
        public CodeExpression Expression { get; protected set; }
        public ClassTarget(string name)
        {
            Expression = new CodeTypeReferenceExpression(name);
        }
    }

    public class VariableTarget : ITarget
    {
        public CodeExpression Expression { get; protected set; }
        public VariableTarget(string name)
        {
            Expression = new CodeVariableReferenceExpression(name);
        }
    }

    public class MethodTarget : ITarget
    {
        public CodeExpression Expression { get; protected set; }
        public MethodTarget(string name)
        {
            Expression = new CodeMethodReferenceExpression();
        }
    }
}