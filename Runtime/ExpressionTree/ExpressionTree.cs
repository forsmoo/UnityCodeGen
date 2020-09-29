using System.Collections.Generic;

namespace CodeGeneration
{
    public class ExpressionTree
    {
        public List<IExpression> Expressions;
    }

    public class ExpressionBuilder
    {
        public List<IExpression> Expressions;
    }

    public class ExpressionStructure
    {
        public IExpression Parent;
        public IExpression Children;
    }
}