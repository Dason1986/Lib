﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Library.Test
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
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        internal MessageEventArgs(string message, MessageType messageType = MessageType.Info)
        {
            Message = message;
            MessageInfo = messageType;
        }
    }
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
    public interface ITestLogic
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
        void Start();
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class TestLogic : ITestLogic
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
        protected virtual void OnFailure(Exception message)
        {
            ExceptionEventHandler handler = Failure;
            if (handler != null) handler(this, new ExceptionEventArgs(message));
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
            if (handler != null) handler(this, new MessageEventArgs(message, messageType));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Stopwatch watch=new Stopwatch();
            watch.Start();
            try
            {
                OnStart();
            }
            catch (Exception ex)
            {
                OnFailure(ex);
            }
            watch.Stop();
            OnCompleted(watch.Elapsed);
        }
        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnStart();
    }
}