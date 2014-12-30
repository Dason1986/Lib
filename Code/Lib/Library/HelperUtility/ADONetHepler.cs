using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Library.Annotations;

namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    public static class ADONetHepler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Cast<T>(this DataRow row, string name, T defaultValue)
        {
            if (row == null||!row.Table.Columns.Contains(name)) return defaultValue;
           var value= row[name];
            return ObjectUtility.TryCast(value, defaultValue);
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static IList<T> GetList<T>([NotNull] this System.Data.DataTable table) where T : class,new()
        {
            if (table == null) throw new ArgumentNullException("table");
            return GetList<T>(table.CreateDataReader());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DataException"></exception>
        [NotNull]
        public static IList<T> GetList<T>([NotNull] this System.Data.IDataReader dataReader) where T : class,new()
        {
            if (dataReader == null) throw new ArgumentNullException("dataReader");
            if (dataReader.IsClosed) throw new DataException("dataReader is close");
            if (dataReader.Depth == -1 || dataReader.FieldCount == 0) throw new DataException("dataReader 無效");
            List<T> list = new List<T>();
            Dictionary<int, PropertyInfo> reInfos = new Dictionary<int, PropertyInfo>();
            var type = typeof(T);
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                var item = type.GetProperty(dataReader.GetName(i));
                if (item == null) continue;
                reInfos.Add(i, item);
            }

            while (dataReader.Read())
            {
                T item = new T();
                list.Add(item);

                IEditableObject itemEditableObject = null;
                if (item is IEditableObject)
                {
                    itemEditableObject = item as IEditableObject;
                    itemEditableObject.BeginEdit();
                }
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    var val = dataReader.GetValue(i);
                    if (DBNull.Value == val) continue;
                    var info = reInfos[i];
                    info.SetValue(item, ObjectUtility.Cast(val, info.PropertyType), null);
                }
                if (item is IRevertibleChangeTracking)
                {
                    var itemRevertibleChangeTracking = item as IRevertibleChangeTracking;
                    itemRevertibleChangeTracking.AcceptChanges();
                }
                if (itemEditableObject != null)
                {
                    itemEditableObject.EndEdit();
                }
            }

            return list;
        }
    }
}
