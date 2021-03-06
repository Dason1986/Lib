using Library.Annotations;
using System;
using System.Drawing;
using System.IO;

namespace Library.Draw.Water
{
    /// <summary>
    ///
    /// </summary>
    public abstract class WaterImageBuilder : ImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        protected string WaterImgPath { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected byte[] WaterImgBuffter { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="waterImgPath"></param>
        public void SetWaterImage([NotNull] string waterImgPath)
        {
            if (!File.Exists(waterImgPath)) throw new FileNotFoundException("File Not Found", waterImgPath);
            WaterImgPath = waterImgPath;
            WaterImgBuffter = File.ReadAllBytes(waterImgPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffter"></param>
        public void SetWaterImage([NotNull] byte[] buffter)
        {
            if (buffter == null) throw new ArgumentNullException("buffter");
            WaterImgBuffter = buffter;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceImg"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected virtual Rectangle GetWaterRectangle(Image sourceImg, Image waterImg)
        {
            return new Rectangle(0, 0, sourceImg.Width, sourceImg.Height);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            if (Opetion == null) throw new ImageException("Opetion is null");

            MemoryStream sourcestream = new MemoryStream(SourceImgBuffter);
            MemoryStream waterImagestream = new MemoryStream(SourceImgBuffter);
            var sourceImg = Image.FromStream(sourcestream);
            var waterImg = Image.FromStream(waterImagestream);
            var trageSize = Opetion == null ? null : Opetion.TragetSize;
            Image tmpimg = trageSize != null ? new Bitmap(trageSize.Value.Width, trageSize.Value.Height) : new Bitmap(sourceImg.Width, sourceImg.Height);
            Graphics gType = CreateGraphics(tmpimg, sourceImg);
            var attributes = GetOpacity(Opetion.Opacity);
            var waterRectangle = GetWaterRectangle(sourceImg, waterImg);
            var tmpwatrer = CreateFillImage(waterRectangle, waterImg);
            try
            {
                gType.DrawImage(tmpwatrer, waterRectangle, 0, 0, tmpwatrer.Width, tmpwatrer.Height, GraphicsUnit.Pixel, attributes);
                return tmpimg;
            }
            finally
            {
                waterImagestream.Dispose();
                sourcestream.Dispose();
                tmpwatrer.Dispose();
                sourceImg.Dispose();
                waterImg.Dispose();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tmpimg"></param>
        /// <param name="sourceImg"></param>
        /// <returns></returns>
        protected Graphics CreateGraphics(Image tmpimg, Image sourceImg)
        {
            Graphics gType = Graphics.FromImage(tmpimg);
            var trageSize = Opetion == null ? null : Opetion.TragetSize;
            if (trageSize != null)
            {
                gType.DrawImage(sourceImg, new Rectangle(new Point(0, 0), trageSize.Value), 0, 0, sourceImg.Width,
                    sourceImg.Height, GraphicsUnit.Pixel);
            }
            else
            {
                gType.DrawImage(sourceImg, new Point(0, 0));
            }
            return gType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected abstract Image CreateFillImage(Rectangle rectangle, Image waterImg);
    }
}