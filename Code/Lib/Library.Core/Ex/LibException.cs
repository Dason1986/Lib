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
        public LibException()
        {
        }

        protected LibException(string message, double resultCode)
            : base(message, resultCode)
        {

        }

        protected LibException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {

        }

        public LibException(string message)
            : base(message)
        {
        }

        public LibException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public LibException(double resultCode, object[] formatages)
            : base(resultCode, formatages)
        {
            ResultCode = resultCode;
        }
        public LibException(double resultCode, string resourceName)
            : base(resultCode, resourceName)
        {
            ResultCode = resultCode;
        }

        public LibException(double resultCode, object[] formatages, string resourceName)
            : base(resultCode, formatages, resourceName)
        {
            ResultCode = resultCode;
        }
        protected LibException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }


    }
}