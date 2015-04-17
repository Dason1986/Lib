using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Library.Draw.Code
{
    /// <summary>
    /// 圖片驗證碼
    /// </summary>
    public class ImageCode
    {
        /*
         默認：4位,純數字，寬100，高50的圖片驗證碼
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ImageCode(int width = 100, int height = 50)
        {
            CodeLength = 4;
            Height = height;
            Width = width;
            ContentType = ContentTypeEnum.Number;
        }
        private int _codeLength = 4;
        private int _height;
        private int _width;
        /// <summary>
        /// 
        /// </summary>
        public ContentTypeEnum ContentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public enum ContentTypeEnum
        {
            /// <summary>
            /// 
            /// </summary>
            Number,
            /// <summary>
            /// 
            /// </summary>
            Char,
            /// <summary>
            /// 
            /// </summary>
            All,
        }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get { return _width; }
            protected set
            {
                if (value < 100|value > 800) throw new ImageException();
          
                _width = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get { return _height; }
            protected set
            {
                if (value < 50||value > 400) throw new ImageException();
          
                _height = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CodeLength
        {
            get { return _codeLength; }
            set
            {
                if (value < 4||value > 10) throw new ImageException("");
              
                _codeLength = value;
                var tmpwidth = (int)Math.Ceiling(_codeLength * 25.0);
                if (tmpwidth > Width) Width = tmpwidth;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Image { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public void Create()
        {

            CreateStringCode();
            BuildingImage();
        }


        private static readonly string[] ConstCodes = {   "1", "2", "3", "3", "4", "5", "6", "7", "8", "9",
                                                 "a","b","c","d","e","f","g","h", "j","k","l","m","n" ,"p","q","r","s","t","u","v","w","x","y","z",
                                                 "A","B","C","D","E","F","G","H", "J","K","L","M","N", "P","Q","R","S","T","U","V","W","X","Y","Z",
                                                 };
        static readonly Color[] ConstColors = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
        private Font[] ConstFonts = 
    {
       new Font(new FontFamily("Times New Roman"),21,FontStyle.Regular),
       new Font(new FontFamily("Georgia"), 21,FontStyle.Regular),
       new Font(new FontFamily("Arial"),21,FontStyle.Regular),
       new Font(new FontFamily("Comic Sans MS"), 21,FontStyle.Regular)
    };
        private void BuildingImage()
        {
            Bitmap image = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(image);
            Random random = new Random();
            //画图片的背景噪音线
            for (int i = 0; i < 6; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(ConstColors[random.Next(ConstColors.Length)]), x1, y1, x2, y2);
            }

            var font = ConstFonts[random.Next(ConstFonts.Length)];
            //var rage = g.MeasureString(Code, font, 100, new StringFormat(StringFormatFlags.MeasureTrailingSpaces, CultureInfo.CurrentCulture.LCID));
            //var pit = new RectangleF(0,0, rage.Width, rage.Height);
            //g.DrawString(Code, font, new SolidBrush(Color.Black), pit, new StringFormat(StringFormatFlags.MeasureTrailingSpaces, CultureInfo.CurrentCulture.LCID));
            //Console.WriteLine(rage);
            for (int i = 0; i < Code.Length; i++)
            {
                var tmpcode = Code[i].ToString();
                Color clr = ConstColors[random.Next(ConstColors.Length)];
                var rotate = random.Next(-2, 2);
                g.TranslateTransform(i * 23, 0, MatrixOrder.Append);
                g.RotateTransform(rotate, MatrixOrder.Append);
                var brush = new SolidBrush(clr);
                g.DrawString(tmpcode, font, brush, 0, 5);

                g.ResetTransform();
            }


            //画图片的前景噪音点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);

                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            image = TwistImage(image, true, random.Next(1, 3), random.Next(4, 6));
            MemoryStream memory = new MemoryStream();
            image.Save(memory, ImageFormat.Png);

            Image = memory.ToArray();
            image.Dispose();
            memory.Dispose();

        }
        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高,一般为3</param>
        /// <param name="dPhase">波形的起始相位,取值区间[0-2*PI)</param>
        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            const double pi = Math.PI;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (pi * (double)j) / dBaseAxisLen : (pi * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }
        /// <summary>
        /// 
        /// </summary>
        protected void CreateStringCode()
        {
            var length = _codeLength;

            string[] validateNums = new string[length];

            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);

            //抽取随机数字
            for (int i = 0; i < length; i++)
            {



                switch (ContentType)
                {
                    case ContentTypeEnum.All: validateNums[i] = ConstCodes[seekRand.Next(0, ConstCodes.Length)]; break;
                    case ContentTypeEnum.Char: validateNums[i] = ConstCodes[seekRand.Next(10, ConstCodes.Length)]; break;
                    case ContentTypeEnum.Number: validateNums[i] = ConstCodes[seekRand.Next(0, 10)]; break;
                }
                //validateNums[i] = numStr;
            }

            Code = string.Join(string.Empty, validateNums);
        }
    }
}
