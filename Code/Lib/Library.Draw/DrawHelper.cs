using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Library.Draw
{
    /// <summary>
    /// System.Drawing.ColorTranslator
    /// </summary>
    public static class DrawHelper
    {
#if !MONO

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        ///  Image转换为Icon
        /// </summary>
        /// <param name="orgImg"></param>
        /// <returns></returns>
        public static Icon ImageToIcon(this Image orgImg)
        {
            var bmp = new Bitmap(orgImg);
            IntPtr h = bmp.GetHicon();
            Icon icon = Icon.FromHandle(h);
            // 释放IntPtr
            DeleteObject(h);
            return icon;
        }

#endif

        /// <summary>
        ///  字节流 转化为 Image
        /// </summary>
        /// <param name="btArray"></param>
        /// <returns></returns>
        public static Image ImageFromByteArray(byte[] btArray)
        {
            var ms = new MemoryStream(btArray);
            Image returnImage = Image.FromStream(ms);
            ms.Dispose();
            return returnImage;
        }

        /// <summary>
        ///  Image 转化为 字节流
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(this Image image, ImageFormat format)
        {
            var ms = new MemoryStream();
            image.Save(ms, format);
            return ms.ToArray();
        }

        /// <summary>
        /// 創建黑白雜點圖
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap CreateNoise(int width, int height)
        {
            Bitmap finalBmp = new Bitmap(width, height);
            Random r = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int num = r.Next(0, 256);
                    finalBmp.SetPixel(x, y, Color.FromArgb(255, num, num, num));
                }
            }

            return finalBmp;
        }

        /// <summary>
        /// 主要色調（平均色）
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Color GetDominantColor(Bitmap bmp)
        {
            //Used for tally
            int r = 0;
            int g = 0;
            int b = 0;

            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            //Calculate average
            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(r, g, b);
        }   /// <summary>

            /// 获取拍照日期/时间
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
        public static string GetTakePicDateTime(string fileName)
        {
            var items = GetExifProperties(fileName);
            return GetTakePicDateTime(items);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static PropertyItem[] GetExifProperties(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            //通过指定的数据流来创建Image

            Image image = Image.FromStream(stream, true, false);
            stream.Close();
            stream.Dispose();
            var items = image.PropertyItems;
            image.Dispose();
            return items;
        }

        //遍历所有元数据，获取拍照日期/时间

        private static string GetTakePicDateTime(IEnumerable<PropertyItem> parr)
        {
            Encoding ascii = Encoding.ASCII;
            //遍历图像文件元数据，检索所有属性
            foreach (System.Drawing.Imaging.PropertyItem p in parr)
            {
                //如果是PropertyTagDateTime，则返回该属性所对应的值

                if (p.Id == 0x0132)
                {
                    return ascii.GetString(p.Value);
                }
            }
            //若没有相关的EXIF信息则返回N/A

            return "N/A";
        }

        /*
        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public uint ColorToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) |
                          (color.G << 8) | (color.B << 0));
        }

        public string ColorToHex(Color color)
        {
            return string.Format("#{0}{1}{2}", color.R.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static Color HexToColor(string hexColor)
        {
            if (IsValidHex(hexColor)) throw new Exception();
            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(0, 1);

            switch (hexColor.Length)
            {
                case 6:
                    {
                        int argb = Int32.Parse(hexColor, NumberStyles.HexNumber);
                        return Color.FromArgb(argb);
                    }
                case 3:
                    {
                        int red = int.Parse(hexColor[0].ToString() + hexColor[0].ToString(), NumberStyles.AllowHexSpecifier);
                        int green = int.Parse(hexColor[1].ToString() + hexColor[1].ToString(), NumberStyles.AllowHexSpecifier);
                        int blue = int.Parse(hexColor[2].ToString() + hexColor[2].ToString(), NumberStyles.AllowHexSpecifier);
                        return Color.FromArgb(red, green, blue);
                    }
                default: throw new NotSupportedException();
            }
        }

        public static bool IsValidHex(string hexColor)
        {
            if (hexColor.StartsWith("#"))
                return hexColor.Length == 7 || hexColor.Length == 4;
            else
                return hexColor.Length == 6 || hexColor.Length == 3;
        }
         */
    }
}