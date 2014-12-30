using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Library.Draw.Effects
{
    /// <summary>
    /// µ¹Ïó
    /// </summary>
    public class ImageFlip : ImageBuilder
    {
        public enum FlipType
        {
            Horizontally,
            Vertically,
            HorizontallyAndVertically,
        }

        public FlipType Flip { get; set; }
        public Image CreateImage(FlipType flip)
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
                    case FlipType.HorizontallyAndVertically:
                        {
                            m = new Matrix(-1, 0, 0, -1, 0, 0);
                            m.Translate(flippedImage.Width, flippedImage.Height, MatrixOrder.Append);
                            break;
                        }
                    case FlipType.Horizontally:
                        {
                            m = new Matrix(-1, 0, 0, 1, 0, 0);
                            m.Translate(flippedImage.Width, 0, MatrixOrder.Append);
                            break;
                        }
                    case FlipType.Vertically:
                        {
                            m = new Matrix(1, 0, 0, -1, 0, 0);
                            m.Translate(0, flippedImage.Height, MatrixOrder.Append);
                            break;
                        }
                    default: throw new ImageException("");
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
    }
}