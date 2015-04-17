using System;
using System.Runtime.Serialization;

namespace Library.Draw
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ImageException : LibException
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
        public ImageException()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ImageException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ImageException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ImageException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}