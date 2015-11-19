using System;
using System.Drawing;
using System.Windows.Forms;
using Library.Annotations;

namespace Library.Win.MVP
{
    /// <summary>
    /// 
    /// </summary>
    public   class ApplicationFacade
    {
        private ApplicationFacade()
        {
            Message = new ApplicationMessage(this);

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
        /// Ωÿ»°∆¡ƒªÉ»»›
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
        public IApplicationMessage Message { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public bool StartOnScreen2 { get; set; }

    }
}