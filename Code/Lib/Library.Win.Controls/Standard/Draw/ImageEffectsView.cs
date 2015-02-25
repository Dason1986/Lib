using System;
using System.Collections.Concurrent;
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
using Library.HelperUtility;

namespace Library.Controls
{
    public partial class ImageEffectsView : Form, IImageEffectsView
    {
        private readonly Image _source;
        private Assembly effectsAssembly;
        private IImageBuilder builderobj;
        private ImageOption option;
        readonly IDictionary<Type, IImageBuilder> _dicinObjects = new ConcurrentDictionary<Type, IImageBuilder>();
        public Image ResultImage { get; protected set; }
        public ImageEffectsView(Image source)
        {
            InitializeComponent();
            _source = source;
            this.pictureBox1.Image = source;

            string[] sourceImageEffects = 
            {
                "BlueImage", "GreenImage", "RedImage",  "FogImage","GaussianBlurImage"
            ,"FlipImage","MosaicImage","NeonImage","GrayImage","RebelliousImage"
            ,"ReliefImage","SharpenImage","TwoValueImage","ColorGradationImage","BlindsImage","IlluminationImage"
            ,"ZoomBlurImage","ColorQuantizeImage","ColorToneImage","AutoLevelImage","HistogramEqualImage"
            ,"BrightContrastImage",  "CleanGlassImage" ,"FeatherImage","RaiseFrameImage","ReflectionImage"
          ,"ThreeDGridImage"
            };
            effectsAssembly = typeof(ImageBuilder).Assembly;

            dt.Columns.Add("name");
            dt.Columns.Add("DisplayName");
            dt.Columns.Add("ImageBuilder", typeof(IImageBuilder));
            dt.Columns.Add("option", typeof(ImageOption));
            for (int i = 0; i < sourceImageEffects.Length; i++)
            {
                var name = sourceImageEffects[i];
                var dr = dt.NewRow();
                string classname = string.Format("Library.Draw.Effects.{0}", name);
                var typeobj = effectsAssembly.GetType(classname);
                builderobj = typeobj.CreateInstance<IImageBuilder>();
                dr[0] = name;
                dr[1] = name;
                dr[2] = builderobj;
                dr[3] = builderobj.CreateOption();
                dt.Rows.Add(dr);
            }
            listBox1.DataSource = dt;
            listBox1.DisplayMember = "DisplayName";
            CreateBuilder();
        }
        DataTable dt = new DataTable();
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateBuilder();
        }

        private void CreateBuilder()
        {

            var index = listBox1.SelectedIndex;
            if (index == -1) return;
            builderobj = (IImageBuilder)dt.Rows[index][2];
            builderobj.SetSourceImage(_source);
            option = (ImageOption)dt.Rows[index][3];
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
            this.pictureBox1.Image = e.Image;
            if (e.Error != null) MessageBox.Show(e.Error.Message);



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
