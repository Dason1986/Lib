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
            if (managers.Count == 0)
            {
                var ass = AppDomain.CurrentDomain.GetAssemblies();
                AppDomain.CurrentDomain.AssemblyLoad += (x, y) => { y.LoadedAssembly.GetAttribute<LanguageRegisterAttribute>(); };
                foreach (var assembly in ass)
                {
                    assembly.GetAttribute<LanguageRegisterAttribute>();

                }
            }
            return managers[name];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="resultCode"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string GetException(string msg, double resultCode, string resourceName)
        {
            string name = "Code" + resultCode.ToString(CultureInfo.InvariantCulture).Replace(".", "_");
            var tmpmsg = GetString(resourceName, name);
            return tmpmsg ?? msg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public static string GetException(string msg, double resultCode)
        {
            return GetException(msg, resultCode, "Global");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public static string GetException(double resultCode)
        {
            return GetException(string.Empty, resultCode, "Global");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetException(string msg)
        {
            return GetString("Global", msg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        /// <returns></returns>
        public static string GetException(string message, double resultCode, object[] formatages)
        {
            var str = GetException(message, resultCode);
            return string.Format(str, formatages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string GetException(string message, double resultCode, object[] formatages, string resourceName)
        {
            var str = GetException(message, resultCode, resourceName);
            return string.Format(str, formatages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        /// <returns></returns>
        public static string GetException(double resultCode, object[] formatages)
        {
            var str = GetException(string.Empty, resultCode);
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
            var str = GetException(string.Empty, resultCode, resourceName);
            return string.Format(str, formatages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="name"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string GetString(string resourceName, string name, CultureInfo culture = null)
        {
            return getManager(resourceName).GetString(name, culture ?? Thread.CurrentThread.CurrentUICulture);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="name"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static Stream GetStream(string resourceName, string name, CultureInfo culture = null)
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
        public static object GetObject(string resourceName, string name, CultureInfo culture = null)
        {
            return getManager(resourceName).GetObject(name, culture ?? Thread.CurrentThread.CurrentUICulture);
        }
    }
}