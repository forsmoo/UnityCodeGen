using System.Collections.Generic;
using System.IO;

namespace CodeGeneration
{

    public interface ICodeWriter
    {
        void Write(TextWriter stream, IEnumerable<IUnit> expressions);
    }

}