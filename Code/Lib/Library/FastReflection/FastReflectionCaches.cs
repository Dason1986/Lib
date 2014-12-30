using System.Reflection;

namespace Library.FastReflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class FastReflectionCaches
    {
        static FastReflectionCaches()
        {
            MethodInvokerCache = new MethodInvokerCache();
            PropertyAccessorCache = new PropertyAccessorCache();
            FieldAccessorCache = new FieldAccessorCache();
            ConstructorInvokerCache = new ConstructorInvokerCache();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IFastReflectionCache<MethodInfo, IMethodInvoker> MethodInvokerCache { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static IFastReflectionCache<PropertyInfo, IPropertyAccessor> PropertyAccessorCache { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static IFastReflectionCache<FieldInfo, IFieldAccessor> FieldAccessorCache { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static IFastReflectionCache<ConstructorInfo, IConstructorInvoker> ConstructorInvokerCache { get; set; }
    }
}
