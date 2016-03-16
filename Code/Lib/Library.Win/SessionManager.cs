using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace Library.Win
{
    /// <summary>
    ///
    /// </summary>
    public static class SessionManager
    {
        static uint _TimeOut;
        /// <summary>
        /// 
        /// </summary>
        public static uint TimeOut
        {
            get { return _TimeOut; }
            set
            {
                _TimeOut = value;
            }
        }

        private static readonly IDictionary<string, SessionItem> Dictionary = new ConcurrentDictionary<string, SessionItem>();
        class SessionItem
        {
            public SessionItem()
            {

            }
            public SessionItem(object value)
            {
                Value = value;
            }
            DateTime _LastTime;
            object _Value;
            public DateTime LastTime { get { return _LastTime; } }

            public object Value { get { return _Value; } set { _Value = value; _LastTime = DateTime.Now; } }
        }
     
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TModel GetSession<TModel>(string key)
        {
            if (!Dictionary.ContainsKey(key)) return default(TModel);
            var obj = Dictionary[key];

            if (obj.Value != null && obj.Value is TModel) return (TModel)obj.Value;
            return default(TModel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetSession(string key)
        {
            if (!Dictionary.ContainsKey(key)) return null;
            var item = Dictionary[key];
            if ((DateTime.Now - item.LastTime).TotalMinutes > 20)
            {
                Remove(key);
                return null;
            }

            return item.Value;
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
                var item = Dictionary[key];
                item.Value = value;
            }
            else
            {
                Dictionary.Add(key, new SessionItem(value));
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