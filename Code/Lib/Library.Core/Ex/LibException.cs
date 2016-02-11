using System;
using System.Runtime.Serialization;

namespace Library
{
    /// <summary>
    ///
    /// </summary>
    public class LibException : CodeException
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
        public LibException(double resultCode) : base(resultCode)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public LibException()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        protected LibException(string message, double resultCode)
            : base(message, resultCode)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <param name="inner"></param>
        protected LibException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public LibException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public LibException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        public LibException(double resultCode, object[] formatages)
            : base(resultCode, formatages)
        {
            ResultCode = resultCode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected LibException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}