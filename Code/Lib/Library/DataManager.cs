using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Library.Annotations;
using Library.ComponentModel;
using Microsoft.Win32;
using Library.HelperUtility;

namespace Library.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DataManager : System.Data.IDataRecord
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datasource"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DataManager([NotNull] object datasource)
        {
            if (datasource == null) throw new ArgumentNullException("datasource");
            if (datasource is IList == false && datasource is IListSource == false) throw new Exception("not data IList<T> or IListSource");
            GetList(datasource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datasource"></param>
        protected virtual void GetList(object datasource)
        {
            if (datasource is IListSource)
            {
                List = ((IListSource)datasource).GetList();
                ObjectType = List.GetType();
            }
            else
            {
                List = (IList)datasource;
                ObjectType = TypeHelper.GetListGenericType(List.GetType());
            }
            Properties = TypeHelper.GetListItemProperties(List);
            if (List.Count > 0) Position = 0;
        }

        //
        // 摘要:
        //     取得基礎清單的屬性描述項集合。
        //
        // 傳回:
        //     清單的 System.ComponentModel.PropertyDescriptorCollection。
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public PropertyDescriptorCollection Properties { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public IList List { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public Type ObjectType { get;protected set; }
        private int _position = -1;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler PositionChanged;

        /// <summary>
        /// 
        /// </summary>
        public virtual int Count { get { return List.Count; } }

        /// <summary>
        /// 
        /// </summary>
        public virtual object Current { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        public int Position
        {
            get { return _position; }
            set
            {
                if (value < 0 || value > List.Count - 1) throw new IndexOutOfRangeException("index");
                _position = value;
                Current = List[value];
                OnPositionChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NameIgnoreCase { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            var value = Position + 1;
            if (value < 0 || value > List.Count - 1) return false;
            Position = value;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnPositionChanged()
        {
            var handler = PositionChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }


        /// <summary>
        /// 
        /// </summary>
        public int FieldCount
        {
            get { return Properties.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool GetBoolean(int i)
        {
            return ObjectUtility.Cast<bool>(GetValue(i));

        }

        private PropertyDescriptor getDescriptor(int index)
        {
            if (index < 0 || index > Properties.Count - 1) throw new LibException("", new IndexOutOfRangeException());
            return Properties[index];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public byte GetByte(int i)
        {
            return ObjectUtility.Cast<byte>(GetValue(i));
        }

        long System.Data.IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public char GetChar(int i)
        {
            return ObjectUtility.Cast<char>(GetValue(i));
        }

        long System.Data.IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        System.Data.IDataReader System.Data.IDataRecord.GetData(int i)
        {
            throw new NotImplementedException();
        }

        string System.Data.IDataRecord.GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DateTime GetDateTime(int i)
        {
            return ObjectUtility.Cast<DateTime>(GetValue(i));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public decimal GetDecimal(int i)
        {
            return ObjectUtility.Cast<decimal>(GetValue(i));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double GetDouble(int i)
        {
            return ObjectUtility.Cast<double>(GetValue(i));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Type GetFieldType(int i)
        {
            return Properties[i].PropertyType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float GetFloat(int i)
        {
            return ObjectUtility.Cast<float>(GetValue(i));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Guid GetGuid(int i)
        {
            return ObjectUtility.Cast<Guid>(GetValue(i));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public short GetInt16(int i)
        {
            return ObjectUtility.Cast<short>(GetValue(i));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int GetInt32(int i)
        {
            return ObjectUtility.Cast<int>(GetValue(i));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public long GetInt64(int i)
        {
            return ObjectUtility.Cast<long>(GetValue(i));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetName(int i)
        {
            return getDescriptor(i).Name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetOrdinal(string name)
        {
            var property = Properties[name];
            if (property != null) return Properties.IndexOf(property);
            if (!NameIgnoreCase) throw new LibException(string.Format("[{0}] not exist", name));
            for (int i = 0; i < Properties.Count; i++)
            {
                if (string.Equals(Properties[i].Name, name, StringComparison.OrdinalIgnoreCase)) return i;
            }
            throw new LibException(string.Format("[{0}] not exist", name));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetString(int i)
        {
            return ObjectUtility.Cast<string>(GetValue(i));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public object GetValue(int i)
        {
            var property = getDescriptor(i);
            var obj = property.GetValue(Current);
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public int GetValues(object[] values)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (values.Length == 0) return 0;
            if (Current == null) throw new LibException("item is empty");
            int count = values.Length;
            for (int i = 0; i < count; i++)
            {
                values[i] = Properties[i].GetValue(Current);
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool IsDBNull(int i)
        {
            var obj = GetValue(i);
            return DBNull.Value == obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                if (Current == null) throw new LibException("item is empty");
                var index = GetOrdinal(name);
                return GetValue(index);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public object this[int i]
        {
            get
            {
                if (Current == null) throw new LibException("item is empty");
                return GetValue(i);
            }
        }
    }
}
