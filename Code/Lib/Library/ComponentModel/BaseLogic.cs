using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Library.ComponentModel.Logic
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BaseLogic : ILogic, IStart
    {
        /// <summary>
        ///
        /// </summary>
        public event MessageEventHandler Messge;

        /// <summary>
        ///
        /// </summary>
        public event ExceptionEventHandler Failure;

        /// <summary>
        ///
        /// </summary>
        public event CompletedEventHandler Completed;

        /// <summary>
        ///
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        ///
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnFailure(Exception ex)
        {
            ExceptionEventHandler handler = Failure;
            var message = string.Format("Date:{2}\r\nMessage:{0}\r\nTarget:{3}\r\nStackTrace:{1}", ex.Message, ex.StackTrace, ex.TargetSite, DateTime.Now);
            Trace.TraceError(message);
            if (handler != null)
            {
                SynchronizationContext.Current.Post(n =>
                {
                    handler(this, new ExceptionEventArgs(ex));
                }, ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCompleted(TimeSpan useTime)
        {
            CompletedEventHandler handler = Completed;
            if (handler != null)
            {
                SynchronizationContext.Current.Post(n =>
                {
                    handler(this, new CompletedEventArgs(useTime));
                }, useTime);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnMessge(string message)
        {
            MessageEventHandler handler = Messge;

            if (handler != null) handler(this, new MessageEventArgs(message));
        }

        private Thread threadWork;

        /// <summary>
        ///
        /// </summary>
        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            threadWork = new Thread((x) =>
            {
                try
                {
                    Stopwatch watch = new Stopwatch(); watch.Start();
                    OnStart();
                    watch.Stop();
                    OnCompleted(watch.Elapsed);
                }
                catch (Exception e)
                {
                    OnFailure(e);
                }
                IsRunning = false;
            });
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        ///
        /// </summary>
        /// <param name="progressPercentage"></param>
        protected virtual void OnProgressChanged(int progressPercentage)
        {
            var handler = ProgressChanged;
            if (handler != null)
            {
                SynchronizationContext.Current.Post(n =>
                {
                    handler(this, new ProgressChangedEventArgs(progressPercentage));
                }, progressPercentage);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Stop()
        {
            try
            {
                if (threadWork != null)
                    threadWork.Abort();
                IsRunning = false;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }
    }
}