using System;
using System.Runtime.Serialization;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class AbException : Exception
    {


        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        /// <summary>
        /// 
        /// </summary>
        public double ResultCode { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        protected AbException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        protected AbException(string message, double resultCode)
            : base(message)
        {
            ResultCode = resultCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <param name="inner"></param>
        protected AbException(string message, double resultCode, Exception inner)
            : base(message, inner)
        {
            ResultCode = resultCode;
        }

        protected AbException(string message)
            : base(message)
        {
        }

        protected AbException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected AbException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}