using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Library.Draw;
using Library.HelperUtility;

namespace ImageManagement
{
    public partial class EffectsForm : Form
    {
        public EffectsForm()
        {
            InitializeComponent();
            List<string> source = new List<string>()
            {
                "BlueImage", "GreenImage", "RedImage", "BrightnessImage", "ContrastImage", "FogImage"
            ,"GaussianBlurImage","ImageFlip","MosaicImage","NeonImage","PixelFunImage","RebelliousImage"
            ,"ReliefImage","SharpenImage","TwoValueImage"
            };
            comboBox1.DataSource = source;
            effectsAssembly = typeof(ImageBuilder).Assembly;

        }

        private Assembly effectsAssembly;
        private byte[] fileBytes;
        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "所支持图片|*.png;*.jpg;*.gif";
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                fileBytes = File.ReadAllBytes(fileDialog.FileName);
                var stream = fileDialog.OpenFile();
                this.pictureBox1.Image = new Bitmap(stream);
                comboBox1_SelectedIndexChanged(this, EventArgs.Empty);
            }

        }

        private IImageBuilder builderobj;
        private void button2_Click(object sender, EventArgs e)
        {


            this.pictureBox2.Image = checkBox1.Checked ? builderobj.UnsafeProcessBitmap() : builderobj.ProcessBitmap();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (effectsAssembly != null)
            {
                string classname = string.Format("Library.Draw.Effects.{0}", comboBox1.Text);
                var typeobj = effectsAssembly.GetType(classname);
                if (typeobj == null) throw new Exception();
                builderobj = typeobj.CreateInstance<IImageBuilder>();
                grid.SelectedObject = new ImageOption();
                var tmp = new byte[fileBytes.Length];
                fileBytes.CopyTo(tmp,0);
                builderobj.SetSourceImage(tmp);
            }
        }
    }
}
