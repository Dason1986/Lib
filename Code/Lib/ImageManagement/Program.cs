using Library.Diagnostics;
using Library.HelperUtility;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TestWinform
{
    internal static class Program
    {
        internal static byte[] Original { get; private set; }
        internal static Bitmap Gbitmap { get; private set; }

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        private static void Main(string[] Args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        //    UDPTraceListener listener = new UDPTraceListener(9090);

            var file = typeof(SheetForm).Assembly.GetManifestResourceStream("TestWinform.original.jpg");
            if (file != null)
            {
                Original = file.ToArray();
            }
            Gbitmap = new Bitmap(new MemoryStream(Original));
            // ImageEffectsVisualizer.TestShowVisualizer(Gbitmap);
            Application.Run(new SheetForm());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null) MessageBox.Show(ex.Message);
        }
    }
}