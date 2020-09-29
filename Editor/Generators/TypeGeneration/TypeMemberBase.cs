using System.CodeDom;

namespace CodeGeneration
{

    public abstract class TypeMemberBase : IMember
    {
        public void AddComment(string comment)
        {
            GetMember().Comments.Add(new CodeCommentStatement(comment));
        }

        public abstract CodeTypeMember GetMember();

        public TypeMemberBase SetCustomAttribute(string attributeType, ParamBuilder parameters)
        {
            var attribute = new CodeAttributeDeclaration(attributeType);
            foreach (var paraameter in parameters.Parameters)
            {
                attribute.Arguments.Add(new CodeAttributeArgument(paraameter));
            }

            GetMember().CustomAttributes.Add(attribute);
            return this;
        }

        public static implicit operator CodeTypeMember(TypeMemberBase gen)
        {
            return gen.GetMember();
        }
    }

}