using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Library.ComponentModel.Logic
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="error"></param>
        internal ExceptionEventArgs(Exception error)
        {
            Error = error;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        ///
        /// </summary>
        Info,

        /// <summary>
        ///
        /// </summary>
        OKAction,
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class CompletedEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        public TimeSpan UseTime { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="useTime"></param>
        public CompletedEventArgs(TimeSpan useTime)
        {
            UseTime = useTime;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ProgressChangedEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="progressPercentage"></param>
        public ProgressChangedEventArgs(int progressPercentage)
        {
            ProgressPercentage = progressPercentage;
        }

        /// <summary>
        ///
        /// </summary>
        public int ProgressPercentage { get; protected set; }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class MessageEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public MessageType MessageInfo { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        public MessageEventArgs(string message, MessageType messageType = MessageType.Info)
        {
            Message = message;
            MessageInfo = messageType;
            Time = DateTime.Now;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    /// <summary>
    ///
    /// </summary>
    public interface ILogic
    {
        /// <summary>
        ///
        /// </summary>
        event MessageEventHandler Messge;

        /// <summary>
        ///
        /// </summary>
        event ExceptionEventHandler Failure;

        /// <summary>
        ///
        /// </summary>
        event CompletedEventHandler Completed;

        /// <summary>
        ///
        /// </summary>
        event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        ///
        /// </summary>
        void Start();

        /// <summary>
        ///
        /// </summary>
        void StartAsync();
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class BaseLogic : ILogic
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
        protected virtual void OnFailure(Exception ex)
        {
            ExceptionEventHandler handler = Failure;
            var message = string.Format("Date:{2}\r\nMessage:{0}\r\nTarget:{3}\r\nStackTrace:{1}", ex.Message, ex.StackTrace, ex.TargetSite, DateTime.Now);
            Trace.TraceError(message);
            if (handler != null) handler(this, new ExceptionEventArgs(ex));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCompleted(TimeSpan useTime)
        {
            CompletedEventHandler handler = Completed;
            if (handler != null) handler(this, new CompletedEventArgs(useTime));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        protected virtual void OnMessge(string message, MessageType messageType = MessageType.Info)
        {
            MessageEventHandler handler = Messge;
            Trace.WriteLine(string.Format("{0}:{1}", messageType, message));
            if (handler != null) handler(this, new MessageEventArgs(message, messageType));
        }

        /// <summary>
        ///
        /// </summary>
        public void StartAsync()
        {
            BackgroundWorker background = new BackgroundWorker();
            Stopwatch watch = new Stopwatch();

            background.DoWork += (x, y) =>
            {
                watch.Start();
                OnStart();
                watch.Stop();
            };
            background.ProgressChanged += (x, y) =>
            {
                OnProgressChanged(y.ProgressPercentage);
            };
            background.RunWorkerCompleted += (x, y) =>
            {
                if (y.Error == null)
                {
                    OnCompleted(watch.Elapsed);
                }
                else
                {
                    OnFailure(y.Error);
                }
            };

            background.RunWorkerAsync();
        }

        /// <summary>
        ///
        /// </summary>
        public void Start()
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                OnStart();
                watch.Stop();
                OnCompleted(watch.Elapsed);
            }
            catch (Exception ex)
            {
                OnFailure(ex);
            }
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
            if (handler != null) handler(this, new ProgressChangedEventArgs(progressPercentage));
        }
    }
}