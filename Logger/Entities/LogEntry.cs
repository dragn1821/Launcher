using System;

namespace Logger.Entities
{
    public class LogEntry
    {
        protected const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss.ff";

        protected DateTime date;
        protected string text;
               
        public LogEntry(DateTime date, string text)
        {
            this.date = date;
            this.text = text;
        }

        public override string ToString()
        {
            return $"{date.ToString(DATETIME_FORMAT)} {text}";
        }
    }
}