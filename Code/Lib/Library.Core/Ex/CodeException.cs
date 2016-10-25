using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Library
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class CodeException : Exception
    {
        /// <summary>
        /// 获取描述当前异常的消息。
        /// </summary>
        /// <returns>
        /// 解释异常原因的错误消息或空字符串 ("")。
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override string Message
        {
            get
            {
                if (Args != null)
                {
                    return string.Format(base.Message, Args);
                }
                return base.Message;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public double ResultCode { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public object[] Args { get; protected set; }

        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        /// <summary>
        ///
        /// </summary>
        public CodeException()

        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <param name="inner"></param>
        public CodeException(double code, Exception inner)
            : base(code.ToString(CultureInfo.InvariantCulture), inner)
        {
            ResultCode = code;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        protected CodeException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resultCode"></param>
        public CodeException(double resultCode)
            : base(resultCode.ToString())
        {
            ResultCode = resultCode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        protected CodeException(string message, double resultCode)
            : base(message)
        {
            ResultCode = resultCode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <param name="inner"></param>
        protected CodeException(string message, double resultCode, Exception inner)
            : base(message, inner)
        {
            ResultCode = resultCode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mesage"></param>
        /// <param name="code"></param>
        /// <param name="args"></param>
        public CodeException(string mesage, double code, params object[] args)
            : base(mesage)
        {
            ResultCode = code;
            Args = args;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="args"></param>
        public CodeException(double resultCode, object[] args)

        {
            ResultCode = resultCode;
            Args = args;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public CodeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CodeException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}