using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace Library
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceManagement
    {
        private static readonly Dictionary<Type, ResourceManager> cache = new Dictionary<Type, ResourceManager>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="resourceType"></param>
        public static void AddRegister(Type resourceType)
        {
            if (cache.ContainsKey(resourceType)) return;
            cache.Add(resourceType, new ResourceManager(resourceType));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public static ResourceManager GetManager(Type resourceType)
        {
            if (cache.ContainsKey(resourceType)) return cache[resourceType];
            var maget = new ResourceManager(resourceType);
            cache.Add(resourceType, maget);
            return maget;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetString(Type resourceType, string name)
        {
            var manager = GetManager(resourceType);
            if (manager == null) return name;
            var str = manager.GetString(name);
            return string.IsNullOrEmpty(str) ? name : str;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="name"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static string GetString(Type resourceType, string name, CultureInfo cultureInfo)
        {
            var manager = GetManager(resourceType);
            if (manager == null) return name;
            var str = manager.GetString(name, cultureInfo);
            return string.IsNullOrEmpty(str) ? name : str;
        }
    }
}