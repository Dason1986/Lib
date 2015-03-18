using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Library.Att;
using Library.HelperUtility;
using Library.Controls;

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

    
   

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            var sel = listBox1.SelectedItem;
            if (sel == null) return;
            var typeobj = Type.GetType(sel.ToString());
            if (typeobj == null) return;
            var form = typeobj.CreateInstance<Form>();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-us");
            Change();
        }

        private void Change()
        {
            button3.Text = LanguageResourceManagement.GetString("English", "Global");
            button2.Text = LanguageResourceManagement.GetString("Chinese", "Global");
            button4.Text = LanguageResourceManagement.GetString("Portuguese", "Global");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-cn");
            Change();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("pt-pt");
            Change();
        }
    }
}
