using System.Collections.Generic;
using System.IO;

namespace CodeGeneration
{

    public class UsingExpression : IExpression
    {
        public string Namespace;

        public void Begin(TextWriter writer)
        {
            writer.WriteLine("using " + Namespace + ";");
        }

        public void End(TextWriter writer)
        {

        }

        public IEnumerable<IUnit> ToUnits()
        {
            return new IUnit[] { new TextUnit("using " + Namespace + ";") };
        }
    }

}