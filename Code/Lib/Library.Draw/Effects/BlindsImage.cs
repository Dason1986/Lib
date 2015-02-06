using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 以百叶效果显示图像
    /// </summary> 
    [LanguageDescription("百叶效果"), LanguageDisplayName("百叶效果")]
    public class BlindsImage : ImageBuilder
    {
        /*
          根据图像的高度或宽度和定制的百叶窗显示条宽度计算百叶窗显示的条目数量
         */
        #region Option
        /// <summary>
        /// 方向
        /// </summary>

        [LanguageDescription("方向"), LanguageDisplayName("方向"), Category("濾鏡選項")]
      
        public AlignmentType Alignment
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
        /// <summary>
        /// 線條數量
        /// </summary>

        [LanguageDescription("線條數量"), LanguageDisplayName("線條數量"), Category("濾鏡選項")]
     
        public int BarCount
        {
            get
            {
                InitOption(); return _opetion.Count;
            }
            set
            {
                InitOption(); _opetion.Count = value;
            }
        }
        /// <summary>
        /// 線條大小
        /// </summary>

        [LanguageDescription("線條大小"), LanguageDisplayName("線條大小"), Category("濾鏡選項")]
        
        public int BarPixel
        {
            get
            {
                InitOption(); return _opetion.Pixel;
            }
            set
            {
                InitOption(); _opetion.Pixel = value;
            }
        }
        /// <summary>
        /// 線條顏色
        /// </summary>

        [LanguageDescription("線條顏色"), LanguageDisplayName("線條顏色"), Category("濾鏡選項")]
 
        public Color BarColor
        {
            get
            {
                InitOption(); return _opetion.BarColor;
            }
            set
            {
                InitOption(); _opetion.BarColor = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public class BlindsOption : ImageOption
        {
            /// <summary>
            /// 
            /// </summary>
            [LanguageDescription("方向"), LanguageDisplayName("方向"), Category("濾鏡選項")]
            public AlignmentType Alignment { get; set; }
            /// <summary>
            /// 
            /// </summary> 
            [LanguageDescription("線條數量"), LanguageDisplayName("線條數量"), Category("濾鏡選項")]
            public int Count { get; set; }
            /// <summary>
            /// 
            /// </summary> 
            [LanguageDescription("線條大小"), LanguageDisplayName("線條大小"), Category("濾鏡選項")]
            public int Pixel { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [LanguageDescription("線條顏色"), LanguageDisplayName("線條顏色"), Category("濾鏡選項")]
            public Color BarColor { get; set; }
        }

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = CreateOption() as BlindsOption;
        }
        private BlindsOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is BlindsOption == false) throw new ImageException("Opetion is not BlindsOption");
                _opetion = (BlindsOption) value;
            }
        }
        public override ImageOption CreateOption()
        {
            return new BlindsOption { Count = 5, Pixel = 4, Alignment = AlignmentType.Vertically, BarColor = Color.DodgerBlue };
        }

        #endregion

        #region  Process

        public override Image ProcessBitmap()
        {
            var myBitmap = (Bitmap)this.Source.Clone();
            int count = BarCount;
            int pixel = BarPixel;
            switch (Alignment)
            {
                case AlignmentType.Horizontally:
                    {
                        int dw = myBitmap.Width;
                        int dh = myBitmap.Height / count;

                        Point[] myPoint = new Point[count - 1];
                        for (int y = 0; y < myPoint.Length; y++)
                        {
                            myPoint[y].Y = (y + 1) * dh;
                            myPoint[y].X = 0;
                        }




                        foreach (Point t in myPoint)
                        {

                            for (int k = 0; k < dw; k++)
                            {
                                for (int i = 0; i < pixel; i++)
                                {
                                    myBitmap.SetPixel(t.X + k, t.Y + i, BarColor);
                                }
                            }
                        }
                        break;
                    }

                case AlignmentType.Vertically:
                    {
                        int dw = myBitmap.Width / count;
                        int dh = myBitmap.Height;

                        Point[] myPoint = new Point[count - 1];
                        for (int x = 0; x < myPoint.Length; x++)
                        {
                            myPoint[x].Y = 0;
                            myPoint[x].X = (x + 1) * dw;
                        }



                        foreach (Point t in myPoint)
                        {

                            for (int k = 0; k < dh; k++)
                            {
                                for (int i = 0; i < pixel; i++)
                                {
                                    myBitmap.SetPixel(t.X + i, t.Y + k, BarColor);

                                }
                            }
                        }
                        break;
                    }
                default: throw new ImageException("Not support");
            }





            return myBitmap;
        }

        public override unsafe Image UnsafeProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            if (bmp == null) return null;
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            int count = BarCount;
            int pixel = BarPixel;
            byte rr = BarColor.R, gg = BarColor.G, bb = BarColor.B;
            switch (Alignment)
            {
                case AlignmentType.Horizontally:
                    {
                        int dw = width;
                        int dh = height / count;
                        var spaceFirst = dw * 4 * pixel;
                        var spaceRec = dw * 4 * (dh - pixel);
                        Point[] myPoint = new Point[count - 1];
                        for (int y = 0; y < myPoint.Length; y++)
                        {
                            myPoint[y].Y = (y + 1) * dh;
                            myPoint[y].X = 0;
                        }

                        ptr += spaceFirst;
                        foreach (Point t in myPoint)
                        {
                            ptr += spaceRec;
                            for (int i = 0; i < pixel; i++)
                            {
                                for (int k = 0; k < dw; k++)
                                {
                                    ptr[0] = bb;
                                    ptr[1] = gg;
                                    ptr[2] = rr;
                                    ptr += 4;
                                }
                            }
                        }
                        break;
                    }

                case AlignmentType.Vertically:
                    {
                        int dw = width / count;
                        int dh = height;
                        var spaceFirst = 4 * dw;
                        var spaceRec = 4 * (dw - 4);
                        var spaceend = 4 * (width - dw * count);
                        Point[] myPoint = new Point[count - 1];
                        for (int x = 0; x < myPoint.Length; x++)
                        {
                            myPoint[x].Y = 0;
                            myPoint[x].X = (x + 1) * dw;
                        }
                        for (int k = 0; k < dh; k++)
                        {
                            ptr += spaceFirst;
                            foreach (Point t in myPoint)
                            {


                                for (int i = 0; i < pixel; i++)
                                {
                                    ptr[0] = bb;
                                    ptr[1] = gg;
                                    ptr[2] = rr;
                                    ptr += 4;
                                }
                                ptr += spaceRec;

                            }
                            ptr += spaceend;
                        }
                        break;
                    }
                default: throw new ImageException("Not support");
            }

            bmp.UnlockBits(bmpData);
            return bmp;
        }

        #endregion

    }
}
