using System;
using System.Runtime.Serialization;

namespace Library
{
    public class LogicException : AbException
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

        protected LogicException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}