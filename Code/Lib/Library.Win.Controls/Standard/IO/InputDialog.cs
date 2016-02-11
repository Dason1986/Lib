using System.Drawing;
using System.Windows.Forms;

namespace Library.Win
{
    /// <summary>
    /// 输入对话
    /// </summary>
    public static class InputDialog
    {
        /// <summary>
        /// 输入对话框
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="codeImage">图片</param>
        /// <returns></returns>
        public static string InputCodeText(string caption, Image codeImage)
        {
            Form inputForm = new Form
            {
                MinimizeBox = false,
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen,
                Width = 220,
                Height = 170,
                Text = caption
            };

            PictureBox imgBox = new PictureBox
            {
                Top = 12,
                Left = 54,
                Width = 100,
                Height = 50,
                Parent = inputForm,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = codeImage
            };

            TextBox tb = new TextBox { Left = 30, Top = 65, Width = 160, Parent = inputForm };
            tb.SelectAll();

            Button btnok = new Button { Left = 30, Top = 100, Parent = inputForm, Text = "确定" };
            inputForm.AcceptButton = btnok;//回车响应

            btnok.DialogResult = DialogResult.OK;
            Button btncancal = new Button
            {
                Left = 120,
                Top = 100,
                Parent = inputForm,
                Text = "取消",
                DialogResult = DialogResult.Cancel
            };
            try
            {
                return inputForm.ShowDialog() == DialogResult.OK ? tb.Text : null;
            }
            finally
            {
                inputForm.Dispose();
            }
        }

        /// <summary>
        /// 输入对话框
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="hint">提示内容</param>
        /// <param name="Default">默认值</param>
        /// <returns></returns>
        public static string InputBox(string caption, string hint, string Default)
        {
            Form inputForm = new Form
            {
                MinimizeBox = false,
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen,
                Width = 220,
                Height = 150,
                Text = caption
            };

            Label lbl = new Label { Text = hint, Left = 10, Top = 20, Parent = inputForm, AutoSize = true };
            TextBox tb = new TextBox { Left = 30, Top = 45, Width = 160, Parent = inputForm, Text = Default };
            tb.SelectAll();
            Button btnok = new Button { Left = 30, Top = 80, Parent = inputForm, Text = "确定" };
            inputForm.AcceptButton = btnok;//回车响应

            btnok.DialogResult = DialogResult.OK;
            Button btncancal = new Button
            {
                Left = 120,
                Top = 80,
                Parent = inputForm,
                Text = "取消",
                DialogResult = DialogResult.Cancel
            };
            try
            {
                return inputForm.ShowDialog() == DialogResult.OK ? tb.Text : null;
            }
            finally
            {
                inputForm.Dispose();
            }
        }
    }
}