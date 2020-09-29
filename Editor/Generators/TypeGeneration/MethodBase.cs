using System;
using System.CodeDom;

namespace CodeGeneration
{

    public abstract class MethodBase : IMember
    {
        public abstract CodeTypeMember GetMember();
        public abstract CodeMemberMethod Method { get; }

        public MethodBase AddParameter(CodeTypeReference typeRef, string name)
        {
            Method.Parameters.Add(new CodeParameterDeclarationExpression(typeRef, name));
            return this;
        }

        public MethodBase AddParameter(Type type, string name)
        {
            Method.Parameters.Add(new CodeParameterDeclarationExpression(type, name));
            return this;
        }

        public MethodBase AddParameter(string type, string name)
        {
            Method.Parameters.Add(new CodeParameterDeclarationExpression(type, name));
            return this;
        }

        public MethodBase SetReturnType(string type)
        {
            Method.ReturnType = new CodeTypeReference(type);
            return this;
        }

        public MethodBase SetReturnType(Type type)
        {
            Method.ReturnType = new CodeTypeReference(type);
            return this;
        }

        public MethodBase AddStatement(CodeExpression expression)
        {
            Method.Statements.Add(expression);
            return this;
        }

        public MethodBase AddStatement(IStatement statement)
        {
            Method.Statements.Add(statement.GetExpression());
            return this;
        }

        public MethodBase AddStatement(string snippet)
        {
            Method.Statements.Add(new CodeSnippetExpression(snippet));
            return this;
        }

        public MethodBase AddStatement(StatementBuilder builder)
        {
            Method.Statements.AddRange(builder);
            return this;
        }

        public static implicit operator CodeMemberMethod(MethodBase gen)
        {
            return gen.Method;
        }

        public MethodBase AddComment(string comment)
        {
            Method.Comments.Add(new CodeCommentStatement(comment));
            return this;
        }
        public MethodBase AddNewLine()
        {
            Method.Comments.Add(new CodeCommentStatement("" + Environment.NewLine));
            return this;
        }
    }

}