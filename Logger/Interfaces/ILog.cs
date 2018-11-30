using System;

namespace Logger.Interfaces
{
    public interface ILog
    {
        void WriteLine(string text);
        void WriteException(Exception exception);
    }
}