using System;
using System.ComponentModel;

namespace NLog.Revicer
{
  //  [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider<HTime>))]
    public class ReportLog : PropertyChangeModel
    {
        private string _logger;
        private long _amount;
        static string[] types = { "TRACE", "DEBUG", "INFO", "WARN", "ERROR", "FATAL", "CLOSE" };
        private DateTime? _lastTime;
        HTime _time =new HTime();
        public string LogType { get; set; }
        public string Logger
        {
            get { return _logger; }
            set
            {
                if (_logger == value) return;
                _logger = value;
                this.OnPropertyChanged("Logger");
            }
        }
        public long Amount
        {
            get { return _amount; }
            set
            {
                if (_amount == value) return;
                _amount = value;
                this.OnPropertyChanged("Amount");
            }
        }
        public DateTime? LastTime
        {
            get { return _lastTime; }
            set
            {
                if (_lastTime == value) return;
                _lastTime = value;
                this.OnPropertyChanged("LastTime");
            }
        }
        public HTime Time
        {
            get { return _time; }
           
        }

        public static ReportLog[] CreateTypes()
        {
            ReportLog[] logs = new ReportLog[types.Length];
            for (int i = 0; i < logs.Length; i++)
            {
                logs[i] = new ReportLog { LogType = types[i] };
            }
            return logs;
        }

    }
}