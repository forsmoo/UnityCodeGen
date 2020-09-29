using System.Collections.Generic;
using System.IO;

namespace CodeGeneration
{

    public class DebugFileWriter : ICodeWriter
    {
        public void Write(TextWriter stream, IEnumerable<IUnit> units)
        {
            foreach (var unit in units)
                unit.Output(stream);
        }
    }

}