using System;
using System.CodeDom;

namespace CodeGeneration
{

    public class PropertyGenerator : TypeMemberBase
    {
        public string Name;
        public string PropertyType;
        protected CodeMemberProperty Member;
        public override CodeTypeMember GetMember() { return Member; }

        public PropertyGenerator(string type, string name)
        {
            this.Name = name;
            this.PropertyType = type;
            Member = new CodeMemberProperty();
            Member.Name = name;
            Member.Type = new CodeTypeReference(type);
            Member.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            throw new NotImplementedException();
        }

    }

}