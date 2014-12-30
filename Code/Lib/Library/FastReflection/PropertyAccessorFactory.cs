using System.Reflection;

namespace Library.FastReflection
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyAccessorFactory : IFastReflectionFactory<PropertyInfo, IPropertyAccessor>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IPropertyAccessor Create(PropertyInfo key)
        {
            return new PropertyAccessor(key);
        }

        #region IFastReflectionFactory<PropertyInfo,IPropertyAccessor> Members

        IPropertyAccessor IFastReflectionFactory<PropertyInfo, IPropertyAccessor>.Create(PropertyInfo key)
        {
            return this.Create(key);
        }

        #endregion
    }
}
