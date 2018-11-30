using Logger.Interfaces;
using System;

namespace Logger
{
    public abstract class LogBase : ILog
    {
        protected IStringBuilderWrapper builder;

        public LogBase(IStringBuilderWrapper builder)
        {
            this.builder = builder;
        }

        #region Abstract Methods

        public abstract void WriteLine(string text);

        #endregion

        #region Public Methods

        public void WriteException(Exception exception)
        {
            int count         = 0;
            Exception current = exception;
            builder.Clear();
            do
            {
                builder.AppendLine("An Exception Occurred:");
                builder.Append("<=== Level ");
                builder.Append(count.ToString());
                builder.AppendLine(" ===>");
                builder.Append("Exception: [");
                builder.Append(current.GetType().FullName);
                builder.Append("] ");
                builder.AppendLine(current.Message);
                builder.Append("\tSource=");
                builder.AppendLine(current.Source);
                builder.Append("\tTarget=");
                builder.AppendLine((current.TargetSite == null) ? "" : current.TargetSite.Name);
                builder.AppendLine("\tStack Trace:");
                builder.AppendLine(current.StackTrace);

                current = current.InnerException;
                ++count;
            } while (current != null);

            WriteLine(builder.ToString());
        }

        #endregion
    }
}