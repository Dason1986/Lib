using System;
using System.Runtime.Serialization;

namespace Library
{
    /// <summary>
    ///
    /// </summary>
    public class LogicException : CodeException
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
        public LogicException()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        protected LogicException(string message, double resultCode)
            : base(message, resultCode)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <param name="inner"></param>
        protected LogicException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public LogicException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public LogicException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        public LogicException(double resultCode, object[] formatages)
            : base(resultCode, formatages)
        {
            ResultCode = resultCode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected LogicException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}