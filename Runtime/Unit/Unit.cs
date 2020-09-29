using System.IO;

namespace CodeGeneration
{
    public interface IUnit
    {
        void Output(TextWriter writer);
    }

    public class TextUnit : IUnit
    {
        public string Text;

        public TextUnit(string text)
        {
            Text = text;
        }

        public void Output(TextWriter writer)
        {
            writer.WriteLine(Text);
        }
    }
}
