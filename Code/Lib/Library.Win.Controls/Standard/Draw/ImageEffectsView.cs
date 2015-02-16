using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Library.Draw;
using Library.Draw.Effects;

namespace Library.Controls
{
    public partial class ImageEffectsView : Form, IImageEffectsView
    {
        private readonly Image _source;
        private Assembly effectsAssembly;
        private IImageBuilder builderobj;
        private ImageOption option;
        public Image ResultImage { get; protected set; }
        public ImageEffectsView(Image source)
        {
            InitializeComponent();
            _source = source;
            this.pictureBox1.Image = source;
            effectsAssembly = typeof(ImageBuilder).Assembly;
            builderobj = new ColorToneImage();
            option = builderobj.CreateOption();
            this.grid.SelectedObject = option;
        }


        private void BtnBuilder_Click(object sender, EventArgs e)
        {


            try
            {

                //   panel1.Enabled = false;
                builderobj.SetOpetion(option);
                builderobj.SetSourceImage(_source);
                ResultImage = this.pictureBox1.Image = checkBox1.Checked
                          ? builderobj.UnsafeProcessBitmap()
                          : builderobj.ProcessBitmap();
            }
            catch (Exception ex)
            {
                ResultImage = this.pictureBox1.Image = null;
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //  panel1.Enabled = true;

            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
