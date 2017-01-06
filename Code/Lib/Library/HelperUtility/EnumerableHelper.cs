using Library.Annotations;
using System;
using System.Collections;
using System.ComponentModel;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class EnumerableHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T[] AppendItem<T>([NotNull] this T[] source, T item)
        {
            if (source == null) throw new ArgumentNullException("source");
            T[] array = new T[source.Length + 1];
            source.CopyTo(array, 0);
            array[source.Length] = item;
            return array;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T[] AppendItems<T>([NotNull] this T[] source, [NotNull] params T[] items)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (items == null) throw new ArgumentNullException("items");
            T[] array = new T[source.Length + items.Length];
            Array.Copy(source, 0, array, 0, source.Length);
            source.CopyTo(array, 0);
            Array.Copy(items, 0, array, source.Length, items.Length);
            return array;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool HasRecord(this IListSource list, int count = 1)
        {
            if (list == null) return false;
            var tmplist = list.GetList();
            return tmplist.HasRecord(count);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool HasRecord(this IList list, int count = 1)
        {
            return list != null && list.Count >= count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool HasRecord(this ICollection list, int count = 1)
        {
            return list != null && list.Count >= count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool HasRecord(this Array list, int count = 1)
        {
            return list != null && list.Length >= count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool HasRecord(this IEnumerable list, int count = 1)
        {
            if (list == null) return false;
            var enumerator = list.GetEnumerator();
            int index = 0;
            while (enumerator.MoveNext())
            {
                index++;
                if (index >= count) return true;
            }
            return false;
        }
    }
}