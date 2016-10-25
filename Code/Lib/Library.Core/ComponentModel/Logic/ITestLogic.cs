using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

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
        public ExceptionEventArgs(Exception error)
        {
            Error = error;
        }
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
        public DateTime Time { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public MessageEventArgs(string message)
        {
            Message = message;
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
        bool IsRunning { get; }
    }
}