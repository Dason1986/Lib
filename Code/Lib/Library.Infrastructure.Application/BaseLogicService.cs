using Library.ComponentModel.Logic;
using System;
using System.Diagnostics;
using System.Threading;

namespace Library.Infrastructure.Application
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseLogicService : ILogicService
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseLogicService()
        {
            Logger = NLog.LogManager.GetLogger(this.GetType().FullName);
            //   logerName = this.GetType().FullName;
        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        protected abstract IOption ServiceOption { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Completed;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usetime"></param>
        protected void OnCompleted(TimeSpan usetime)
        {

            Logger.InfoByContent("Completed", usetime);
            var handler = Completed;
            if (handler == null) return;
            SynchronizationContext.Current.Post(n =>
            {

                handler.Invoke(this, EventArgs.Empty);
            }, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (!OnVerification())
            {
                Logger.Warn("Verification ");

                return;
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            Logger.Info("Start");

            OnDowrok();
            watch.Stop();
            OnCompleted(watch.Elapsed);
        }
        Thread threadPool;
        /// <summary>
        /// 
        /// </summary>
        public void StartAsyn()
        {
            if (threadPool != null) return;
            threadPool = new Thread(n =>
            {
                Start();
            });
            threadPool.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnDowrok();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnVerification()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
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
