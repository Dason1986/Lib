﻿using System;
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
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract object GetService(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="argtypes"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract object GetService(Type type, Type[] argtypes, object[] obj);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constantNames"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract object GetService(Type type, string[] constantNames, object[] obj);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T GetService<T>(string name);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract T GetService<T>(Type[] type, object[] obj);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constantNames"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract T GetService<T>(string[] constantNames, object[] obj);

    }
}

