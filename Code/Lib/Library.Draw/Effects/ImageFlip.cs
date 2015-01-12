using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Library.Draw.Effects
{
    [Flags]
    public enum AlignmentType
    {
        Horizontally = 1,
        Vertically = 2,

    }
    /// <summary>
    /// µ¹Ïó
    /// </summary>
    public class FlipImage : ImageBuilder
    {
        public AlignmentType Flip
        {
            get
            {
                InitOption(); return _opetion.Alignment;
            }
            set
            {
                InitOption(); _opetion.Alignment = value;
            }
        }
        #region Option


        public class FlipOption : ImageOption
        {
            public AlignmentType Alignment { get; set; }
        }

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new FlipOption();
        }
        private FlipOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is FlipOption == false) throw new ImageException("Opetion is not AlignmentOption");
                _opetion = value as FlipOption;
            }
        }
        public override ImageOption CreateOption()
        {
            return new FlipOption(){Alignment = AlignmentType.Horizontally|AlignmentType.Vertically  };
        }

        #endregion

        #region Process

        public Image CreateImage(AlignmentType flip)
        {

            MemoryStream sourcestream = new MemoryStream(SourceImgBuffter);
            Image image = new Bitmap(sourcestream);
            Bitmap flippedImage = Opetion != null && Opetion.TrageSize != null
                ? new Bitmap(Opetion.TrageSize.Value.Width, Opetion.TrageSize.Value.Height)
                : new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(flippedImage))
            {
                //Matrix transformation
                Matrix m = null;
                switch (flip)
                {
                    case AlignmentType.Horizontally | AlignmentType.Vertically:
                        {
                            m = new Matrix(-1, 0, 0, -1, 0, 0);
                            m.Translate(flippedImage.Width, flippedImage.Height, MatrixOrder.Append);
                            break;
                        }
                    case AlignmentType.Horizontally:
                        {
                            m = new Matrix(-1, 0, 0, 1, 0, 0);
                            m.Translate(flippedImage.Width, 0, MatrixOrder.Append);
                            break;
                        }
                    case AlignmentType.Vertically:
                        {
                            m = new Matrix(1, 0, 0, -1, 0, 0);
                            m.Translate(0, flippedImage.Height, MatrixOrder.Append);
                            break;
                        }
                    default: throw new ImageException("Not support");
                }


                //Draw
                g.Transform = m;
                if (Opetion != null)
                {
                    var attributes = GetOpacity(Opetion.Opacity);
                    g.DrawImage(image, new Rectangle(0, 0, flippedImage.Width, flippedImage.Height), 0, 0, flippedImage.Width, flippedImage.Height, GraphicsUnit.Pixel, attributes);
                }
                else
                {
                    g.DrawImage(image, 0, 0);
                }

                //clean up
                m.Dispose();
            }
            sourcestream.Dispose();
            return flippedImage;
        }

        public override Image ProcessBitmap()
        {
            return CreateImage(Flip);
        }
        #endregion
    }
}