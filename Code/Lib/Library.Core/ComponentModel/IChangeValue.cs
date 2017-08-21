using System.Text;

namespace Library.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChangeValue
    {
        /// <summary>
        /// 
        /// </summary>
        string PropertyName { get; }
   
        /// <summary>
        /// 
        /// </summary>
        object OldValue { get; }
        /// <summary>
        /// 
        /// </summary>
        object NewValue { get; }
    }
    /// <summary>
    /// 变更值
    /// </summary>
    public struct ChangeValue : IChangeValue
    {
        /// <summary>
        /// 初始化变更值
        /// </summary>
        /// <param name="propertyName">属性名</param> 
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        public ChangeValue(string propertyName,   object oldValue, object newValue)
        {
            PropertyName = propertyName; 
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; private set; } 
        /// <summary>
        /// 旧值
        /// </summary>
        public object OldValue { get; private set; }
        /// <summary>
        /// 新值
        /// </summary>
        public object NewValue { get; private set; }



        /// <summary>
        /// 输出变更信息
        /// </summary>
        public override string ToString()
        {
            var result = new StringBuilder();
        
            result.AppendFormat("旧值:{0},新值:{1}", OldValue, NewValue);
            return result.ToString();
        }
    }
    /// <summary>
    /// 变更值
    /// </summary>
    public struct ChangeValue<T> : IChangeValue
    {
        /// <summary>
        /// 初始化变更值
        /// </summary>
        /// <param name="propertyName">属性名</param> 
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        public ChangeValue(string propertyName,  T oldValue, T newValue)
        {
            PropertyName = propertyName; 
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; private set; } 
        /// <summary>
        /// 旧值
        /// </summary>
        public T OldValue { get; private set; }
        /// <summary>
        /// 新值
        /// </summary>
        public T NewValue { get; private set; }

        object IChangeValue.OldValue { get { return this.OldValue; } }

        object IChangeValue.NewValue { get { return this.NewValue; } }

        /// <summary>
        /// 输出变更信息
        /// </summary>
        public override string ToString()
        {
            var result = new StringBuilder(); 
            result.AppendFormat("旧值:{0},新值:{1}", OldValue, NewValue);
            return result.ToString();
        }
    }
}