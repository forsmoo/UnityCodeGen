using System;
using System.CodeDom;

namespace CodeGeneration
{

    public class DelegateGenerator : TypeGeneratorBase
    {
        public string Name;
        public readonly CodeTypeDelegate delegateType;
        public override CodeTypeDeclaration GetTypeDeclaration() { return delegateType; }

        public DelegateGenerator(string name)
        {
            this.Name = name;
            delegateType = new CodeTypeDelegate(name);
        }

        public DelegateGenerator AddParameter(Type type, string name)
        {
            delegateType.Parameters.Add(new CodeParameterDeclarationExpression(type, name));
            return this;
        }

        public DelegateGenerator AddParameter(string type, string name)
        {
            delegateType.Parameters.Add(new CodeParameterDeclarationExpression(type, name));
            return this;
        }

        public DelegateGenerator AddReturnType(string type)
        {
            delegateType.ReturnType = new CodeTypeReference(type);
            return this;
        }

        public DelegateGenerator AddReturnType(Type type)
        {
            delegateType.ReturnType = new CodeTypeReference(type);
            return this;
        }

        public static implicit operator CodeTypeDelegate(DelegateGenerator gen)
        {
            return gen.delegateType;
        }
    }

}