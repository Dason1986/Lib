using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
        public ThumbnailModel Model { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ImageFormat TragetFormat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Size TragetSize { get; set; }



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

            bitmap.Save(ms, TragetFormat);


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