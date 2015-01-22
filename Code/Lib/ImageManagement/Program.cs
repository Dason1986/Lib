using System;
using System.Reflection;
using System.Windows.Forms;
using Library.HelperUtility;

namespace TestWinform
{
    static class Program
    {

        internal static byte[] Original { get; private set; }
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var file = Assembly.GetEntryAssembly().GetManifestResourceStream("TestWinform.original.jpg");
            if (file != null)
            {

                Original = file.ToArray();
            }
            Application.Run(new SheetForm());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null) MessageBox.Show(ex.Message);
        }
    }
}
