using Library.ComponentModel.Logic;
using System;
using System.Diagnostics;
using System.Threading;

namespace Library.Infrastructure.Application
{
    public abstract class BaseLogicService : ILogicService
    {
        public BaseLogicService()
        {
            Logger = NLog.LogManager.GetLogger(this.GetType().FullName);
            //   logerName = this.GetType().FullName;
        }

        protected NLog.ILogger Logger { get; set; }
        IOption ILogicService.Option
        {
            get
            {
                return ServiceOption;
            }

            set
            {
                ServiceOption = value;
            }
        }
        protected abstract IOption ServiceOption { get; set; }

        public event EventHandler Completed;
        protected void OnCompleted(TimeSpan usetime)
        {

            Logger.Trace("完成執行");
            var handler = Completed;
            if (handler == null) return;
            SynchronizationContext.Current.Post(n =>
            {

                handler.Invoke(this, EventArgs.Empty);
            }, null);
        }

        public void Start()
        {
            if (!OnVerification())
            {
                Logger.Warn("驗證失敗");

                return;
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            Logger.Trace("開始執行");

            OnDowrok();
            watch.Stop();
            OnCompleted(watch.Elapsed);
        }
        Thread threadPool;
        public void StartAsyn()
        {
            if (threadPool != null) return;
            threadPool = new Thread(n =>
            {
                Start();
            });
            threadPool.Start();
        }
        protected abstract void OnDowrok();
        protected virtual bool OnVerification()
        {
            return true;
        }

        public void StopAsyn()
        {
            if (threadPool != null)
            {
                threadPool.Abort();
                threadPool = null;
            }
        }
    }
}
