using System;
using System.CodeDom;

namespace CodeGeneration
{

    public class EventGenerator : IMember
    {
        public string Name;
        public CodeMemberEvent Member;
        public CodeTypeMember GetMember() { return Member; }

        public EventGenerator(string name, Type type)
        {
            this.Name = name;
            Member = new CodeMemberEvent()
            {
                Name = name,
                Type = new CodeTypeReference(type),
            };
            Member.Attributes = MemberAttributes.Final | MemberAttributes.Public;
        }

        public EventGenerator(string name, string type)
        {
            this.Name = name;
            Member = new CodeMemberEvent()
            {
                Name = name,
                Type = new CodeTypeReference(type)
            };
            Member.Attributes = MemberAttributes.Final | MemberAttributes.Public;
        }

        public EventGenerator(string name, CodeTypeDelegate type)
        {
            this.Name = name;
            Member = new CodeMemberEvent()
            {
                Name = name,
                Type = new CodeTypeReference(type.Name)
            };
            Member.Attributes = MemberAttributes.Final | MemberAttributes.Public;
        }

        public static implicit operator CodeMemberEvent(EventGenerator gen)
        {
            return gen.Member;
        }
    }
}