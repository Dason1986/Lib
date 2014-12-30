using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Library.FastReflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFieldAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        object GetValue(object instance);
    }

    /// <summary>
    /// 
    /// </summary>
    public class FieldAccessor : IFieldAccessor
    {
        private readonly Func<object, object> _mGetter;

        /// <summary>
        /// 
        /// </summary>
        public FieldInfo FieldInfo { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        public FieldAccessor(FieldInfo fieldInfo)
        {
            this.FieldInfo = fieldInfo;
            this._mGetter = this.GetDelegate(fieldInfo);
        }

        private Func<object, object> GetDelegate(FieldInfo fieldInfo)
        {
            // target: (object)(((TInstance)instance).Field)

            // preparing parameter, object type
            var instance = Expression.Parameter(typeof(object), "instance");

            // non-instance for static method, or ((TInstance)instance)
            var instanceCast = fieldInfo.IsStatic ? null :
                Expression.Convert(instance, fieldInfo.ReflectedType);

            // ((TInstance)instance).Property
            var fieldAccess = Expression.Field(instanceCast, fieldInfo);

            // (object)(((TInstance)instance).Property)
            var castFieldValue = Expression.Convert(fieldAccess, typeof(object));

            // Lambda expression
            var lambda = Expression.Lambda<Func<object, object>>(castFieldValue, instance);

            return lambda.Compile();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object GetValue(object instance)
        {
            return this._mGetter(instance);
        }

        #region IFieldAccessor Members

        object IFieldAccessor.GetValue(object instance)
        {
            return this.GetValue(instance);
        }

        #endregion
    }
}
