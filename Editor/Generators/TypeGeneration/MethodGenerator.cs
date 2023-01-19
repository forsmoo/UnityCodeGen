using System.CodeDom;

namespace CodeGeneration
{
    public class MethodGenerator : MethodBase
    {
        public string Name;
        public CodeMemberMethod Member { get; protected set; }
        public override CodeTypeMember GetMember() { return Member; }
        public override CodeMemberMethod Method { get { return Member; } }

        protected MethodGenerator()
        {

        }

        public MethodGenerator(string name)
        {
            this.Name = name;
            Member = new CodeMemberMethod() { Name = name };
            Member.Attributes = MemberAttributes.Public | MemberAttributes.Final; //If Final is not included it will be virtual
        }

        public void MakeStatic()
        {
            Member.Attributes |= MemberAttributes.Static;
        }

        public MethodGenerator(string eventName, DelegateGenerator forDelegate)
        {
            this.Name = eventName + "_" + forDelegate.Name;
            Member = new CodeMemberMethod() { Name = this.Name };
            Member.Attributes = MemberAttributes.Private | MemberAttributes.Final;
            Member.Parameters.AddRange(forDelegate.delegateType.Parameters);
            Member.ReturnType = forDelegate.delegateType.ReturnType;
        }

        public static implicit operator CodeMemberMethod(MethodGenerator gen)
        {
            return gen.Member;
        }

        public MethodBase SetAttributes(MemberAttributes attributes)
        {
            Member.Attributes = attributes;
            return this;
        }
    }

    public class MethodReturnField : MethodGenerator
    {
        public MethodReturnField(FieldGenerator field)
        {
            Member = new CodeMemberMethod()
            {
                Name = "Get" + field.Name,
                Attributes = MemberAttributes.Public
            };

            var returnStatement = new CodeMethodReturnStatement();
            Member.ReturnType = new CodeTypeReference(field.FieldType);
            Member.Statements.Add(returnStatement);

            var fieldReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
            returnStatement.Expression = fieldReference;
        }
    }

}