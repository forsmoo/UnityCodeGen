using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGeneration
{
    public class StructGenerator : TypeGeneratorBase
    {
        public string Name;
        public CodeTypeDeclaration structType;

        public override CodeTypeDeclaration GetTypeDeclaration() { return structType; }

        public StructGenerator(string name, MemberAttributes attributes = MemberAttributes.Public)
        {
            this.Name = name;
            structType = new CodeTypeDeclaration(Name);
            structType.Attributes = attributes;
            structType.TypeAttributes = TypeAttributes.Public;
            structType.IsStruct = true;
        }

        public List<AutoPropertyGenerator> AutoProperties = new List<AutoPropertyGenerator>();
        public List<FieldGenerator> Fields = new List<FieldGenerator>();
        public List<PropertyGenerator> Properties = new List<PropertyGenerator>();
        public List<MethodBase> Methods = new List<MethodBase>();
        public List<EventGenerator> Events = new List<EventGenerator>();

        public void AddMember(CodeTypeMember member)
        {
            structType.Members.Add(member);
        }

        public StructGenerator AddField(FieldGenerator field)
        {
            structType.Members.Add(field.Member);
            Fields.Add(field);
            return this;
        }

        public StructGenerator AddMethod(MethodBase method)
        {
            structType.Members.Add(method.GetMember());
            Methods.Add(method);
            return this;
        }

        public StructGenerator AddBaseType(Type baseType)
        {
            structType.BaseTypes.Add(new CodeTypeReference(baseType));
            return this;
        }

        public StructGenerator AddBaseType(string interfaceName)
        {
            structType.BaseTypes.Add(new CodeTypeReference(interfaceName));
            return this;
        }

        public StructGenerator SetIsSealed(bool isSealed)
        {
            if (isSealed)
                structType.TypeAttributes |= TypeAttributes.Sealed;
            else
                structType.TypeAttributes = structType.TypeAttributes & ~TypeAttributes.Sealed;
            return this;
        }

    }
}
