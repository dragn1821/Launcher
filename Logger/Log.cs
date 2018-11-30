using Logger.Entities;
using Logger.Interfaces;
using Logger.Wrappers;
using System;

namespace Logger
{
    public class Log : LogBase
    {
        private ITextFile textFile;

        public Log(string name, string path) 
            : this(new TextFileWrapper(name, path), new StringBuilderWrapper())
        { }

        public Log(ITextFile textFile, IStringBuilderWrapper builder) 
            : base(builder)
        {
            this.textFile = textFile;
        }

        public override void WriteLine(string text)
        {
            LogEntry entry = new LogEntry(DateTime.Now, text);
            textFile.AppendLine(entry.ToString());
        }
    }
}