using System;
using System.Collections.Generic;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] RandomByDischarge(int min, int max, int count)
        {
            if (count < 1 || count > max - min + 1)
            {
                throw new IndexOutOfRangeException("Params is illegal.");
            }

            Random random = new Random();
            List<int> ret = new List<int>();
            int[] flag = new int[max - min + 1];

            while (ret.Count < count)
            {
                var rand = random.Next(max - min + 1) + min; //生成[m,n]之间的随机数
                if (flag[rand - min] == 0)
                {
                    ret.Add(rand);
                    flag[rand - min] = 1;
                }
            }

            return ret.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] RandomBySwap(int min, int max, int count)
        {
            if (count < 1 || count > max - min + 1)
            {
                throw new IndexOutOfRangeException("Params is illegal.");
            }

            Random random = new Random();
            List<int> ret = new List<int>();
            int[] arr = new int[max - min + 1];
            int j = min;
            for (int i = 0; i < max - min + 1; i++)
            {
                arr[i] = j++;
            }

            for (int i = max - min; i >= 0; i--)
            {
                int randIndex = random.Next(max - min + 1);
                int t = arr[randIndex];
                arr[randIndex] = arr[i];
                arr[i] = t;
            }

            for (int i = 0; i < count; i++)
            { //截取前k个
                ret.Add(arr[i]);
            }

            return ret.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] RandomByMove(int min, int max, int count)
        {
            if (count < 1 || count > max - min + 1)
            {
                throw new IndexOutOfRangeException("Params is illegal.");
            }

            Random random = new Random();
            List<int> ret = new List<int>();
            int[] arr = new int[max - min + 1];
            int j = min;
            for (int i = 0; i < max - min + 1; i++)
            {
                arr[i] = j++;
            }

            int cur = max - min + 1;

            while (cur > 0 && max - min + 1 - cur < count)
            {
                int randIndex = random.Next(cur);
                int randValue = arr[randIndex];
                ret.Add(randValue);
                for (int i = randIndex + 1; i < cur; i++)
                {
                    arr[i - 1] = arr[i];
                }
                arr[cur - 1] = randValue;
                cur--;
            }

            return ret.ToArray();
        }
    }
}