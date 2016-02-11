using Library.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Library.Win.MVP
{
    /// <summary>
    ///
    /// </summary>
    public class PresenterContext
    {
        /// <summary>
        ///
        /// </summary>
        public virtual IPresenter Presenter { get; set; }

        /// <summary>
        ///
        /// </summary>
        public ViewContext ParentActionViewContext { get; }
    }

    /// <summary>
    ///
    /// </summary>
    public class ViewContext : PresenterContext
    {
        /// <summary>
        ///
        /// </summary>
        public virtual IView View { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IViewEngine
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="presenterContext"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        ViewEngineResult FindView(PresenterContext presenterContext, string viewName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="presenterContext"></param>
        /// <param name="view"></param>
        void ReleaseView(PresenterContext presenterContext, IView view);
    }

    /// <summary>
    ///
    /// </summary>
    public class ViewEngineResult
    {
        /// <summary>
        /// 使用指定的搜索位置来初始化 System.Web.Mvc.ViewEngineResult 类的新实例。
        /// </summary>
        /// <param name="searchedLocations">搜索的位置。</param>
        /// <exception cref="System.ArgumentNullException"> searchedLocations 参数为 null。
        /// </exception>
        public ViewEngineResult(IEnumerable<string> searchedLocations)
        {
        }

        /// <summary>
        ///  使用指定的视图和视图引擎来初始化 System.Web.Mvc.ViewEngineResult 类的新实例。
        /// </summary>
        /// <param name="view">  视图。</param>
        /// <param name="viewEngine"> 视图引擎。</param>
        /// <exception cref="System.ArgumentNullException">
        ///   view  或 viewEngine 参数为 null。
        /// </exception>
        public ViewEngineResult(IView view, IViewEngine viewEngine)
        {
        }

        /// <summary>
        ///  获取或设置搜索的位置。
        /// </summary>
        public IEnumerable<string> SearchedLocations { get; private set; }

        /// <summary>
        /// 获取或设置视图。
        /// </summary>
        public IView View { get; private set; }

        /// <summary>
        ///  获取或设置视图引擎。
        /// </summary>
        public IViewEngine ViewEngine { get; private set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class ApplicationFacade
    {
        private ApplicationFacade()
        {
            Message = new ApplicationMessage(this);
            Writer = Console.Out;

            Notify = CreateNotify();
            // UnhandledException();
            Application.ThreadExit += (x, y) =>
            {
                if (Notify != null) Notify.Visible = false;
            };
            Notify.Visible = true;
        }

        internal NotifyIcon Notify { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="iconPath"></param>
        public void SetNotifyIcon([NotNull] string iconPath)
        {
            if (iconPath == null) throw new ArgumentNullException("iconPath");
            Notify.Icon = new Icon(iconPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="menu"></param>
        public void AddNotifyMenuItem(MenuItem menu)
        {
            Notify.ContextMenu.MenuItems.Add(menu);
        }

        private NotifyIcon CreateNotify()
        {
            Notify = new NotifyIcon
            {
                ContextMenu = new ContextMenu(new MenuItem[] { new MenuItem("Exit", (x, y) => { Application.Exit(); }), }),
            };
            return Notify;
        }

        /// <summary>
        /// 截取屏幕內容
        /// </summary>
        /// <returns></returns>
        public Image GetScreenImage()
        {
            int Width = Screen.PrimaryScreen.Bounds.Width, Height = Screen.PrimaryScreen.Bounds.Height;
            int px = 0, py = 0;
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Bounds.X != 0) Width += screen.Bounds.Width;
                if (screen.Bounds.Y != 0) Height += screen.Bounds.Height;
                if (px > screen.Bounds.X) px = screen.Bounds.X;
                if (py > screen.Bounds.Y) py = screen.Bounds.Y;
            }
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(new Point(px, py), Point.Empty, new Size(Width, Height));
            return bitmap;
        }

        private static readonly ApplicationFacade _instance = new ApplicationFacade();

        /// <summary>
        ///
        /// </summary>
        public static ApplicationFacade Instance { get { return _instance; } }

        /// <summary>
        ///
        /// </summary>
        public IApplicationMessage Message { get; protected internal set; }

        /// <summary>
        ///
        /// </summary>
        public virtual TextWriter Writer { get; set; }

        ///// <summary>
        /////
        ///// </summary>
        //public bool StartOnScreen2 { get; set; }
    }
}