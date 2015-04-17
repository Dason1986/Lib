using System;
using Library.Annotations;

namespace Library.HelperUtility
{ 
    /// <summary>
    /// 
    /// </summary>
    public interface IErrorMessageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        Type ErrorType { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        string GetMessage(Exception exception);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool CanExcute(Exception exception);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ErrorMessageBuilder : IErrorMessageBuilder
    {
        private readonly Func<Exception, string> _fun;
        /// <summary>
        /// 
        /// </summary>
        public Type ErrorType { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="fun"></param>
        public ErrorMessageBuilder([NotNull] Type errorType, [NotNull] Func<Exception, string> fun)
        {
            if (errorType == null) throw new ArgumentNullException("errorType");
            this.ErrorType = errorType;
            if (fun == null) throw new ArgumentNullException("fun");
            _fun = fun;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public string GetMessage(Exception exception)
        {
            return _fun(exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public bool CanExcute(Exception exception)
        {
            return ErrorType.IsInstanceOfType(exception);
        }
    }
}