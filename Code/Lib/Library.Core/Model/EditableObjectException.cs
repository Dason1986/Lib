using System;
using System.Runtime.Serialization;

namespace Library
{
    [Serializable]
    public class EditableObjectException : LibException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public EditableObjectException()
        {
        }
        protected EditableObjectException(string message, double resultCode)
            : base(message, resultCode)
        {

        }

        protected EditableObjectException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {

        }
        public EditableObjectException(string message)
            : base(message)
        {
        }

        public EditableObjectException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected EditableObjectException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}