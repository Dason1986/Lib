using System;
using System.Runtime.Serialization;

namespace Library.ComponentModel.IO.MDS
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class DocumentManagementException : CodeException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public DocumentManagementException(string message) : base(message, 0)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <param name="args"></param>
        public DocumentManagementException(double code, params object[] args)
            : base(code, args)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <param name="inner"></param>
        public DocumentManagementException(double code, Exception inner)
            : base(code, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mesage"></param>
        /// <param name="code"></param>
        /// <param name="args"></param>
        public DocumentManagementException(string mesage, double code, params object[] args)
            : base(mesage, code, args)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mesage"></param>
        /// <param name="code"></param>
        /// <param name="inner"></param>
        public DocumentManagementException(string mesage, double code, Exception inner)
            : base(mesage, code, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DocumentManagementException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}