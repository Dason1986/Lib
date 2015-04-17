using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Library.Annotations;
using Library.IO;

namespace Library.Draw.Print
{
    /// <summary>
    /// 
    /// </summary>
    public class PrintUntity : IDisposable
    {
        private bool _isDispose;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static PrintUntity Create(IPrintBuilder builder, PrintOption option)
        {
            if (builder == null) throw new ArgumentNullException("builder");
            if (option == null) throw new ArgumentNullException("option");

            var command = new PrintUntity
            {
                Model = builder.Model as IPrintModel,
                Option = option,
                Builder = builder
            };
            command.Initiative();
            return command;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param> 
        /// <returns></returns>
        public static PrintUntity Create(IPrintBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException("builder");
            var command = new PrintUntity
            {
                Model = builder.Model as IPrintModel,
                Option = new PrintOption(),
                Builder = builder
            };
            command.Initiative();
            return command;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static PrintUntity Create([NotNull] IPrintModel model, [NotNull] PrintOption option)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (option == null) throw new ArgumentNullException("option");

            var command = new PrintUntity { Model = model, Option = option };

            command.Initiative();
            return command;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param> 
        /// <returns></returns>
        public static PrintUntity Create([NotNull] IPrintModel model)
        {
            if (model == null) throw new ArgumentNullException("model");
            var command = new PrintUntity { Model = model, Option = new PrintOption() };
            command.Initiative();
            return command;
        }
        /// <summary>
        /// 
        /// </summary>
        protected PrintUntity()
        {

        }
        #region MyRegion

        /// <summary>
        /// 要打印的对象
        /// </summary>
        public IPrintModel Model { get; protected set; }
        /// <summary>
        /// 打印选项
        /// </summary>
        public PrintOption Option { get; protected set; }
        /// <summary>
        /// 打印构造器
        /// </summary>
        protected IPrintBuilder Builder { get; private set; }
        /// <summary>
        /// 打印 文档
        /// </summary>
        protected PrintDocument PrintDocument { get; private set; }
        /// <summary>
        /// 打印预览对话框
        /// </summary>
        protected PrintPreviewDialog PrintPreviewDialog { get; private set; }
        /// <summary>
        /// 打印对话框
        /// </summary>
        protected PrintDialog PrintDialog { get; private set; }
        /// <summary>
        /// 开始打印事件（当前页）
        /// </summary>
        public event PrintEventHandler BeginPrint
        {
            add { PrintDocument.BeginPrint += value; }
            remove { PrintDocument.BeginPrint -= value; }
        }
        /// <summary>
        /// 由Builder生成的打印内容
        /// </summary>
        protected Image SourceImage { get; set; }
        /// <summary>
        /// 结束打印事件（当前页）
        /// </summary>
        public event PrintEventHandler EndPrint
        {
            add { PrintDocument.EndPrint += value; }
            remove { PrintDocument.EndPrint -= value; }
        }
        /// <summary>
        /// 要打印背景图
        /// </summary>
        protected bool HasBackgroundImage { get; private set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IPrintBuilder FactoryBuilder()
        {

            var builder = PrintBuilderHelper.FactoryBuilder(Model);
            if (builder == null) throw new PrintException("创建Builder为空", 14001.021);

            return builder;

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Initiative()
        {
            if (Builder == null)
            {
                Builder = FactoryBuilder();
                Builder.Model = Model;
            }
            Builder.Validate();
            PrintDocument = new PrintDocument();
            PrintDialog = new PrintDialog();
            PrintPreviewDialog = new PrintPreviewDialog();
            InitPrintDocumen();
            InitPrintPreviewDialog();
            InitPrintDialog();

            Builder.PageRectangle = PrintDocument.DefaultPageSettings.Bounds;

        }


        /// <summary>
        /// 
        /// </summary>
        public void Print()
        {
            if (Option.IsPreview)
            {
                OnPreview();
            }
            else
            {
                OnPrint();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnPrint()
        {

            PrintDialog.Document = PrintDocument;
            PrintDialog.AllowSomePages = Builder.HasMorePages;
            PrintDialog.AllowPrintToFile = false;
            PrintDialog.PrinterSettings.FromPage = 1;
            PrintDialog.PrinterSettings.ToPage = (int)Builder.TotalPages;
            DialogResult userResPonse = PrintDialog.ShowDialog();
            if (userResPonse != DialogResult.OK) return;
            PrintDocument.Print();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnPreview()
        {
            PrintPreviewDialog.PrintPreviewControl.Document = PrintDocument;
            PrintPreviewDialog.ShowDialog();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            MultiplePages(e);

        }

        private void MultiplePages(PrintPageEventArgs e)
        {
            if (e.PageBounds != Builder.PageRectangle) Builder.PageRectangle = e.PageBounds;

            if (HasBackgroundImage) e.Graphics.DrawImage(Builder.PreviewBackgroundImage, Point.Empty);
            var image = Builder.CreateNextBitmap();
            e.Graphics.DrawImage(image, Option.MovePoint);
            e.HasMorePages = Builder.CanNextPange();
        }




        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitPrintDialog()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitPrintPreviewDialog()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitPrintDocumen()
        {
            PrintDocument.PrintPage += PrintDocument_PrintPage;
            PrintDocument.EndPrint += PrintDocument_EndPrint;
            PrintDocument.BeginPrint += PrintDocument_BeginPrint;
        }


        void PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            HasBackgroundImage = Builder.PreviewBackgroundImage != null && PrintDocument.PrintController.IsPreview;
            if (PrintDialog.PrinterSettings.PrintRange == PrintRange.SomePages)
                Builder.SetPageRange(PrintDialog.PrinterSettings.FromPage, PrintDialog.PrinterSettings.ToPage);
            else
                Builder.ResetIndex();
        }

        void PrintDocument_EndPrint(object sender, PrintEventArgs e)
        {

        }




        private void Dispose(bool isDispose)
        {
            if (!isDispose)
                return;
            if (_isDispose)
                return;
            _isDispose = true;
            PrintDocument.PrintPage -= PrintDocument_PrintPage;
            PrintDocument.BeginPrint -= PrintDocument_BeginPrint;
            PrintDocument.EndPrint -= PrintDocument_EndPrint;
            PrintDocument = null;
            PrintDialog = null;
            PrintPreviewDialog = null;
            System.GC.SuppressFinalize(this);

        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}