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
    public partial class PrintComponent : Component
    {
        private object _pintObj;
        private IPrintBuilder _pintbuilder;
        private PrintDialog _dialog;
        public PrintComponent()
        {
            InitializeComponent();
        }

        public PrintComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [DefaultValue(null)]
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
