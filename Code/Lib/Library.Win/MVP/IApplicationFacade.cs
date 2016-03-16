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
        /// ʹ��ָ��������λ������ʼ�� System.Web.Mvc.ViewEngineResult �����ʵ����
        /// </summary>
        /// <param name="searchedLocations">������λ�á�</param>
        /// <exception cref="System.ArgumentNullException"> searchedLocations ����Ϊ null��
        /// </exception>
        public ViewEngineResult(IEnumerable<string> searchedLocations)
        {
        }

        /// <summary>
        ///  ʹ��ָ������ͼ����ͼ��������ʼ�� System.Web.Mvc.ViewEngineResult �����ʵ����
        /// </summary>
        /// <param name="view">  ��ͼ��</param>
        /// <param name="viewEngine"> ��ͼ���档</param>
        /// <exception cref="System.ArgumentNullException">
        ///   view  �� viewEngine ����Ϊ null��
        /// </exception>
        public ViewEngineResult(IView view, IViewEngine viewEngine)
        {
        }

        /// <summary>
        ///  ��ȡ������������λ�á�
        /// </summary>
        public IEnumerable<string> SearchedLocations { get; private set; }

        /// <summary>
        /// ��ȡ��������ͼ��
        /// </summary>
        public IView View { get; private set; }

        /// <summary>
        ///  ��ȡ��������ͼ���档
        /// </summary>
        public IViewEngine ViewEngine { get; private set; }
    }
  
    /// <summary>
    ///
    /// </summary>
    public class ApplicationFacade : IApplicationFacade
    {
        private ApplicationFacade()
        {
            Message = new ApplicationMessage(this);
            Writer = Console.Out;
         
            // UnhandledException();


        }
    




        /// <summary>
        /// ��ȡ��Ļ����
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
        public ApplicationMessage Message { get; protected internal set; }

        IApplicationMessage IApplicationFacade.Message { get { return Message; } }
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