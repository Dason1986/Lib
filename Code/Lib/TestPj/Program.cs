using Library.Draw.Code;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace TestPj
{
    internal class Program
    {
        public const string StrDate = "20101102";
        public static readonly int? Number1 = 5;
        public const string StrGUID = "e56f90f3-4622-4896-b7a3-fd94dafe33cc";
        public const int NumDate = 20101102;

        private static void Main(string[] args)
        {
            Console.ReadKey();
            Console.ReadLine();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            //logic message
            //lib
        }

        public static void TestWmf()
        {
            var path = @"C:\1.Wmf";

            Bitmap bmp = new Bitmap(220, 220);

            Graphics gs = Graphics.FromImage(bmp);
            Metafile metafile = new Metafile(path, gs.GetHdc());
            Graphics g = Graphics.FromImage(metafile);
            HatchBrush hb = new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Black, Color.White);

            g.FillEllipse(Brushes.Gray, 10f, 10f, 200, 200);
            g.DrawEllipse(new Pen(Color.Black, 1f), 10f, 10f, 200, 200);

            g.FillEllipse(hb, 30f, 95f, 30, 30);
            g.DrawEllipse(new Pen(Color.Black, 1f), 30f, 95f, 30, 30);

            g.FillEllipse(hb, 160f, 95f, 30, 30);
            g.DrawEllipse(new Pen(Color.Black, 1f), 160f, 95f, 30, 30);

            g.FillEllipse(hb, 95f, 30f, 30, 30);
            g.DrawEllipse(new Pen(Color.Black, 1f), 95f, 30f, 30, 30);

            g.FillEllipse(hb, 95f, 160f, 30, 30);
            g.DrawEllipse(new Pen(Color.Black, 1f), 95f, 160f, 30, 30);

            g.FillEllipse(Brushes.Blue, 60f, 60f, 100, 100);
            g.DrawEllipse(new Pen(Color.Black, 1f), 60f, 60f, 100, 100);

            g.FillEllipse(Brushes.BlanchedAlmond, 95f, 95f, 30, 30);
            g.DrawEllipse(new Pen(Color.Black, 1f), 95f, 95f, 30, 30);

            g.DrawRectangle(new Pen(System.Drawing.Brushes.Blue, 0.1f), 6, 6, 208, 208);

            g.DrawLine(new Pen(Color.Black, 0.1f), 110f, 110f, 220f, 25f);
            g.DrawString("剖面图", new Font("宋体", 9f), Brushes.Green, 220f, 20f);
            g.Save();
            g.Dispose();

            //  bmp.Save(;
        }

        private static void CreateImageCode()
        {
            ImageCode code = new ImageCode();
            code.ContentType = ImageCode.ContentTypeEnum.Char;
            code.Create();
            File.WriteAllBytes(String.Format(@"C:\ss\{0}.png", code.Code), code.Image);
        }

        private static void OddEvenSort(int[] list)
        {
            var isSorted = false;

            //如果还没有排序完，就需要继续排序，知道没有交换为止
            while (!isSorted)
            {
                //先默认已经排序完了
                isSorted = true;

                //先进行 奇数位 排序
                for (int i = 0; i < list.Length; i = i + 2)
                {
                    //如果 前者 大于 后者，则需要进行交换操作,也要防止边界
                    if (i + 1 < list.Length && list[i] > list[i + 1])
                    {
                        var temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;

                        //说明存在过排序，还没有排序完
                        isSorted = false;
                    }
                }

                //再进行 奇数位 排序
                for (int i = 1; i < list.Length; i = i + 2)
                {
                    //如果 前者 大于 后者，则需要进行交换操作，也要防止边界
                    if (i + 1 < list.Length && list[i] > list[i + 1])
                    {
                        var temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;

                        //说明存在过排序，还没有排序完
                        isSorted = false;
                    }
                }
            }
        }

        /// <summary>
        /// 鸡尾酒排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void CockTailSort(int[] list)
        {
            //判断是否已经排序了
            var isSorted = false;

            //因为是双向比较，所以比较次数为原来数组的1/2次即可。
            for (int i = 1; i <= list.Length / 2; i++)
            {
                //从前到后的排序 (升序)
                for (int m = i - 1; m <= list.Length - i; m++)
                {
                    //如果前面大于后面，则进行交换
                    if (m + 1 < list.Length && list[m] > list[m + 1])
                    {
                        var temp = list[m];

                        list[m] = list[m + 1];

                        list[m + 1] = temp;

                        isSorted = true;
                    }
                }

                //   Console.WriteLine("正向排序 => {0}", string.Join(",", list));

                //从后到前的排序（降序）
                for (int n = list.Length - i - 1; n >= i; n--)
                {
                    //如果前面大于后面，则进行交换
                    if (n > 0 && list[n - 1] > list[n])
                    {
                        var temp = list[n];

                        list[n] = list[n - 1];

                        list[n - 1] = temp;

                        isSorted = true;
                    }
                }

                //当不再有排序，提前退出
                if (!isSorted)
                    break;

                //  Console.WriteLine("反向排序 => {0}", string.Join(",", list));
            }
        }
    }
}