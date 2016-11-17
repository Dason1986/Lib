using NLog.Revicer.Models;
using System;

namespace NLog.Revicer.Listeners
{

    public abstract  class LogAnalysis: ILogAnalysis
    {
        public LogAnalysis()
        {
            Config = new LogViewConfig();
            Report = new ReprotLogLogic();
            LogProvider = new NLogJsonProvider();
        }
        protected ILogProvider LogProvider { get; private set; }

        public LogViewConfig Config { get; protected set; }
        public ReprotLogLogic Report { get; protected set; }
        public event EventHandler<NewLogEventArgs> NewLog;

        protected void OnNewLog(SourceLog log)
        {
            var hanlder = NewLog;
            if (hanlder == null) return;
            hanlder.Invoke(this, new NewLogEventArgs(log));
        }

        public void SetConfig(LogViewConfig config, ReprotLogLogic report)
        {
            Config = config;
            Report = report;

        }
    }
}