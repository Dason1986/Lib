using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Library.Draw.Print;

namespace Library.Win
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PrintComponent : Component
    {
        private object _pintObj;
        private IPrintBuilder _pintbuilder;
        private PrintDialog _dialog;
        private PrintDocument _document;
        /// <summary>
        /// 
        /// </summary>
        public PrintComponent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public PrintComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// 打印對象
        /// </summary>
        [DefaultValue(null), DisplayName("打印對象"), Category("打印相關")]
        public object PintObj
        {
            get { return _pintObj; }
            set
            {
                if (object.ReferenceEquals(_pintObj, value)) return;
                _pintbuilder = PrintBuilderHelper.FactoryBuilder(value);
                _pintObj = value;
            }
        }
        /// <summary>
        /// 打印設置
        /// </summary>
        [DefaultValue(null), DisplayName("打印設置"), Category("打印相關")]
        public PrintOption Option { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public void ShowPrintDialog()
        {
            _pintbuilder.Validate();
            CreateDocument();
            _dialog = new PrintDialog();
            _dialog.Document = _document;
            if (_dialog.ShowDialog() == DialogResult.OK)
            {
                _document.Print();
            }

        }

        private void CreateDocument()
        {
            _document = new PrintDocument();
            _document.BeginPrint += Document_BeginPrint;
            _document.EndPrint += Document_EndPrint;
            _document.PrintPage += document_PrintPage;
            _document.QueryPageSettings += document_QueryPageSettings;

        }

        void document_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            //  throw new NotImplementedException();
        }


        void document_PrintPage(object sender, PrintPageEventArgs e)
        {

            var image = _pintbuilder.CreateCurrentBitmap();
            e.Graphics.DrawImage(image, new Point(0, 0));
            //throw new NotImplementedException();
        }

        void Document_EndPrint(object sender, PrintEventArgs e)
        {
            //    throw new NotImplementedException();
        }

        void Document_BeginPrint(object sender, PrintEventArgs e)
        {
            //     throw new NotImplementedException();
        }
        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                if (_document != null)
                {
                    _document.BeginPrint -= Document_BeginPrint;
                    _document.EndPrint -= Document_EndPrint;
                    _document.PrintPage -= document_PrintPage;
                    _document.QueryPageSettings -= document_QueryPageSettings;
                    _document.Dispose();
                }
                if (_dialog != null)
                {
                    _dialog.Dispose();
                }
                components.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
