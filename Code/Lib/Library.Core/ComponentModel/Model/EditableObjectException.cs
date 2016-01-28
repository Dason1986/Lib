using System;
using System.Runtime.Serialization;

namespace Library.ComponentModel.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class EditableObjectException : LibException
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
        public EditableObjectException()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        protected EditableObjectException(string message, double resultCode)
            : base(message, resultCode)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <param name="inner"></param>
        protected EditableObjectException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public EditableObjectException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public EditableObjectException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected EditableObjectException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}