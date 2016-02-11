using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Library.Win
{
    /// <summary>
    ///
    /// </summary>
    public static class SessionManager
    {
        private static readonly IDictionary<string, object> Dictionary = new ConcurrentDictionary<string, object>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TModel GetSession<TModel>(string key)
        {
            var obj = Dictionary[key];
            if (obj != null && obj is TModel) return (TModel)obj;
            return default(TModel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetSession(string key)
        {
            return Dictionary[key]; ;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetSession(string key, object value)
        {
            if (Dictionary.ContainsKey(key))
            {
                Dictionary[key] = value;
            }
            else
            {
                Dictionary.Add(key, value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            Dictionary.Remove(key);
        }

        /// <summary>
        ///
        /// </summary>
        public static void RemoveAll()
        {
            Dictionary.Clear();
        }
    }
}