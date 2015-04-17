using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Library.FastReflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        object GetValue(object instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        void SetValue(object instance, object value);
    }

    /// <summary>
    /// 
    /// </summary>
    public class PropertyAccessor : IPropertyAccessor
    {
        private Func<object, object> _mGetter;
        private MethodInvoker _mSetMethodInvoker;

        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
            this.InitializeGet(propertyInfo);
            this.InitializeSet(propertyInfo);
        }

        private void InitializeGet(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead) return;

            // Target: (object)(((TInstance)instance).Property)

            // preparing parameter, object type
            var instance = Expression.Parameter(typeof(object), "instance");

            // non-instance for static method, or ((TInstance)instance)
            var instanceCast = propertyInfo.GetGetMethod(true).IsStatic ? null :
                Expression.Convert(instance, propertyInfo.ReflectedType);

            // ((TInstance)instance).Property
            var propertyAccess = Expression.Property(instanceCast, propertyInfo);

            // (object)(((TInstance)instance).Property)
            var castPropertyValue = Expression.Convert(propertyAccess, typeof(object));

            // Lambda expression
            var lambda = Expression.Lambda<Func<object, object>>(castPropertyValue, instance);

            this._mGetter = lambda.Compile();
        }

        private void InitializeSet(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite) return;
            this._mSetMethodInvoker = new MethodInvoker(propertyInfo.GetSetMethod(true));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object GetValue(object o)
        {
            if (this._mGetter == null)
            {
                throw new NotSupportedException("Get method is not defined for this property.");
            }

            return this._mGetter(o);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="value"></param>
        public void SetValue(object o, object value)
        {
            if (this._mSetMethodInvoker == null)
            {
                throw new NotSupportedException("Set method is not defined for this property.");
            }

            this._mSetMethodInvoker.Invoke(o, new object[] { value });
        }

        #region IPropertyAccessor Members

        object IPropertyAccessor.GetValue(object instance)
        {
            return this.GetValue(instance);
        }

        void IPropertyAccessor.SetValue(object instance, object value)
        {
            this.SetValue(instance, value);
        }

        #endregion
    }
}
