using System;
using System.Runtime.Serialization;

namespace Library.IDCrad
{
    /// <summary>
    ///
    /// </summary>
    public class IDCardException : LibException
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
        public IDCardException()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        public IDCardException(string message, double resultCode)
            : base(message, resultCode)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <param name="inner"></param>
        public IDCardException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public IDCardException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public IDCardException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IDCardException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}