﻿using Library.Draw;
using Library.HelperUtility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Library.Controls
{
    /// <summary>
    ///
    /// </summary>
    public partial class ImageEffectsView : Form, IImageEffectsView
    {
        private Image Source
        {
            get { return _source; }
            set
            {
                _source = value;
                SetImageInfo();
            }
        }

        private Assembly effectsAssembly;
        private IImageBuilder builderobj;
        private ImageOption option;
        private readonly IDictionary<Type, IImageBuilder> _dicinObjects = new ConcurrentDictionary<Type, IImageBuilder>();

        /// <summary>
        ///
        /// </summary>
        public Image ResultImage { get; protected set; }

        private DataTable dt;
        private int index;
        private Image _source;

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        public ImageEffectsView(Image source)
        {
            InitializeComponent();
            Source = source;

            Init();
        }

        private void Init()
        {
            string[] sourceImageEffects =
            {
                "BlueImage", "GreenImage", "RedImage", "FogImage", "GaussianBlurImage"
                , "FlipImage", "MosaicImage", "NeonImage", "GrayImage", "RebelliousImage"
                , "ReliefImage", "SharpenImage", "TwoValueImage", "ColorGradationImage", "BlindsImage", "IlluminationImage"
                , "ZoomBlurImage", "ColorQuantizeImage", "ColorToneImage", "AutoLevelImage", "HistogramEqualImage"
                , "BrightContrastImage", "CleanGlassImage", "FeatherImage", "RaiseFrameImage", "ReflectionImage"
                , "ThreeDGridImage"
            };
            effectsAssembly = typeof(ImageBuilder).Assembly;
            dt = new DataTable();
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
            LBEffects.DataSource = dt;
            LBEffects.DisplayMember = "DisplayName";
            CreateBuilder();
        }

        private void SetImageInfo()
        {
            this.pictureBox1.Image = Source;
            this.propertyGrid1.SelectedObject = ImageExif.GetExifInfo(Source);
        }

        private void LBEffects_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateBuilder();
        }

        private void CreateBuilder()
        {
            if (index == LBEffects.SelectedIndex) return;
            index = LBEffects.SelectedIndex;
            if (index == -1) return;
            builderobj = (IImageBuilder)dt.Rows[index][2];
            builderobj.SetSourceImage(Source);
            option = (ImageOption)dt.Rows[index][3];
            grid.SelectedObject = option;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            foreach (var imageBuilder in _dicinObjects.Values)
            {
                imageBuilder.ProcessCompleted -= builderobj_ProcessCompleted;
            }
            _dicinObjects.Clear();
            base.OnClosed(e);
        }

        private void builderobj_ProcessCompleted(object sender, ImageEventArgs e)
        {
            this.pictureBox1.Image = e.Image;
            if (e.Error != null) MessageBox.Show(e.Error.Message);
        }

        private void BtnBuilder_Click(object sender, EventArgs e)
        {
            try
            {
                builderobj.SetOpetion(option);
                builderobj.SetSourceImage(Source);
                ResultImage = this.pictureBox1.Image = checkBox1.Checked
                          ? builderobj.UnsafeProcessBitmap()
                          : builderobj.ProcessBitmap();
            }
            catch (Exception ex)
            {
                ResultImage = this.pictureBox1.Image = null;
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "All Image Formats (*.bmp;*.jpg;*.jpeg;*.png;*.tif)|*.bmp;*.jpg;*.jpeg;*.png;*.tif|Bitmaps (*.bmp)|*.bmp|JPEGs (*.jpg)|*.jpg;*.jpeg|PNGs (*.png)|*.png|TIFs (*.tif)|*.tif" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Source = Image.FromFile(dialog.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog dialog = new SaveFileDialog() { Filter = ".jpeg|*.jpeg" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var fs = dialog.OpenFile();
                var ThumbnailWidth = 0;
                var ThumbnailHeight = 0;
                var image = ResultImage??Source;
                if (image.Width < 120 && image.Height < 120)
                {
                    ThumbnailWidth = image.Width;
                    ThumbnailHeight = image.Height;
                }
                else if (image.Width > image.Height)
                {
                    var per = (decimal)120 / image.Width;
                    ThumbnailWidth = 120;

                    ThumbnailHeight = (int)(image.Height * per);
                }
                else
                {
                    var per = (decimal)120 / image.Height;
                    ThumbnailHeight = 120;

                    ThumbnailWidth = (int)(image.Width * per);
                }
                Bitmap thumbnailImage = new Bitmap(image, new Size(ThumbnailWidth, ThumbnailHeight));

                thumbnailImage.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                thumbnailImage.Dispose();
                fs.Dispose();
            }
        }
    }
}