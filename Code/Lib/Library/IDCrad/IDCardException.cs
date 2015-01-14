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
        public IDCardException()
        {
        }

        protected IDCardException(string message, double resultCode)
            : base(message, resultCode)
        {

        }

        protected IDCardException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {

        }

        public IDCardException(string message)
            : base(message)
        {
        }

        public IDCardException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected IDCardException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}