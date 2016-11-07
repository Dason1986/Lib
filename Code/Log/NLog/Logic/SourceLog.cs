using System;

namespace NLog.Revicer
{
    public class SourceLog
    {
        protected SourceLog(string[] sourceItems)
        {
            SourceItems = sourceItems;
        }
        public DateTime Time { get; protected set; }
        public string LogType { get; protected set; }

        public string Logger { get; protected set; }

        public string Message { get; protected set; }

        public string Content { get; protected set; }

        public string[] SourceItems { get; private set; }
        public static SourceLog NLog(string source)
        {
            return NLog(source.Split('|'));
        }
        public static SourceLog NLog(string[] source)
        {
            if (source == null || source.Length < 4) throw new Exception();//format error
            SourceLog log = new SourceLog(source);
            log.Time = DateTime.Parse(source[0]);
            log.LogType = source[1];
            log.Logger = source[2];
            log.Message = source[3];
            if (source.Length > 4) log.Content = source[4];
            return log;
        }
    }
}