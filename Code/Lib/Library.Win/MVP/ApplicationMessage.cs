using Library.Annotations;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Library.Win.MVP
{
    /// <summary>
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
        /// <param name="message"></param>
        /// <returns></returns>
        void SetStatusBarText(string message);

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
        private NotifyManagement _notify;

        internal ApplicationMessage(ApplicationFacade facade)
        {
            _facade = facade;
            _notify = new NotifyManagement();
            _notify.CreateNotify();
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
            Notification(string.Empty, message, timeout);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="timeout"></param>
        public void Notification(string title, string message, int timeout)
        {
            _notify.ShowTip(timeout, title, message);
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public void SetStatusBarText(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        class NotifyManagement
        {
            internal NotifyIcon Notify { get; private set; }

            internal NotifyIcon CreateNotify()
            {
                Notify = new NotifyIcon
                {
                    ContextMenu = new ContextMenu(new MenuItem[] { new MenuItem("Exit", (x, y) => { Application.Exit(); }), }),
                };
                Notify.Visible = true;
                Application.ThreadExit += (x, y) =>
                {
                    if (Notify != null) Notify.Visible = false;
                };
                return Notify;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="iconPath"></param>
            public void SetIcon([NotNull] string iconPath)
            {
                if (iconPath == null) throw new ArgumentNullException("iconPath");
                Notify.Icon = new Icon(iconPath);
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="menu"></param>
            public void AddMenuItem(MenuItem menu)
            {
                Notify.ContextMenu.MenuItems.Add(menu);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="timeout"></param>
            /// <param name="title"></param>
            /// <param name="message"></param>
            public void ShowTip(int timeout, string title, string message)
            {
                Notify.ShowBalloonTip(timeout, title, message, ToolTipIcon.Info);
            }
        }
    }
}