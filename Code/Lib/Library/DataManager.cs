using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Library.Annotations;
using Library.HelperUtility;

namespace Library.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DataManager
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
        protected Type ObjectType;
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
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual object GetValue(string name)
        {
            var property = Properties[name];
            if (property == null && NameIgnoreCase)
            {
                property = Properties.OfType<PropertyDescriptor>().FirstOrDefault(n => string.Equals(n.Name, name, StringComparison.OrdinalIgnoreCase));
            }
            if (property == null) throw new LibException(string.Format("[{0}] not exist", name));
            return property.GetValue(Current);
        }



        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnPositionChanged()
        {
            var handler = PositionChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }


    }
}
