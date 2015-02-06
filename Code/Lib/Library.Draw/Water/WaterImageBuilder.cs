using System;
using System.Drawing;
using System.IO;
using Library.Annotations;

namespace Library.Draw.Water
{
    public abstract class WaterImageBuilder : ImageBuilder
    {




        protected string WaterImgPath { get; set; }
        protected byte[] WaterImgBuffter { get; private set; }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="waterImgPath"></param>
        public void SetWaterImage([NotNull] string waterImgPath)
        {


            if (!File.Exists(waterImgPath)) throw new FileNotFoundException("文件不存在", waterImgPath);
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
        protected virtual Rectangle GetWaterRectangle(Image sourceImg, Image waterImg)
        {
            return new Rectangle(0, 0, sourceImg.Width, sourceImg.Height);
        }

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

        protected abstract Image CreateFillImage(Rectangle rectangle, Image waterImg);





       


    }
}