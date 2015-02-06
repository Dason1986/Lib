using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Library.Draw;
using Library.HelperUtility;
using Library.Win.Controls;

namespace TestWinform
{
    public partial class EffectsForm : Form
    {
        public EffectsForm()
        {
            InitializeComponent();
            List<string> source = new List<string>()
            {
                "BlueImage", "GreenImage", "RedImage",  "FogImage"
            ,"FlipImage","MosaicImage","NeonImage","GrayImage","RebelliousImage"
            ,"ReliefImage","SharpenImage","TwoValueImage","ColorGradationImage","BlindsImage","IlluminationImage"
            ,"ZoomBlurImage","ColorQuantizeImage","ColorToneImage","AutoLevelImage","HistogramEqualImage"
            ,"BrightContrastImage",  "CleanGlassImage" ,"FeatherImage","RaiseFrameImage"
            };
            comboBox1.DataSource = source;
            effectsAssembly = typeof(ImageBuilder).Assembly;

            if (Program.Original == null) return;
            Image image = new Bitmap(new MemoryStream(Program.Original));
            this.pictureBox1.Image = image;
            fileBytes = Program.Original;
            CreateBuilder();
        }

        private readonly Assembly effectsAssembly;
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
                CreateBuilder();
            }

        }

        private IImageBuilder builderobj;
        private ImageOption option;
        private void button2_Click(object sender, EventArgs e)
        {


            try
            {

                panel1.Enabled = false;
                builderobj.SetOpetion(option);

                this.pictureBox2.Image = checkBox1.Checked
                    ? builderobj.UnsafeProcessBitmap()
                    : builderobj.ProcessBitmap();
            }
            catch (Exception ex)
            {
                this.pictureBox2.Image = null;
                MessageBox.Show(ex.Message);

            }
            finally
            {
                panel1.Enabled = true;

            }
        }
        readonly IDictionary<Type, IImageBuilder> _dicinObjects = new ConcurrentDictionary<Type, IImageBuilder>();
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateBuilder();
        }

        private void CreateBuilder()
        {
            if (effectsAssembly == null) return;
            string classname = string.Format("Library.Draw.Effects.{0}", comboBox1.Text);
            var typeobj = effectsAssembly.GetType(classname);
            if (typeobj == null) throw new Exception();
            if (_dicinObjects.ContainsKey(typeobj))
            {
                builderobj = _dicinObjects[typeobj];
            }
            else
            {
                builderobj = typeobj.CreateInstance<IImageBuilder>();
                builderobj.ProcessCompleted += builderobj_ProcessCompleted;
                if (fileBytes != null)
                {
                    var tmp = new byte[fileBytes.Length];
                    fileBytes.CopyTo(tmp, 0);
                    builderobj.SetSourceImage(tmp);
                }
            }
            option = builderobj.CreateOption();
            grid.SelectedObject = option;
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (var imageBuilder in _dicinObjects.Values)
            {
                imageBuilder.ProcessCompleted -= builderobj_ProcessCompleted;
            }
            _dicinObjects.Clear();
            base.OnClosed(e);
        }

        void builderobj_ProcessCompleted(object sender, ImageEventArgs e)
        {
            this.pictureBox2.Image = e.Image;
            if (e.Error != null) MessageBox.Show(e.Error.Message);

            panel1.Enabled = true;
            cmd.HideOpaqueLayer();
        }

        private OpaqueCommand cmd;
        private void button3_Click(object sender, EventArgs e)
        {

            panel1.Enabled = false;
            cmd = OpaqueLayer.Show(this, 125, true);
            builderobj.SetOpetion(option);

            if (checkBox1.Checked)
            {
                builderobj.UnsafeProcessBitmapAsync();
            }
            else
            {
                builderobj.ProcessBitmapAsync();
            }

        }
    }
}
