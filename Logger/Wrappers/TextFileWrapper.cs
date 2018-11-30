using Logger.Interfaces;
using System;
using System.IO;

namespace Logger.Wrappers
{
    public class TextFileWrapper : ITextFile
    {
        private const string DATE_FORMAT = "yyyy-MM-dd";

        public string Name { get; private set; }
        public string Path { get; private set; }

        public TextFileWrapper(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        public void AppendLine(string text)
        {
            string filename = GetFileName();

            using (StreamWriter writer = File.AppendText(filename))
            {
                writer.WriteLine(text);
            }
        }

        private string GetFileName()
        {
            DirectoryInfo directory = new DirectoryInfo(Path);
            directory.Create();
            return System.IO.Path.Combine(directory.FullName, $"{Name}-{DateTime.Now.ToString(DATE_FORMAT)}.log");
        }
    }
}