using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Library.Annotations;
using Library.Draw.Effects;

namespace Library.Draw
{

    /// <summary>
    /// 图像处理功能
    /// </summary>
    public interface IImageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceImgPath"></param>
        void SetSourceImage([NotNull] string sourceImgPath);

        void SetSourceImage([NotNull] byte[] buffter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="opetion"></param>
        void SetOpetion([NotNull] ImageOption opetion);
        /// <summary>
        /// .net自带处理方法
        /// </summary>
        Image ProcessBitmap();

        Image ProcessBitmap(ImageOption opetion);

        /// <summary>
        /// 不安全代码处理方法
        /// </summary>
        unsafe Image UnsafeProcessBitmap();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="opacity"></param>
        void SetOpacity(float opacity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        void SetTrageSize(Size size);
    }

    public abstract class ImageBuilder : IImageBuilder
    {
        protected string SourceImgPath { get; private set; }
        protected byte[] SourceImgBuffter { get; private set; }
        private Image _source;
        protected Image Source
        {
            get
            {
                if (_source != null) return _source;
                if (SourceImgBuffter != null)
                {
                    MemoryStream memory = new MemoryStream(SourceImgBuffter);
                    _source = new Bitmap(memory);
                    return _source;
                }
                if (File.Exists(SourceImgPath))
                {
                    _source = new Bitmap(SourceImgPath);
                    return _source;
                }
                return null;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceImgPath"></param>
        public void SetSourceImage(string sourceImgPath)
        {

            if (!File.Exists(sourceImgPath)) throw new FileNotFoundException("文件不存在", sourceImgPath);
            SourceImgPath = sourceImgPath;
            SourceImgBuffter = File.ReadAllBytes(sourceImgPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffter"></param>
        public void SetSourceImage(byte[] buffter)
        {
            if (buffter == null) throw new ArgumentNullException("buffter");
            SourceImgBuffter = buffter;
        }

        public virtual Image ProcessBitmap(ImageOption opetion)
        {
            SetOpetion(opetion);
            return ProcessBitmap();
        }

        public virtual unsafe Image UnsafeProcessBitmap()
        {
            throw new NotImplementedException();
        }

        protected virtual ImageOption Opetion { get; set; }

        public virtual void SetOpetion([NotNull] ImageOption opetion)
        {
            if (opetion == null) throw new ArgumentNullException("opetion");
            Opetion = opetion;
        }

        public virtual void SetOpacity(float opacity)
        {
            InitOption();
            Opetion.Opacity = opacity;
        }
        public virtual void SetTrageSize(Size size)
        {
            InitOption();
            Opetion.TrageSize = size;
        }

        protected virtual void InitOption()
        {
            if (Opetion == null) Opetion = new ImageOption();
        }
        public abstract Image ProcessBitmap();


        protected ImageAttributes GetOpacity(float opacity)
        {
            float[][] nArray =
            {
                new[] {1f, 0, 0, 0, 0},
                new[] {0, 1f, 0, 0, 0},
                new[] {0, 0, 1f, 0, 0},
                new[] {0, 0, 0,opacity , 0},
                new[] {0, 0, 0, 0, 1f}
            };
            ColorMatrix matrix = new ColorMatrix(nArray);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return attributes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processable"></param>
        /// <param name="sourcePath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static TryResult ProcessBitmap(IImageBuilder processable, string sourcePath, string savePath)
        {
            if (processable == null) return false;
            if (!File.Exists(sourcePath)) return false;

            try
            {
                processable.SetSourceImage(sourcePath);
                var image = processable.ProcessBitmap();
                image.Save(savePath);

            }
            catch (Exception ex)
            {
                return ex;

            }

            return true;
        }
    }
}