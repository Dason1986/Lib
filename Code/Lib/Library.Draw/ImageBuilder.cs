using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Library.Annotations;

namespace Library.Draw
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageEventArgs : EventArgs
    {
        public Exception Error { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public Image Image { get; protected set; }


        protected internal ImageEventArgs(Image image)
        {
            Image = image;

        }

        protected internal ImageEventArgs(Exception error)
        {
            Error = error;

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ImageCompletedEventHandler(object sender, ImageEventArgs e);
    /// <summary>
    /// 图像处理功能
    /// </summary>
    public interface IImageBuilder
    {

        /// <summary>
        /// 
        /// </summary>
        event ImageCompletedEventHandler ProcessCompleted;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceImgPath"></param>
        void SetSourceImage([NotNull] string sourceImgPath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffter"></param>
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

        /// <summary>
        /// 
        /// </summary>
        void ProcessBitmapAsync();

        /// <summary>
        /// 
        /// </summary>
        unsafe void UnsafeProcessBitmapAsync();
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ImageOption CreateOption();
    }

    public abstract class ImageBuilder : IImageBuilder
    {

        /// <summary>
        /// 
        /// </summary>
        public event ImageCompletedEventHandler ProcessCompleted;
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

        public virtual ImageOption CreateOption()
        {
            return new ImageOption();
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

        public unsafe void UnsafeProcessBitmapAsync()
        {
            BackgroundWorker background = new BackgroundWorker();
            Image image = null;
            background.DoWork += (x, y) =>
            {
                image = UnsafeProcessBitmap();
            };
            background.RunWorkerCompleted += (x, y) =>
            {
                //y.Error
                OnProcessBitmapCompleted(y.Error == null ? new ImageEventArgs(image) : new ImageEventArgs(y.Error));
            };

            background.RunWorkerAsync();
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
        public void ProcessBitmapAsync()
        {
            BackgroundWorker background = new BackgroundWorker();
            Image image = null;
            background.DoWork += (x, y) =>
            {
                image = ProcessBitmap();
            };
            background.RunWorkerCompleted += (x, y) =>
            {
                OnProcessBitmapCompleted(y.Error == null ? new ImageEventArgs(image) : new ImageEventArgs(y.Error));
            };
            background.RunWorkerAsync();
        }


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

        protected virtual void OnProcessBitmapCompleted(ImageEventArgs e)
        {
            var handler = ProcessCompleted;
            if (handler != null) handler(this, e);
        }


    }
}