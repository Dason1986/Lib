using System;
using System.Runtime.Serialization;

namespace Library.Draw.Print
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PrintException : LibException
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
        public PrintException()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public PrintException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public PrintException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stateCode"></param>
        public PrintException(string message, double stateCode)
            : base(message, stateCode)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stateCode"></param>
        /// <param name="inner"></param>
        public PrintException(string message, double stateCode, Exception inner)
            : base(message, stateCode, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PrintException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}