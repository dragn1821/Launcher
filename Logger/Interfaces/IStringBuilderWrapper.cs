namespace Logger.Interfaces
{
    public interface IStringBuilderWrapper
    {
        void Clear();
        void Append(string text);
        void AppendLine(string text);
        string ToString();
    }
}