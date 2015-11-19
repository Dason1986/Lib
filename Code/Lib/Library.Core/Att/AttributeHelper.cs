using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Library.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    public static class AttributeHelper
    {
#if !SILVERLIGHT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributes<T>(object obj) where T : Attribute
        {
            AttributeCollection attributes = TypeDescriptor.GetAttributes(obj, true);
            return attributes.OfType<T>();
        }   /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAttribute<T>(object obj) where T : Attribute
        {
            AttributeCollection attributes = TypeDescriptor.GetAttributes(obj, true);
            return attributes.OfType<T>().FirstOrDefault();
        }
#endif
    


    
        /// <summary>
        /// 取成员属性
        /// </summary>
        /// <param name="member"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributes<T>(this MemberInfo member) where T : Attribute
        {

            var ef = member.GetCustomAttributes(true);

            return ef.OfType<T>();
        }
        /// <summary>
        /// 取成员属性
        /// </summary>
        /// <param name="member"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAttribute<T>(this Assembly member) where T : Attribute
        {

            var ef = member.GetAttributes<T>();
            if (ef != null && ef.Any()) return ef.FirstOrDefault();

            return null;
        }
        /// <summary>
        /// 取成员属性
        /// </summary>
        /// <param name="ass"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributes<T>(this Assembly ass) where T : Attribute
        {

            var ef = ass.GetCustomAttributes(true);

            return ef.OfType<T>();
        }
        /// <summary>
        /// 取成员属性
        /// </summary>
        /// <param name="member"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributes<T>(this Type member) where T : Attribute
        {

            var ef = member.GetCustomAttributes(true);
            return ef.OfType<T>();
        }
        /// <summary>
        /// 取成员属性
        /// </summary>
        /// <param name="member"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type member) where T : Attribute
        {

            var ef = member.GetAttributes<T>();
            if (ef != null && ef.Any()) return ef.FirstOrDefault();

            return null;
        }
        /// <summary>
        /// 取成员属性
        /// </summary>
        /// <param name="member"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAttribute<T>(this MemberInfo member) where T : Attribute
        {
            var ef = member.GetAttributes<T>();
            if (ef != null && ef.Any()) return ef.FirstOrDefault();

            return null;
        }
    }
}