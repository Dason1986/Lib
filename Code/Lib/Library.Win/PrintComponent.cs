using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            _dialog = new PrintDialog();
            _dialog.Document = new PrintDocument();

            var image = _pintbuilder.CreateCurrentBitmap();

            _dialog.ShowDialog();
        }
    }
}
