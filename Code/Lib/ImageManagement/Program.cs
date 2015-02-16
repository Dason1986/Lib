using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using Library;
using Library.HelperUtility;
using Microsoft.VisualStudio.DebuggerVisualizers;


namespace TestWinform
{

    static class Program
    {

        internal static byte[] Original { get; private set; }
        internal static Bitmap Gbitmap { get; private set; }
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {

            Properties.Resources.Culture = new CultureInfo("en-us");
            Properties.Resources.ResourceManager.IgnoreCase = true;
            var obj = Properties.Resources.Test;
          Console.WriteLine(obj);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var file = typeof(SheetForm).Assembly.GetManifestResourceStream("TestWinform.original.jpg");
            if (file != null)
            {

                Original = file.ToArray();
            }
            Gbitmap = new Bitmap(new MemoryStream(Original));

            Application.Run(new SheetForm());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null) MessageBox.Show(ex.Message);
        }
    }
}
