using System;

namespace Library.Win.MVP
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationFacade
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Notification(string message);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool MessageConfirm(string message);
        /// <summary>
        /// 
        /// </summary>
        void Message(string message);

        /// <summary>
        /// 
        /// </summary>
        void ShowSplash();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        void SetSplashText(string text);

        /// <summary>
        /// 
        /// </summary>
        void HideSplash();
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ApplicationFacade : IApplicationFacade
    {
        private ApplicationFacade()
        {

        }

        private static readonly ApplicationFacade _instance = new ApplicationFacade();
        /// <summary>
        /// 
        /// </summary>
        public static ApplicationFacade Instance { get { return _instance; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Notification(string message)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool MessageConfirm(string message)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Message(string message)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ShowSplash()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void SetSplashText(string text)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public void HideSplash()
        {
            throw new NotImplementedException();
        }
    }
}