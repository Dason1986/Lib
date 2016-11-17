using Library.Annotations;
using Library.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection;

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
        /// <param name="reader"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static MicroDataTable FillDataTable(this IDataReader reader, int pageIndex, int pageSize)
        {
            bool defined = false;

            MicroDataTable table = new MicroDataTable();

            int index = 0;
            int beginIndex = pageSize * pageIndex;
            int endIndex = pageSize * (pageIndex + 1) - 1;

            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];

                if (!defined)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        MicroDataColumn column = new MicroDataColumn()
                        {
                            ColumnName = reader.GetName(i),
                            ColumnType = reader.GetFieldType(i)
                        };

                        table.Columns.Add(column);
                    }

                    defined = true;
                }

                if (index >= beginIndex && index <= endIndex)
                {
                    reader.GetValues(values);

                    table.Rows.Add(new MicroDataRow(table.Columns, values));
                }

                index++;
            }

            table.TotalCount = index;

            return table;
        }
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
            if (row == null || !row.Table.Columns.Contains(name)) return defaultValue;
            var value = row[name];
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
        public static IList<T> GetList<T>([NotNull] this System.Data.DataTable table) where T : class, new()
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
        public static IList<T> GetList<T>([NotNull] this System.Data.IDataReader dataReader) where T : class, new()
        {
            if (dataReader == null) throw new ArgumentNullException("dataReader");
            if (dataReader.IsClosed) throw new DataException("dataReader is close");
            if (dataReader.Depth == -1 || dataReader.FieldCount == 0) throw new DataException("dataReader 無效");
            List<T> list = new List<T>();
            var reInfos = GetPropertyInfos(typeof(T), dataReader);

            while (dataReader.Read())
            {
                T item = new T();
                list.Add(item);
                object obj = item;
                GetValue(dataReader, ref obj, reInfos);
            }

            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetEntity(this IDataReader dataReader, Type type)
        {
            if (dataReader == null) throw new ArgumentNullException("dataReader");
            if (dataReader.IsClosed) throw new DataException("dataReader is close");
            if (dataReader.Depth == -1 || dataReader.FieldCount == 0) throw new DataException("dataReader 無效");
            if (!dataReader.Read()) return null;
            var reInfos = GetPropertyInfos(type, dataReader);

            object item = type.CreateInstance();
            if (item == null) throw new Exception("type can not Instance");

            GetValue(dataReader, ref item, reInfos);

            return item;
        }

        private static Dictionary<int, PropertyInfo> GetPropertyInfos(Type type, IDataReader dataReader)
        {
            Dictionary<int, PropertyInfo> reInfos = new Dictionary<int, PropertyInfo>();
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                var itemProperty = type.GetProperty(dataReader.GetName(i));
                if (itemProperty == null) continue;
                reInfos.Add(i, itemProperty);
            }
            return reInfos;
        }

        private static void GetValue(IDataReader dataReader, [NotNull] ref object item,
            [NotNull] Dictionary<int, PropertyInfo> reInfos)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (reInfos == null) throw new ArgumentNullException("reInfos");
            IEditableObject itemEditableObject = null;
            if (item is IEditableObject)
            {
                itemEditableObject = item as IEditableObject;
                itemEditableObject.BeginEdit();
            }
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                var val = dataReader.GetValue(i);
                if (DBNull.Value == val || val == null) continue;
                var info = reInfos[i];
                info.FastSetValue(item, ObjectUtility.Cast(val, info.PropertyType));
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
            //  return item;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static DbConnection Clone(this DbConnection connection)
        {
            if (connection == null) return null;
            if (connection is ICloneable)
            {
                return ((ICloneable)connection).Clone() as DbConnection;
            }
            var factory = GetDbProviderFactory(connection);
            var newconnection = factory.CreateConnection();
            if (newconnection != null)
            {
                newconnection.ConnectionString = connection.ConnectionString;
                return newconnection;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(DbConnection connection)
        {
            var propertyinfo = connection.GetType().GetProperty("DbProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyinfo == null) return null;
            var obj = propertyinfo.GetValue(connection, null);
            if (obj is DbProviderFactory) return (DbProviderFactory)obj;
            return null;
        }
    }
}