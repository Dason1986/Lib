using System;
using System.Reflection;
using Library.FastReflection;

namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    public static class FastReflectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="instance"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object FastInvoke(this MethodInfo methodInfo, object instance, params object[] parameters)
        {
            return FastReflectionCaches.MethodInvokerCache.Get(methodInfo).Invoke(instance, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public static void FastSetValue(this PropertyInfo propertyInfo, object instance, object value)
        {
            FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).SetValue(instance, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static object FastGetValue(this PropertyInfo propertyInfo, object instance)
        {
            return FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).GetValue(instance);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T FastGetValue<T>(this PropertyInfo propertyInfo, object instance)
        {
            var obj = propertyInfo.FastGetValue(instance);
            return ObjectUtility.Cast<T>(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static object FastGetValue(this FieldInfo fieldInfo, object instance)
        {
            return FastReflectionCaches.FieldAccessorCache.Get(fieldInfo).GetValue(instance);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T FastGetValue<T>(this FieldInfo fieldInfo, object instance)
        {
            var obj = fieldInfo.FastGetValue(instance);
            return ObjectUtility.Cast<T>(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object FastInvoke(this ConstructorInfo constructorInfo, params object[] parameters)
        {
            return FastReflectionCaches.ConstructorInvokerCache.Get(constructorInfo).Invoke(parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T FastInvoke<T>(this ConstructorInfo constructorInfo, params object[] parameters)
        {
            return typeof (T).IsAssignableFrom(constructorInfo.DeclaringType)
                       ? (T) FastInvoke(constructorInfo, parameters)
                       : default(T);
        }

        /// <summary>
        ///  创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>(this Type type)
        {
            return typeof (T).IsAssignableFrom(type) ? (T) CreateInstance(type) : default(T);
        }

        /// <summary>
        ///  创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type type)
        {
            if (type == null)
                return null;
            var iConstructor = type.GetConstructor(new Type[] { });
            if (iConstructor == null)throw new NotSupportedException("無構造函數，無法創建對象");
            var obj = iConstructor.FastInvoke();
            return obj;
        }
        /// <summary>
        ///  创建对象
        /// </summary> 
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            return CreateInstance<T>(typeof(T));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parmTypes"></param>
        /// <param name="parms"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T CreateInstance<T>(this Type type, Type[] parmTypes, object[] parms)
        {
            if (type == null || !typeof(T).IsAssignableFrom(type))
                return default(T);
            // throw new ArgumentException(string.Format("[{0}]类不是从[{1}]类派生.", type.FullName, typeof(T).FullName));
            var iConstructor = type.GetConstructor(parmTypes);
            var obj = iConstructor.FastInvoke(parms);
            return (T)obj;
        }
    }
}
