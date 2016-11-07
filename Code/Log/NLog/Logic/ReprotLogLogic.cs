using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NLog.Revicer
{
    public class ReprotLogLogic
    {
        public ReprotLogLogic()
        {
            AmountLogers = new BindingList<ReportLog> { };
            AmountLogers2 = new BindingList<ReportLog> { };
            AmountLogers3 = new BindingList<ReportLogContent> { };
            foreach (var item in ReportLog.CreateTypes())
            {
                AmountLogers.Add(item);
            }
        }
        public IList<ReportLog> AmountLogers { get; private set; }

        public IList<ReportLog> AmountLogers2 { get; private set; }

        public IList<ReportLogContent> AmountLogers3 { get; private set; }
     

        private void Logtype(SourceLog sourcelog)
        {
            var item = AmountLogers.FirstOrDefault(n => n.LogType == sourcelog.LogType);
            if (item == null) return;
            Add(sourcelog.Time, item);

        }
        private void LogContent(SourceLog sourcelog)
        {
            var item = AmountLogers3.FirstOrDefault(n => n.LogType == sourcelog.LogType && n.Logger == sourcelog.Logger && n.Message == sourcelog.Message);
            if (item == null)
            {
                item = new ReportLogContent { LogType = sourcelog.LogType, Logger = sourcelog.Logger, Message = sourcelog.Message };
                AmountLogers3.Add(item);
            }
            Add(sourcelog.Time, item);

        }

        private static void Add(DateTime time, ReportLog item)
        {
            item.Amount = item.Amount + 1;
            item.LastTime = time;
            item.Time.AddTime(time);
        }

        public  void AddLog(SourceLog sourcelog)
        {
            LogTypeAndLogger(sourcelog);
            LogContent(sourcelog);
            Logtype(sourcelog);
        }

        private void LogTypeAndLogger(SourceLog sourcelog)
        {
            var item = AmountLogers2.FirstOrDefault(n => n.LogType == sourcelog.LogType && n.Logger == sourcelog.Logger);
            if (item == null)
            {
                item = new ReportLog { LogType = sourcelog.LogType, Logger = sourcelog.Logger };
                AmountLogers2.Add(item);
            }
            Add(sourcelog.Time, item);
        }
    }
}