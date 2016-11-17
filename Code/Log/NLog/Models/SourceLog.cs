using System;

namespace NLog.Revicer.Models
{
    public class SourceLog
    {
        public SourceLog()
        {

        }
        protected internal SourceLog(string[] sourceItems)
        {
            SourceItems = sourceItems;
        }

        public string ID { get; set; }
        public DateTime Time { get; set; }
        public string LogType { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string Machinename { get; set; }
        public string Processname { get; set; }
        public string Processtime { get; set; }
        public string Appdomain { get; set; }
        public string Callsite { get; set; }
        public string Context { get; set; }

        public string[] SourceItems { get; private set; }

        public LogException Error { get; set; }

    }
    public class LogException
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public LogException InnerException { get; set; }
    }
}

