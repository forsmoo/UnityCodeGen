using System.CodeDom;

namespace CodeGeneration
{
    public class ConstructorGenerator : MethodBase
    {
        public string Name;
        public CodeConstructor Member { get; protected set; }
        public override CodeTypeMember GetMember() { return Member; }
        public override CodeMemberMethod Method { get { return Member; } }

        public ConstructorGenerator()
        {
            Member = new CodeConstructor();
            Member.Attributes = MemberAttributes.Public;
        }

        public ConstructorGenerator(CodeTypeDeclaration classType)
        {
            //Constructor for all fields
            Member = new CodeConstructor();
            Member.Attributes = MemberAttributes.Public;
            StatementBuilder builder = new StatementBuilder();
            foreach (var member in classType.Members)
            {
                var field = (member as CodeMemberField);
                if (field != null)
                {
                    AddParameter(field.Type, field.Name);
                    builder.AddConstructorFieldAssignement(field.Name, field.Name);
                }
            }
            AddStatement(builder);
        }

        public ConstructorGenerator AddBaseCall(string variable)
        {
            Member.BaseConstructorArgs.Add(new CodeVariableReferenceExpression(variable));
            return this;
        }

        public ConstructorGenerator AddBaseCallHardcoded(string variable)
        {
            Member.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("\"" + variable + "\""));
            return this;
        }

    }
}