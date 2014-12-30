using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// ·Å´óçR
    /// </summary>
    public class ImageZoom
    {
      
        private Image ZoomImage(Image input, Rectangle zoomArea, Rectangle sourceArea)
        {
            Bitmap newBmp = new Bitmap(sourceArea.Width, sourceArea.Height);

            using (Graphics g = Graphics.FromImage(newBmp))
            {
                //high interpolation
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(input, sourceArea, zoomArea, GraphicsUnit.Pixel);
            }

            return newBmp;
        }

        /// <summary>
        /// Returns the dominant color of an image
        /// </summary>
        private Color GetDominantColor(Bitmap bmp, bool includeAlpha)
        {
            // GDI+ still lies to us - the return format is BGRA, NOT ARGB.
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                           ImageLockMode.ReadWrite,
                                           PixelFormat.Format32bppArgb);

            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;

            int r = 0;
            int g = 0;
            int b = 0;
            int a = 0;
            int total = 0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - bmp.Width * 4;
                int nWidth = bmp.Width;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        r += p[0];
                        g += p[1];
                        b += p[2];
                        a += p[3];

                        total++;

                        p += 4;
                    }
                    p += nOffset;
                }
            }

            bmp.UnlockBits(bmData);

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            if (includeAlpha)
                return Color.FromArgb(a, r, g, b);
            else
                return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Calculates the opposite color of a given color. 
        /// Source: http://dotnetpulse.blogspot.com/2007/01/function-to-calculate-opposite-color.html
        /// </summary>
        /// <param name="clr"></param>
        /// <returns></returns>
        private Color CalculateOppositeColor(Color clr)
        {
            return Color.FromArgb(255 - clr.R, 255 - clr.G, 255 - clr.B);
        }

        /// <summary>
        /// Constricts a set of given dimensions while keeping aspect ratio.
        /// </summary>
        private Size ShrinkToDimensions(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
        {
            int newWidth = 0;
            int newHeight = 0;

            if (originalWidth >= originalHeight)
            {
                //Match area width to max width
                if (originalWidth <= maxWidth)
                {
                    newWidth = originalWidth;
                    newHeight = originalHeight;
                }
                else
                {
                    newWidth = maxWidth;
                    newHeight = originalHeight * maxWidth / originalWidth;
                }
            }
            else
            {
                //Match area height to max height
                if (originalHeight <= maxHeight)
                {
                    newWidth = originalWidth;
                    newHeight = originalHeight;
                }
                else
                {
                    newWidth = originalWidth * maxHeight / originalHeight;
                    newHeight = maxHeight;
                }
            }

            return new Size(newWidth, newHeight);
        }
        /*
        private void picSmall_Paint(object sender, PaintEventArgs e)
        {
            if (loadedImage != null)
            {
                //Adjust the selected area to reflect the zoom value
                Rectangle adjustedArea = new Rectangle();
                adjustedArea.X = selectedArea.X;
                adjustedArea.Y = selectedArea.Y;
                adjustedArea.Width = selectedArea.Width / tZoom.Value;
                adjustedArea.Height = selectedArea.Height / tZoom.Value;

                //Draw the selected area on the thumbnail
                picSmall.Image = MarkImage(thumbnail, adjustedArea, selectionColor);
            }
        }

        private void picSmall_Click(object sender, EventArgs e)
        {
            //Update the selected area when the user clicks on the thumbnail
            Point mouseLoc = picSmall.PointToClient(Cursor.Position);

            selectedArea.X = mouseLoc.X - ((selectedArea.Width / tZoom.Value) / 2);
            selectedArea.Y = mouseLoc.Y - ((selectedArea.Height / tZoom.Value) / 2);

            //Bound the box to the picture area bounds
            if (selectedArea.Left < 0)
                selectedArea.X = 0;
            else if (selectedArea.Right > picSmall.Width)
                selectedArea.X = picSmall.Width - selectedArea.Width - 1;

            if (selectedArea.Top < 0)
                selectedArea.Y = 0;
            else if (selectedArea.Bottom > picSmall.Height)
                selectedArea.Y = picSmall.Height - selectedArea.Height - 1;

            picSmall.Invalidate();
            updateZoom();
        }

        private void tValue_Scroll(object sender, EventArgs e)
        {
            updateZoom();
        }*/
        private Image MarkImage(Image input, Rectangle selectedArea, Color selectColor)
        {
            Bitmap newImg = new Bitmap(input.Width, input.Height);

            using (Graphics g = Graphics.FromImage(newImg))
            {
                //Prevent using images internal thumbnail
                input.RotateFlip(RotateFlipType.Rotate180FlipNone);
                input.RotateFlip(RotateFlipType.Rotate180FlipNone);

                g.DrawImage(input, 0, 0);

                //Draw the selection rect
                using (Pen p = new Pen(selectColor))
                    g.DrawRectangle(p, selectedArea);
            }

            return (Image)newImg;
        }

     
        private Image ResizeImage(Image input, Size newSize, InterpolationMode interpolation)
        {
            Bitmap newImg = new Bitmap(newSize.Width, newSize.Height);

            using (Graphics g = Graphics.FromImage(newImg))
            {
                //Prevent using images internal thumbnail
                input.RotateFlip(RotateFlipType.Rotate180FlipNone);
                input.RotateFlip(RotateFlipType.Rotate180FlipNone);

                //Interpolation
                g.InterpolationMode = interpolation;

                //Draw the image with the new dimensions
                g.DrawImage(input, 0, 0, newSize.Width, newSize.Height);
            }

            return (Image)newImg;
        }
    }
}