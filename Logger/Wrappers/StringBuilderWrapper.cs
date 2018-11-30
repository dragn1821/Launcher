using Logger.Interfaces;
using System.Text;

namespace Logger.Wrappers
{
    public class StringBuilderWrapper : IStringBuilderWrapper
    {
        private StringBuilder builder = new StringBuilder();

        public void Append(string text)
        {
            builder.Append(text);
        }

        public void AppendLine(string text)
        {
            builder.AppendLine(text);
        }

        public void Clear()
        {
            builder.Clear();
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}