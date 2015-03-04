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
        public double ResultCode { get; protected set; }
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        public CodeException()
        {

        }
        protected CodeException(string message)
            : base(message)
        {

        }
        public CodeException(double resultCode)
            : base(LanguageResourceManagement.GetException(resultCode))
        {
            ResultCode = resultCode;
        }
        protected CodeException(string message, double resultCode)
            : base(message)
        {
            ResultCode = resultCode;
        }
        protected CodeException(string message, double resultCode, Exception inner)
            : base(message, inner)
        {
            ResultCode = resultCode;
        }

        public CodeException(double resultCode, object[] formatages)
            : base(LanguageResourceManagement.GetException(resultCode, formatages))
        {
            ResultCode = resultCode;
        }
        public CodeException(double resultCode, string resourceName)
            : base(LanguageResourceManagement.GetException(string.Empty, resultCode, resourceName))
        {
            ResultCode = resultCode;
        }

        public CodeException(double resultCode, object[] formatages, string resourceName)
            : base(LanguageResourceManagement.GetException(resultCode, formatages, resourceName))
        {
            ResultCode = resultCode;
        }
        public CodeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CodeException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}