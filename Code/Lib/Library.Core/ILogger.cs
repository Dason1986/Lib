using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// This generic form is intended to be used by wrappers.
        /// </summary>
        void Log(LogCategory level, object message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool IsEnabledFor(LogCategory level);
    }

    /// <summary>
    /// 
    /// </summary>
    public enum LogCategory
    {
        /// <summary>
        /// 
        /// </summary>
        Fatal,
        /// <summary>
        /// 
        /// </summary>
        Error,
        /// <summary>
        /// 
        /// </summary>
        Warn,
        /// <summary>
        /// 
        /// </summary>
        Debug,
        /// <summary>
        /// 
        /// </summary>
        Info,
        
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ILoggerWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        ILogger Logger
        {
            get;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ILog : ILoggerWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Debug(object message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Debug(object message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Info(object message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Info(object message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Warn(object message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Warn(object message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Error(object message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Error(object message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Fatal(object message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Fatal(object message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void FatalFormat(string format, params object[] args);


        /// <summary>
        /// 
        /// </summary>
        bool IsDebugEnabled
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsInfoEnabled
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsErrorEnabled
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsWarnEnabled
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsFatalEnabled
        {
            get;
        }
    }
}
