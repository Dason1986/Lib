using Library;
using Library.HelperUtility;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace TestWinform
{
    public partial class SheetForm : Form
    {
        public SheetForm()
        {
            if (sheet != null) throw new Exception("sheet exist");
            InitializeComponent();
            listBox1.Items.Add("TestWinform.WaterForm");
            listBox1.Items.Add("TestWinform.EffectsForm");
            listBox1.Items.Add("TestWinform.DateForm");
            listBox1.Items.Add("TestWinform.IDCardForm");
            sheet = this;
        }

        private static SheetForm sheet;

        public static void SetObject(object obj)
        {
            if (sheet != null) sheet.propertyGrid1.SelectedObject = obj;
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
            button3.Text = ResourceManagement.GetString(typeof(GlobalResource), "English");
            button2.Text = ResourceManagement.GetString(typeof(GlobalResource), "Chinese");
            button4.Text = ResourceManagement.GetString(typeof(GlobalResource), "Portuguese");
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

        private void BtnClear_Click(object sender, EventArgs e)
        {
            RtxInfo.Clear();
        }

        private void SheetForm_Load(object sender, EventArgs e)
        {
       
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".rtf";
            saveFileDialog.Filter = "Rich Text Format(RTF)(*.rtf)|*.rtf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var file = saveFileDialog.OpenFile();
                RtxInfo.SaveFile(file, RichTextBoxStreamType.TextTextOleObjs);
                file.Close();
                file.Dispose();
            }
        }
    }
}