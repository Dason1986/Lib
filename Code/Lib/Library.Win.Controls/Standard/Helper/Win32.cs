using System;
using System.Runtime.InteropServices;

namespace Library.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class Win32
    {
        #region Window Const
        /// <summary>
        /// 
        /// </summary>
        public const int WM_KEYDOWN = 0x0100;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_KEYUP = 0x0101;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_CTLCOLOREDIT = 0x133;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_ERASEBKGND = 0x0014;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x0201;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_LBUTTONUP = 0x0202;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_LBUTTONDBLCLK = 0x0203;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_WINDOWPOSCHANGING = 0x46;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_PAINT = 0xF;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_CREATE = 0x0001;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_ACTIVATE = 0x0006;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCCREATE = 0x0081;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCCALCSIZE = 0x0083;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCPAINT = 0x0085;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCACTIVATE = 0x0086;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCLBUTTONUP = 0x00A2;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCMOUSEMOVE = 0x00A0;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NCHITTEST = 0x0084;
        /// <summary>
        /// 
        /// </summary>
        public const int HTLEFT = 10;
        /// <summary>
        /// 
        /// </summary>
        public const int HTRIGHT = 11;
        /// <summary>
        /// 
        /// </summary>
        public const int HTTOP = 12;
        /// <summary>
        /// 
        /// </summary>
        public const int HTTOPLEFT = 13;
        /// <summary>
        /// 
        /// </summary>
        public const int HTTOPRIGHT = 14;
        /// <summary>
        /// 
        /// </summary>
        public const int HTBOTTOM = 15;
        /// <summary>
        /// 
        /// </summary>
        public const int HTBOTTOMLEFT = 0x10;
        /// <summary>
        /// 
        /// </summary>
        public const int HTBOTTOMRIGHT = 17;
        /// <summary>
        /// 
        /// </summary>
        public const int HTCAPTION = 2;
        /// <summary>
        /// 
        /// </summary>
        public const int HTCLIENT = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_FALSE = 0;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_TRUE = 1;



        #endregion

        #region Public extern methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);
        /// <summary>
        /// /
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hRgn"></param>
        /// <param name="bRedraw"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hwnd, int hRgn, Boolean bRedraw);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Ansi)]
        public static extern int DeleteObject(int hObject);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        #endregion
    }
}