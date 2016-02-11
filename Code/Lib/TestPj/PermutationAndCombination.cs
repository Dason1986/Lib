using System.Collections.Generic;

namespace TestPj
{
    public class PermutationAndCombination<T>
    {
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="a">����1</param>
        /// <param name="b">����2</param>
        public static void Swap(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// �ݹ��㷨������(˽�г�Ա)
        /// </summary>
        /// <param name="list">���ص��б�</param>
        /// <param name="t">��������</param>
        /// <param name="startIndex">��ʼ���</param>
        /// <param name="endIndex">�������</param>
        private static void GetPermutation(ref List<T[]> list, T[] t, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
            {
                if (list == null)
                {
                    list = new List<T[]>();
                }
                T[] temp = new T[t.Length];
                t.CopyTo(temp, 0);
                list.Add(temp);
            }
            else
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    Swap(ref t[startIndex], ref t[i]);
                    GetPermutation(ref list, t, startIndex + 1, endIndex);
                    Swap(ref t[startIndex], ref t[i]);
                }
            }
        }

        /// <summary>
        /// �����ʼ��ŵ�������ŵ����У�����Ԫ�ز���
        /// </summary>
        /// <param name="t">��������</param>
        /// <param name="startIndex">��ʼ���</param>
        /// <param name="endIndex">�������</param>
        /// <returns>����ʼ��ŵ�����������еķ���</returns>
        public static List<T[]> GetPermutation(T[] t, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex > t.Length - 1)
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            GetPermutation(ref list, t, startIndex, endIndex);
            return list;
        }

        /// <summary>
        /// ������������Ԫ�ص�ȫ����
        /// </summary>
        /// <param name="t">��������</param>
        /// <returns>ȫ���еķ���</returns>
        public static List<T[]> GetPermutation(T[] t)
        {
            return GetPermutation(t, 0, t.Length - 1);
        }

        /// <summary>
        /// ��������n��Ԫ�ص�����
        /// </summary>
        /// <param name="t">��������</param>
        /// <param name="n">Ԫ�ظ���</param>
        /// <returns>������n��Ԫ�ص�����</returns>
        public static List<T[]> GetPermutation(T[] t, int n)
        {
            if (n > t.Length)
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            List<T[]> c = GetCombination(t, n);
            for (int i = 0; i < c.Count; i++)
            {
                List<T[]> l = new List<T[]>();
                GetPermutation(ref l, c[i], 0, n - 1);
                list.AddRange(l);
            }
            return list;
        }

        /// <summary>
        /// ��������n��Ԫ�ص����
        /// </summary>
        /// <param name="t">��������</param>
        /// <param name="n">Ԫ�ظ���</param>
        /// <returns>������n��Ԫ�ص���ϵķ���</returns>
        public static List<T[]> GetCombination(T[] t, int n)
        {
            if (t.Length < n)
            {
                return null;
            }
            int[] temp = new int[n];
            List<T[]> list = new List<T[]>();
            GetCombination(ref list, t, t.Length, n, temp, n);
            return list;
        }

        /// <summary>
        /// �ݹ��㷨����������(˽�г�Ա)
        /// </summary>
        /// <param name="list">���صķ���</param>
        /// <param name="t">��������</param>
        /// <param name="n">��������</param>
        /// <param name="m">��������</param>
        /// <param name="b">��������</param>
        /// <param name="M">��������M</param>
        private static void GetCombination(ref List<T[]> list, T[] t, int n, int m, int[] b, int M)
        {
            for (int i = n; i >= m; i--)
            {
                b[m - 1] = i - 1;
                if (m > 1)
                {
                    GetCombination(ref list, t, i - 1, m - 1, b, M);
                }
                else
                {
                    if (list == null)
                    {
                        list = new List<T[]>();
                    }
                    T[] temp = new T[M];
                    for (int j = 0; j < b.Length; j++)
                    {
                        temp[j] = t[b[j]];
                    }
                    list.Add(temp);
                }
            }
        }
    }
}