using System;
using System.Runtime.Serialization;
using Library.Att;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CodeException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public double ResultCode { get; protected set; }
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        /// <summary>
        /// 
        /// </summary>
        public CodeException()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        protected CodeException(string message)
            : base(message)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        public CodeException(double resultCode)
            : base(LanguageResourceManagement.GetException(resultCode))
        {
            ResultCode = resultCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        protected CodeException(string message, double resultCode)
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
        protected CodeException(string message, double resultCode, Exception inner)
            : base(message, inner)
        {
            ResultCode = resultCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        public CodeException(double resultCode, object[] formatages)
            : base(LanguageResourceManagement.GetException(resultCode, formatages))
        {
            ResultCode = resultCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="resourceName"></param>
        public CodeException(double resultCode, string resourceName)
            : base(LanguageResourceManagement.GetException( resultCode, resourceName))
        {
            ResultCode = resultCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        /// <param name="resourceName"></param>
        public CodeException(double resultCode, object[] formatages, string resourceName)
            : base(LanguageResourceManagement.GetException(resultCode, formatages, resourceName))
        {
            ResultCode = resultCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public CodeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CodeException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}