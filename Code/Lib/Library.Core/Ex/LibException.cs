using System;
using System.Runtime.Serialization;

namespace Library
{
    public class LibException : AbException
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

        protected LibException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}