using System;
using System.IO;

namespace CodeGeneration
{
    public interface IExpression
    {
        //string ToCodeString();
        /*void Begin(TextWriter writer);
        void End(TextWriter writer);*/
        //IEnumerable<IUnit> ToUnits();
        //void Write(TextWriter writer);
    }

    public class NamespaceExpression : IExpression
    {
        public string Namespace;

        public void Write(TextWriter writer)
        {
            throw new NotImplementedException();
        }

        public void End(TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public class ClassExpression : IExpression
    {
        public string Name;
        public ClassExpression(string name)
        {
            this.Name = name;
        }

        public void Write(TextWriter writer)
        {
            throw new NotImplementedException();
        }
        public void End(TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public class ConstructorExpression : IExpression
    {
        public void Write(TextWriter writer)
        {
            throw new NotImplementedException();
        }
        public void End(TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public class MethodExpression : IExpression
    {
        public void Write(TextWriter writer)
        {
            throw new NotImplementedException();
        }
        public void End(TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}