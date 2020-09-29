using System;
using System.CodeDom;

namespace CodeGeneration
{

    public class FieldGenerator : TypeMemberBase
    {
        public string Name;
        public CodeMemberField Member;
        public override CodeTypeMember GetMember() { return Member; }
        public string FieldType;

        public FieldGenerator(Type type, string name)
        {
            this.Name = name;
            this.FieldType = type.ToString();
            Member = new CodeMemberField(new CodeTypeReference(type), name);
        }

        public FieldGenerator(string type, string name)
        {
            this.Name = name;
            this.FieldType = type;
            Member = new CodeMemberField(type, name);
        }

        public FieldGenerator(string type, string name, bool isPublic)
        {
            this.Name = name;
            this.FieldType = type;
            Member = new CodeMemberField(type, name);
            Member.Attributes = isPublic ? MemberAttributes.Public : MemberAttributes.Private;
        }

        public static implicit operator CodeMemberField(FieldGenerator gen)
        {
            return gen.Member;
        }
    }

}