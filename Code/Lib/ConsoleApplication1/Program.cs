using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var times = 59999;
            DateTime time = DateTime.Now;
            for (int i = 0; i < times; i++)
            {
                List<int> list = NewMethod();
                OddEvenSort(list);
            }
            Console.WriteLine(time - DateTime.Now);

            time = DateTime.Now;
            for (int i = 0; i < times; i++)
            {
                List<int> list = NewMethod();

                BubbleSort(list);
            }
            Console.WriteLine(time - DateTime.Now);

            time = DateTime.Now;
            for (int i = 0; i < times; i++)
            {
                List<int> list = NewMethod();

                CombSort(list);
            }
            Console.WriteLine(time - DateTime.Now);

            time = DateTime.Now;
            for (int i = 0; i < times; i++)
            {
                List<int> list = NewMethod();
                CockTailSort(list);

            }
            Console.WriteLine(time - DateTime.Now);



            Console.Read();
        }

        private static List<int> NewMethod()
        {
            return new List<int> { 1165, 1255, 2434, 2847, 133, 245, 356, 2467, 5372, 6158, 679, 347, 165,155, 244, 287, 33, 45, 56, 267, 372, 158, 69, 347, 923, 185, 476, 587, 673,755, 346, 117, 23, 125, 236, 377 ,65, 55, 44, 87, 3, 5, 6, 27, 32, 15, 6, 37, 93, 85, 76, 87, 73, 55, 46, 17, 3, 25, 36, 77 };
        }

        /// <summary>
        /// 奇偶排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static IList<int> OddEvenSort(IList<int> list)
        {
            var isSorted = false;

            //如果还没有排序完，就需要继续排序，知道没有交换为止
            while (!isSorted)
            {
                //先默认已经排序完了
                isSorted = true;

                //先进行 奇数位 排序
                for (int i = 0; i < list.Count; i = i + 2)
                {
                    //如果 前者 大于 后者，则需要进行交换操作,也要防止边界
                    if (i + 1 < list.Count && list[i] > list[i + 1])
                    {
                        var temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;

                        //说明存在过排序，还没有排序完
                        isSorted = false;
                    }
                }

                //再进行 奇数位 排序
                for (int i = 1; i < list.Count; i = i + 2)
                {
                    //如果 前者 大于 后者，则需要进行交换操作，也要防止边界
                    if (i + 1 < list.Count && list[i] > list[i + 1])
                    {
                        var temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;

                        //说明存在过排序，还没有排序完
                        isSorted = false;
                    }
                }
            }

            return list;
        }
        /// <summary>
        /// 泡沫排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static IList<int> BubbleSort(IList<int> list)
        {
            // 取长度最长的词组 -- 冒泡法
            for (int j = 1; j < list.Count; j++)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    // 如果 myArray[i] < myArray[i+1] ，则 myArray[i] 下沉一位
                    if (list[i] < list[i + 1])
                    {
                        int temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 梳排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static IList<int> CombSort(IList<int> list)
        {
            //获取最佳排序尺寸： 比率为 1.3
            var step = (int)Math.Floor(list.Count / 1.3);

            while (step >= 1)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    //如果前者大于后者，则进行交换
                    if (i + step < list.Count && list[i] > list[i + step])
                    {
                        var temp = list[i];

                        list[i] = list[i + step];

                        list[i + step] = temp;
                    }

                    //如果越界，直接跳出
                    if (i + step > list.Count)
                        break;
                }

                //在当前的step在除1.3
                step = (int)Math.Floor(step / 1.3);
            }

            return list;
        }

        /// <summary>
        /// 鸡尾酒排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static IList<int> CockTailSort(IList<int> list)
        {
            //判断是否已经排序了
            var isSorted = false;

            //因为是双向比较，所以比较次数为原来数组的1/2次即可。
            for (int i = 1; i <= list.Count / 2; i++)
            {
                //从前到后的排序 (升序)
                for (int m = i - 1; m <= list.Count - i; m++)
                {
                    //如果前面大于后面，则进行交换
                    if (m + 1 < list.Count && list[m] > list[m + 1])
                    {
                        var temp = list[m];

                        list[m] = list[m + 1];

                        list[m + 1] = temp;

                        isSorted = true;
                    }
                }

                //   Console.WriteLine("正向排序 => {0}", string.Join(",", list));

                //从后到前的排序（降序）
                for (int n = list.Count - i - 1; n >= i; n--)
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

            return list;
        }
    }
}