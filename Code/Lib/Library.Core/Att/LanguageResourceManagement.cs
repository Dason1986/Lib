using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading;
using Library.Annotations;
using Library.HelperUtility;

namespace Library.Att
{
    /// <summary>
    /// 
    /// </summary>
    public static class LanguageResourceManagement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="resourceManager"></param>
        public static void AddResource([NotNull] string name, [NotNull] ResourceManager resourceManager)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (resourceManager == null) throw new ArgumentNullException("resourceManager");
            if (managers.Values.Any(n => n.BaseName == resourceManager.BaseName)) return;
            if (managers.ContainsKey(name)) return;
            managers.Add(name, resourceManager);
        }

        readonly static Dictionary<string, ResourceManager> managers = new Dictionary<string, ResourceManager>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetResourceNames()
        {
            return managers.Keys.ToArray();
        }

        private static ResourceManager getManager(string name)
        {
            if (managers.Count != 0) return managers[name];
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            AppDomain.CurrentDomain.AssemblyLoad += (x, y) => { y.LoadedAssembly.GetAttribute<LanguageRegisterAttribute>(); };
            foreach (var assembly in ass)
            {
                assembly.GetAttribute<LanguageRegisterAttribute>();

            }
            return managers[name];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string GetException(double resultCode, string resourceName)
        {
            if (resultCode == 0) return GetString("Unknown", "Global");
            string name = "_Code" + resultCode.ToString(CultureInfo.InvariantCulture).Replace(".", "_").Replace("-", "__");
            var tmpmsg = GetString(name,resourceName);
            return tmpmsg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public static string GetException(double resultCode)
        {
            return GetException(resultCode, "Global");

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        /// <returns></returns>
        public static string GetException(double resultCode, object[] formatages)
        {
            var str = GetException(resultCode);
            return string.Format(str, formatages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string GetException(double resultCode, object[] formatages, string resourceName)
        {
            var str = GetException(resultCode, resourceName);
            return string.Format(str, formatages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="name"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string GetString(string name, string resourceName, CultureInfo culture = null)
        {
            var str = getManager(resourceName).GetString(name, culture ?? Thread.CurrentThread.CurrentUICulture);
            return str ?? name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="name"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static Stream GetStream(string name, string resourceName, CultureInfo culture = null)
        {
            return getManager(resourceName).GetStream(name, culture ?? Thread.CurrentThread.CurrentUICulture);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="name"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static object GetObject(string name, string resourceName, CultureInfo culture = null)
        {
            return getManager(resourceName).GetObject(name, culture ?? Thread.CurrentThread.CurrentUICulture);
        }
    }
}