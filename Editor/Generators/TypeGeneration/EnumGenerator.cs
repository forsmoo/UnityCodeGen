using System.CodeDom;
using System.Reflection;

namespace CodeGeneration
{

    public class EnumGenerator : TypeGeneratorBase
    {
        public string Name;
        public CodeTypeDeclaration EnumType;
        public override CodeTypeDeclaration GetTypeDeclaration() { return EnumType; }
        public EnumGenerator(string name)
        {
            this.Name = name;
            EnumType = new CodeTypeDeclaration(name)
            {
                TypeAttributes = TypeAttributes.Public,
                IsEnum = true
            };
        }

        public void AddOption(string name)
        {
            CodeMemberField field = new CodeMemberField(EnumType.Name, name);
            EnumType.Members.Add(field);
        }

        public static implicit operator CodeTypeDeclaration(EnumGenerator gen)
        {
            return gen.EnumType;
        }
    }

}