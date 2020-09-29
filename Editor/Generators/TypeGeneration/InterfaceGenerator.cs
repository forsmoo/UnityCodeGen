using System;
using System.CodeDom;
using System.Reflection;

namespace CodeGeneration
{
    public class InterfaceGenerator : TypeGeneratorBase
    {
        public string Name;
        public CodeTypeDeclaration interfaceType;
        public override CodeTypeDeclaration GetTypeDeclaration() { return interfaceType; }
        public InterfaceGenerator(string name)
        {
            this.Name = name;
            interfaceType = new CodeTypeDeclaration(name)
            {
                TypeAttributes = TypeAttributes.Public,
                IsInterface = true
            };
        }

        public InterfaceGenerator AddMember(CodeTypeMember member)
        {
            interfaceType.Members.Add(member);
            return this;
        }

        public InterfaceGenerator AddMember(IMember member)
        {
            interfaceType.Members.Add(member.GetMember());
            return this;
        }

        public InterfaceGenerator AddBaseType(Type baseType)
        {
            interfaceType.BaseTypes.Add(new CodeTypeReference(baseType));
            return this;
        }

        public InterfaceGenerator AddAutoProperty(AutoPropertyGenerator property)
        {
            interfaceType.Members.Add(property.GetMember());
            return this;
        }

        public static implicit operator CodeTypeDeclaration(InterfaceGenerator gen)
        {
            return gen.interfaceType;
        }
    }
}