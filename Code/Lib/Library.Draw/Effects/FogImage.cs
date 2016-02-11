using Library.Att;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// ��Ч��
    /// ͼ����������ǻ���ͼ�������ص�֮��ļ���,���Ǹ�ͼ�����ص���ɫֵ����һ�������ֵ,
    /// ʹͼ�����ë������ˮ����Ч��..
    /// </summary>
    [LanguageDescription("�Աȶ�"), LanguageDisplayName("��Ч��")]
    public class FogImage : ImageBuilder
    {
        /*
         * ��ÿ������A(i,j)���д���������Χһ����Χ�������A(i+d,j+d),(-k<d<k)�������������Ȼ���Ըõ�ΪԲ�ĵ�Բ�뾶Խ������Ч��Խ���ԡ�
         */

        /// <summary>
        /// Բ�돽
        /// </summary>
        [LanguageDescription("Բ�돽"), LanguageDisplayName("Բ�돽"), Category("�V�R�x�")]
        public float Fog
        {
            get
            {
                InitOption();
                return _opetion.Value;
            }
            set
            {
                InitOption();
                _opetion.Value = value;
            }
        }

        #region Option

        /// <summary>
        ///
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ValueOption();
        }

        private ValueOption _opetion;

        /// <summary>
        ///
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ValueOption == false) throw new ImageException("Opetion is not ValueOption");
                _opetion = (ValueOption)value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new ValueOption();
        }

        #endregion Option

        #region Process

        #region IImageProcessable ��Ա

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            Random rnd = new Random();
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    int k = rnd.Next(-12345, 12345);
                    //���ؿ��С
                    int dx = x + k % 7;
                    int dy = y + k % 7;
                    //�������
                    if (dx >= width)
                        dx = width - 1;
                    if (dy >= height)
                        dy = height - 1;
                    if (dx < 0)
                        dx = 0;
                    if (dy < 0)
                        dy = 0;

                    Color c1 = bmp.GetPixel(dx, dy);
                    bmp.SetPixel(x, y, c1);
                }
            }
            return bmp;
        }

        #endregion IImageProcessable ��Ա

        #region IImageProcessable ��Ա

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override unsafe Image UnsafeProcessBitmap()
        {
            //    throw new NotImplementedException();
            var n = Fog == 0 ? 7 : Fog;
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            Random rnd = new Random();
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    int k = rnd.Next(-12345, 12345);
                    //���ؿ��С ����N�Ĵ�С������ģ����
                    int dj = j + (int)(k % n); //ˮƽ���ҷ�������ƫ�ƺ�
                    int di = i + (int)(k % n); //��ֱ���·�������ƫ�ƺ�
                    if (dj >= width) dj = width - 1;
                    if (di >= height) di = height - 1;
                    if (di < 0)
                        di = 0;
                    if (dj < 0)
                        dj = 0;
                    //���Format32bppArgb��ʽ���أ�ָ��ƫ����Ϊ4�ı��� 4*dj  4*di
                    //g(i,j)=f(di,dj)
                    ptr[bmpData.Stride * i + j * 4 + 0] = ptr[bmpData.Stride * di + dj * 4 + 0]; //B
                    ptr[bmpData.Stride * i + j * 4 + 1] = ptr[bmpData.Stride * di + dj * 4 + 1]; //G
                    ptr[bmpData.Stride * i + j * 4 + 2] = ptr[bmpData.Stride * di + dj * 4 + 2]; //R
                    // ptr += 4;  ע��˴�ָ��û�ƶ���ʼ����bmpData.Scan0��ʼ
                }
                //  ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        #endregion IImageProcessable ��Ա

        #endregion Process
    }
}