using NLog.Revicer.Models;
using System.Net;

namespace NLog.Revicer.Listeners
{
    public abstract class LogListener : LogAnalysis, ILogAnalysis
    {

        public void Start()
        {
            IsRunning = true;
            OnInit();
        }

        public void Stop()
        {
            IsRunning = false;
            OnStop();
        }
        protected virtual void OnStop()
        {

        }

        protected abstract void OnInit();
        public bool IsRunning { get; protected set; }


        protected virtual SourceLog GetLog(byte[] buff)
        {
            var log = System.Text.Encoding.UTF8.GetString(buff).Trim().Replace("\0", string.Empty);
            return LogProvider.Log(log);
        }


      
        protected virtual void ReceiveNewLog(byte[] data, string address)
        {
            var log = GetLog(data);
            log.Address = address;
            Report.AddLog(log);
            OnNewLog(log);
        }
        protected virtual void ReceiveNewLog(string data, string address)
        {
            var log = LogProvider.Log(data);
            log.Address = address;
            Report.AddLog(log);
            OnNewLog(log);
        }
    }

}
