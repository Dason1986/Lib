using NLog.Revicer.Models;
using System;

namespace NLog.Revicer.Listeners
{
    public interface ILogAnalysis
    {
        LogViewConfig Config { get; }
        ReprotLogLogic Report { get; }

        event EventHandler<NewLogEventArgs> NewLog;

        void SetConfig(LogViewConfig config, ReprotLogLogic report);
    }
}