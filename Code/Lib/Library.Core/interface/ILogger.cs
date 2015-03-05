using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{


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
    public interface ILog
    {

        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        string Name { get; }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void Write(string format, params object[] args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Write(object message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Write(object message, Exception exception);




        /// <summary>
        /// 
        /// </summary>
        LogCategory Category
        {
            get;
        }


    }

    /// <summary>
    /// Used to manage logging.
    /// </summary>
    public static class LogManager
    {
        static readonly ILog NullLogInstance = new ConsoleLog(LogCategory.Info);

        /// <summary>
        /// Creates an <see cref="ILog"/> for the provided type.
        /// </summary>
        public static Func<Type, ILog> GetLog = type => NullLogInstance;
        /// <summary>
        /// 
        /// </summary>
        public static ILog NullLog { get { return NullLogInstance; } }
        class ConsoleLog : ILog
        {
            public ConsoleLog(LogCategory category)
            {
                this.Category = category;
            }

            public string Name
            {
                get { return "Console Log"; }
            }

            public void Write(object message)
            {
                Console.WriteLine(message);
            }

            public void Write(string format, params object[] args)
            {
                Console.WriteLine(format, args);
            }




            public void Write(object message, Exception exception)
            {
                Console.WriteLine(message);
                Console.WriteLine(exception.ToString());
            }

            public LogCategory Category { get; protected set; }
        }
    }
}
