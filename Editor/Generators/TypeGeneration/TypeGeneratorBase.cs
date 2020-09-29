using System.CodeDom;
using System.Reflection;

namespace CodeGeneration
{

    public abstract class TypeGeneratorBase : ITypeGenerator
    {
        public abstract CodeTypeDeclaration GetTypeDeclaration();

        public TypeGeneratorBase SetTypeAttributes(TypeAttributes attributes)
        {
            GetTypeDeclaration().TypeAttributes = attributes;
            return this;
        }

        public TypeGeneratorBase SetCustomAttribute(string attributeType, ParamBuilder parameters)
        {
            var attribute = new CodeAttributeDeclaration(attributeType);
            foreach (var paraameter in parameters.Parameters)
            {
                attribute.Arguments.Add(new CodeAttributeArgument(paraameter));
            }

            GetTypeDeclaration().CustomAttributes.Add(attribute);
            return this;
        }
    }

}