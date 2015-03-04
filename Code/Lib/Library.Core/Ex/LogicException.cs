using System;
using System.Runtime.Serialization;

namespace Library
{
    public class LogicException : CodeException
    {


        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        public LogicException()
        {
        }

        protected LogicException(string message, double resultCode)
            : base(message, resultCode)
        {

        }

        protected LogicException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {

        }

        public LogicException(string message)
            : base(message)
        {
        }

        public LogicException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public LogicException(double resultCode, object[] formatages)
            : base(resultCode, formatages)
        {
            ResultCode = resultCode;
        }
        public LogicException(double resultCode, string resourceName)
            : base(resultCode, resourceName)
        {
            ResultCode = resultCode;
        }

        public LogicException(double resultCode, object[] formatages, string resourceName)
            : base(resultCode, formatages, resourceName)
        {
            ResultCode = resultCode;
        }
        protected LogicException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}