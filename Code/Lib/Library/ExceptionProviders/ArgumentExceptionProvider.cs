using System;

namespace Library.ExceptionProviders
{

    /// <summary>
    /// 
    /// </summary>
    public class ArgumentExceptionProvider : CustomExceptionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Exception ProvideFault(Exception error, ref string message)
        {
            if (error is ArgumentException == false) return null;
            var argumentNull = (ArgumentException)error;

            message = string.Format("{0}:{1}", argumentNull.Message, argumentNull.ParamName);
            return new Exception(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public override bool HandleError(Exception error)
        {
            return error is ArgumentException;
        }
    }
}