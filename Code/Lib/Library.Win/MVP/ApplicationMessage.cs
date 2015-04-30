using System;
using System.Windows.Forms;

namespace Library.Win.MVP
{ /// <summary>
    /// 
    /// </summary>
    public interface IApplicationMessage
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
        bool Confirm(string message);
        /// <summary>
        /// 
        /// </summary>
        void Show(string message);

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
    public sealed class ApplicationMessage : IApplicationMessage
    {
        private readonly ApplicationFacade _facade;

        internal ApplicationMessage(ApplicationFacade facade)
        {
            _facade = facade;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>

        public void Notification(string message)
        {
            Notification(message, 5);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeout"></param>
        public void Notification(string message, int timeout)
        {
            _facade.Notify.ShowBalloonTip(timeout, "", message, ToolTipIcon.Info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Confirm(string message)
        {
            return MessageBox.Show(message, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Show(string message)
        {
            MessageBox.Show(message);
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