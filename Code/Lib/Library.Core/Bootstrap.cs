using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class BootstrapException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public BootstrapException() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public BootstrapException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public BootstrapException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BootstrapException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class Bootstrap
    {
        static Bootstrap()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        protected Bootstrap()
        {
            if (Currnet != null) throw new BootstrapException();
            Currnet = this;
        }
        /// <summary>
        /// 
        /// </summary>
        public static Bootstrap Currnet { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {

            Register();

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Register()
        {



        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T GetService<T>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T GetService<T>(string name);
    }
}
