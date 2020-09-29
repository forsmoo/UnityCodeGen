using System.CodeDom;
using System.Collections.Generic;

namespace CodeGeneration
{

    public class ParamBuilder
    {
        public List<CodeExpression> Parameters = new List<CodeExpression>();

        public ParamBuilder AddThis()
        {
            Parameters.Add(new CodeThisReferenceExpression());
            return this;
        }

        public ParamBuilder AddField(string source, string name)
        {
            Parameters.Add(new CodeFieldReferenceExpression(
                new CodeSnippetExpression(source),
                name));
            return this;
        }

        public ParamBuilder AddMemberField(string name)
        {
            Parameters.Add(new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                name));
            return this;
        }

        public ParamBuilder AddMemberProperty(string name)
        {
            Parameters.Add(new CodePropertyReferenceExpression(
                new CodeThisReferenceExpression(),
                name));
            return this;
        }

        public ParamBuilder AddVariable(string variable)
        {
            Parameters.Add(new CodeVariableReferenceExpression(variable));
            return this;
        }

        public ParamBuilder AddPrimitiveExpression(string value)
        {
            Parameters.Add(new CodePrimitiveExpression(value));
            return this;
        }

        public static implicit operator CodeExpression[](ParamBuilder builder)
        {
            return builder.Parameters.ToArray();
        }

        public ParamBuilder AddNew(string typeName)
        {
            Parameters.Add(new CodeObjectCreateExpression(new CodeTypeReference(typeName)));
            return this;
        }
    }

}