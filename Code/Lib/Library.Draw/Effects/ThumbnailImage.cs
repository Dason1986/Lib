using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 
    /// </summary>
    public class ThumbnailImage : ImageBuilder
    {


        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum ThumbnailModel
        {
            /// <summary>
            /// 指定高宽缩放（可能变形）
            /// </summary>
            HW,
            /// <summary>
            /// 指定宽，高按比例
            /// </summary>
            W,
            /// <summary>
            /// 指定高，宽按比例
            /// </summary>
            H,
            /// <summary>
            /// 指定高宽裁减（不变形）
            /// </summary>
            Cut
        }
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("縮放方式"), Category("濾鏡選項")]
        public ThumbnailModel Model
        {
            get
            {
                InitOption();
                return _opetion.Model;
            }
            set
            {
                InitOption();
                _opetion.Model = value;
            }
        }

        /// <summary>
        /// 新尺寸
        /// </summary>
        [LanguageDescription("新尺寸"), Category("濾鏡選項")]
        public Size TragetSize
        {
            get
            {
                InitOption();
                return _opetion.TragetSize.GetValueOrDefault();
            }
            set
            {
                InitOption();
                _opetion.TragetSize = value;
            }
        }

        #region Option

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ThumbnailOption();
        }
        private ThumbnailOption _opetion;

        /// <summary>
        /// 
        /// </summary>
        public class ThumbnailOption : ImageOption
        {
            /// <summary>
            /// 
            /// </summary>
            public ThumbnailModel Model { get; set; }
        }

        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ThumbnailOption == false) throw new ImageException("Opetion is not ThumbnailOption");
                _opetion = (ThumbnailOption) value;
            }
        }

        public override ImageOption CreateOption()
        {
            return new ThumbnailOption();
        }
        #endregion

        public override Image ProcessBitmap()
        {
            Bitmap bmp = Source.Clone() as Bitmap;

            int towidth = TragetSize.Width;
            int toheight = TragetSize.Height;
            int x = 0;
            int y = 0;
            int ow = bmp.Width;
            int oh = bmp.Height;
            switch (Model)
            {
                case ThumbnailModel.HW://指定高宽缩放（可能变形）　　　　　　　　
                    break;
                case ThumbnailModel.W://指定宽，高按比例　　　　　　　　　　
                    toheight = bmp.Height * TragetSize.Width / bmp.Width;
                    break;
                case ThumbnailModel.H://指定高，宽按比例
                    towidth = bmp.Width * TragetSize.Height / bmp.Height;
                    break;
                case ThumbnailModel.Cut://指定高宽裁减（不变形）　　　　　　　　
                    if ((double)bmp.Width / (double)bmp.Height > (double)towidth / (double)toheight)
                    {
                        oh = bmp.Height;
                        ow = bmp.Height * towidth / toheight;
                        y = 0;
                        x = (bmp.Width - ow) / 2;
                    }
                    else
                    {
                        ow = bmp.Width;
                        oh = bmp.Width * TragetSize.Height / towidth;
                        x = 0;
                        y = (bmp.Height - oh) / 2;
                    }
                    break;
            }



            var bitmap = new Bitmap(towidth, toheight);
            Graphics g = Graphics.FromImage(bitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(bmp, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
            MemoryStream ms = new MemoryStream();

            bitmap.Save(ms, ImageFormat.Png);


            ms.Dispose();
            g.Dispose();

            return bitmap;




        }




        public override unsafe Image UnsafeProcessBitmap()
        {
            throw new NotImplementedException();
        }
    }
}