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






    }

    /// <summary>
    /// Used to manage logging.
    /// </summary>
    public static class LogManager
    {
        static readonly ILogger NullLogInstance = new ConsoleLog();

        /// <summary>
        /// Creates an <see cref="ILogger"/> for the provided type.
        /// </summary>
        static readonly IDictionary<string, ILogger> loggerdic = new Dictionary<string, ILogger>();
        /// <summary>
        /// 
        /// </summary>
        public static ILogger Logger
        {
            get
            {
                return NullLogInstance;
            }
        }
        class ConsoleLog : ILogger
        {


            public string Name
            {
                get { return "Console Log"; }
            }

            public void Write(object message)
            {
                try
                {
                    Console.WriteLine(message);
                    foreach (ILogger item in loggerdic.Values)
                    {
                        item.Write(message);
                    }
                }
                catch (Exception )
                {

                   
                }

                
            }

            public void Write(string format, params object[] args)
            {
                
                try
                {
                    Console.WriteLine(format, args);
                    foreach (ILogger item in loggerdic.Values)
                    {
                     
                        item.Write(format, args);
                    }
                }
                catch (Exception)
                {


                }
            }




            public void Write(object message, Exception exception)
            {
              
                try
                {
                    Console.WriteLine(message);
                    Console.WriteLine(exception.ToString());
                    foreach (ILogger item in loggerdic.Values)
                    {
                        item.Write(message, exception);
                    }
                }
                catch (Exception)
                {


                }
            }


        }
    }
}
