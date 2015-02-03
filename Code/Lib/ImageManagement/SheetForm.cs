using System;
using System.Windows.Forms;
using Library.HelperUtility;
using Library.Win.Controls;

namespace TestWinform
{
    public partial class SheetForm : Form
    {
        public SheetForm()
        {
            InitializeComponent(); 
            listBox1.Items.Add("TestWinform.WaterForm");
            listBox1.Items.Add("TestWinform.EffectsForm");
            listBox1.Items.Add("TestWinform.DateForm");
            listBox1.Items.Add("TestWinform.IDCardForm");
            
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
