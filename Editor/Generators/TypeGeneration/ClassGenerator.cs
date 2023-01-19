using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGeneration
{
    public class ClassGenerator : TypeGeneratorBase
    {
        public string Name;
        public CodeTypeDeclaration classType;

        public override CodeTypeDeclaration GetTypeDeclaration() { return classType; }

        public ClassGenerator(string name, MemberAttributes attributes = MemberAttributes.Public)
        {
            this.Name = name;
            classType = new CodeTypeDeclaration(Name);
            classType.Attributes = attributes;
            classType.TypeAttributes = TypeAttributes.Public;
        }

        public void MakeStatic()
        {
            classType.StartDirectives.Add(
                new CodeRegionDirective(CodeRegionMode.Start, "\nstatic"));
            classType.EndDirectives.Add(
                new CodeRegionDirective(CodeRegionMode.End, String.Empty));
        }

        public List<AutoPropertyGenerator> AutoProperties = new List<AutoPropertyGenerator>();
        public List<FieldGenerator> Fields = new List<FieldGenerator>();
        public List<PropertyGenerator> Properties = new List<PropertyGenerator>();
        public List<MethodBase> Methods = new List<MethodBase>();
        public List<EventGenerator> Events = new List<EventGenerator>();

        public void AddMember(CodeTypeMember member)
        {
            classType.Members.Add(member);
        }

        public ClassGenerator AddField(FieldGenerator field)
        {
            classType.Members.Add(field.Member);
            Fields.Add(field);
            return this;
        }

        public ClassGenerator AddAutoProperty(AutoPropertyGenerator property)
        {
            classType.Members.Add(property.GetMember());
            AutoProperties.Add(property);
            return this;
        }

        public ClassGenerator AddProperty(PropertyGenerator property)
        {
            classType.Members.Add(property.GetMember());
            Properties.Add(property);
            return this;
        }

        public ClassGenerator AddMethod(MethodBase method)
        {
            classType.Members.Add(method.GetMember());
            Methods.Add(method);
            return this;
        }

        public ClassGenerator AddEvent(EventGenerator evt)
        {
            classType.Members.Add(evt.Member);
            Events.Add(evt);
            return this;
        }

        public ClassGenerator AddBaseType(Type baseType)
        {
            classType.BaseTypes.Add(new CodeTypeReference(baseType));
            return this;
        }

        public ClassGenerator AddBaseType(string interfaceName)
        {
            classType.BaseTypes.Add(new CodeTypeReference(interfaceName));
            return this;
        }

        public ClassGenerator SetIsSealed(bool isSealed)
        {
            if (isSealed)
                classType.TypeAttributes |= TypeAttributes.Sealed;
            else
                classType.TypeAttributes = classType.TypeAttributes & ~TypeAttributes.Sealed;
            return this;
        }

        public ClassGenerator SetIsPartial(bool isPartial)
        {
            classType.IsPartial = isPartial;
            return this;
        }

        public ClassGenerator SetIsAbstract(bool isAbstract)
        {
            classType.TypeAttributes |= TypeAttributes.Abstract;
            return this;
        }

        public void AddNewLine()
        {
            //classType.Members.Add.Add(new CodeSnippetTypeMember(""));
        }
    }
}
