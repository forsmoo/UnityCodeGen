using System;
using System.CodeDom;

namespace CodeGeneration
{

    public class AutoPropertyGenerator : TypeMemberBase
    {
        private CodeSnippetTypeMember snippet;
        public override CodeTypeMember GetMember() { return snippet; }

        public string Name;
        public string PropertyType;
        private string tabbedNewline = Environment.NewLine + Environment.NewLine + "\t\t";
        public AutoPropertyGenerator(string type, string name) : base()
        {
            this.Name = name;
            this.PropertyType = type;
            snippet = new CodeSnippetTypeMember { Text = string.Format("public {0} {1} {{ get; set; }}" + tabbedNewline, type, name) };
        }

        public AutoPropertyGenerator ForInterfaceGet()
        {
            snippet = new CodeSnippetTypeMember { Text = string.Format("{0} {1} {{ get; }}" + tabbedNewline, PropertyType, Name) };
            return this;
        }

        public AutoPropertyGenerator InterfaceImplementation(bool protectedSet)
        {
            string setAccessor = protectedSet ? "protected" : "";
            snippet = new CodeSnippetTypeMember { Text = string.Format("public {0} {1} {{ get; {2} set; }}" + tabbedNewline, PropertyType, Name, setAccessor) };
            return this;
        }

        public AutoPropertyGenerator InterfaceImplementation(string code)
        {
            snippet = new CodeSnippetTypeMember { Text = string.Format("public {0} {1} {{ {2} }}" + tabbedNewline, PropertyType, Name, code) };
            return this;
        }
    }
}