using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Library.HelperUtility;

namespace ImageManagement
{
    public partial class SheetForm : Form
    {
        public SheetForm()
        {
            InitializeComponent();
            listBox1.Items.Add("ImageManagement.WaterForm");
            listBox1.Items.Add("ImageManagement.EffectsForm");
            listBox1.Items.Add("ImageManagement.DateForm");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
          var sel= listBox1.SelectedItem;
            if (sel == null) return;
           var typeobj= Type.GetType(sel.ToString());
            if (typeobj == null) return;
           var form= typeobj.CreateInstance<Form>();
            form.ShowDialog();
        }

   

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            var sel = listBox1.SelectedItem;
            if (sel == null) return;
            var typeobj = Type.GetType(sel.ToString());
            if (typeobj == null) return;
            var form = typeobj.CreateInstance<Form>();
            form.ShowDialog();
        }
    }
}
