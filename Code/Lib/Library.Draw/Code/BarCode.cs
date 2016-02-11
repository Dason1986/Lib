using System.Drawing;

namespace Library.Draw.Code
{
    /// <summary>
    ///
    /// </summary>
    public class BarCode
    {
        private const string AlphaBet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";

        private static readonly string[] Code39 =
             {
                 /* 0 */ "000110100",
                 /* 1 */ "100100001",
                 /* 2 */ "001100001",
                 /* 3 */ "101100000",
                 /* 4 */ "000110001",
                 /* 5 */ "100110000",
                 /* 6 */ "001110000",
                 /* 7 */ "000100101",
                 /* 8 */ "100100100",
                 /* 9 */ "001100100",
                 /* A */ "100001001",
                 /* B */ "001001001",
                 /* C */ "101001000",
                 /* D */ "000011001",
                 /* E */ "100011000",
                 /* F */ "001011000",
                 /* G */ "000001101",
                 /* H */ "100001100",
                 /* I */ "001001100",
                 /* J */ "000011100",
                 /* K */ "100000011",
                 /* L */ "001000011",
                 /* M */ "101000010",
                 /* N */ "000010011",
                 /* O */ "100010010",
                 /* P */ "001010010",
                 /* Q */ "000000111",
                 /* R */ "100000110",
                 /* S */ "001000110",
                 /* T */ "000010110",
                 /* U */ "110000001",
                 /* V */ "011000001",
                 /* W */ "111000000",
                 /* X */ "010010001",
                 /* Y */ "110010000",
                 /* Z */ "011010000",
                 /* - */ "010000101",
                 /* . */ "110000100",
                 /*' '*/ "011000100",
                 /* $ */ "010101000",
                 /* / */ "010100010",
                 /* + */ "010001010",
                 /* % */ "000101010",
                 /* * */ "010010100"
             };

        /// <summary>
        /// 条码文本,最后一次调用时的BarCode
        /// </summary>
        public string BarCodeText { get; set; }

        /// <summary>
        ///
        /// </summary>
        public StringFormat StringFormat { get; set; }

        private int _leftMargin = 5;
        private int _topMargin;
        private int _thickLength = 2;
        private int _narrowLength = 1;
        private int _barCodeHeight = 35;

        /// <summary>
        ///
        /// </summary>
        public int ThickLength
        {
            get { return _thickLength; }
            set { _thickLength = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int NarrowLength
        {
            get { return _narrowLength; }
            set { _narrowLength = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int TopMargin
        {
            get { return _topMargin; }
            set { _topMargin = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int LeftMargin
        {
            get { return _leftMargin; }
            set { _leftMargin = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int BarCodeHeight
        {
            get { return _barCodeHeight; }
            set { _barCodeHeight = value; }
        }

        /// <summary>
        /// 生成条码Bitmap,自定义条码高度,自定义文字对齐样式
        /// </summary>
        /// <returns></returns>
        public Bitmap Create()
        {
            if (BarCodeText == null) throw new ImageException();

            int intSourceLength = BarCodeText.Length;
            string strEncode = "010010100"; //添加起始码“*”.

            var sourceCode = BarCodeText.ToUpper();
            Bitmap objBitmap = new Bitmap(((_thickLength * 3 + _narrowLength * 7) * (intSourceLength + 2)) +
                                          (_leftMargin * 2), _barCodeHeight + (_topMargin * 2));
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            objGraphics.FillRectangle(Brushes.White, 0, 0, objBitmap.Width, objBitmap.Height);
            for (int i = 0; i < intSourceLength; i++)
            {
                //非法字符校验
                if (AlphaBet.IndexOf(sourceCode[i]) == -1 || sourceCode[i] == '*')
                {
                    objGraphics.DrawString("Invalid Bar Code", SystemFonts.DefaultFont, Brushes.Red, _leftMargin,
                        _topMargin);
                    return objBitmap;
                }
                //编码
                strEncode = string.Format("{0}0{1}", strEncode,
                    Code39[AlphaBet.IndexOf(sourceCode[i])]);
            }
            strEncode = string.Format("{0}0010010100", strEncode); //添加结束码“*”
            int intEncodeLength = strEncode.Length;
            for (int i = 0; i < intEncodeLength; i++) //绘制 Code39 barcode
            {
                int intBarWidth = strEncode[i] == '1' ? _thickLength : _narrowLength;
                objGraphics.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White, _leftMargin, _topMargin,
                    intBarWidth, _barCodeHeight);
                _leftMargin += intBarWidth;
            }
            //绘制明码
            Font barCodeTextFont = new Font("黑体", 10F);
            RectangleF rect = new RectangleF(2, _barCodeHeight - 20, objBitmap.Width - 4, 20);
            objGraphics.FillRectangle(Brushes.White, rect);
            //文本对齐
            if (StringFormat == null)
                objGraphics.DrawString(sourceCode, barCodeTextFont, Brushes.Black, rect);
            else
                objGraphics.DrawString(BarCodeText, barCodeTextFont, Brushes.Black, rect, StringFormat);
            return objBitmap;
        }
    }
}