using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Win.MVP
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPresenter
    {
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IView
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public interface IModel
    {

    }
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
        void ShowLoading();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        void SetLoadingText(string text);

        /// <summary>
        /// 
        /// </summary>
        void HideLoading();
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ApplicationFacade
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
        public void Notification(string message)
        {
        }
    }

}
