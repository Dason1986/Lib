using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictionaryHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null) return;
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultvalue)
        {
            if (dictionary.IsEmpty() || !dictionary.ContainsKey(key)) return defaultvalue;
            return dictionary[key];

        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValueIgnoreCase<TValue>(this IDictionary<string, TValue> dictionary, string key, TValue defaultValue)
        {
            if (dictionary.IsEmpty()) return defaultValue;
            if (dictionary.ContainsKey(key)) return dictionary[key];
            var tmpkey = dictionary.Keys.FirstOrDefault(n => string.Equals(n, key, StringComparison.OrdinalIgnoreCase));
            return tmpkey == null ? defaultValue : dictionary[tmpkey];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        public static void Reomve<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue, bool> func)
        {
            if (dict.IsEmpty() || func == null) return;
            var removekey = from value in dict let flag = func(value.Value) where flag select value;
            foreach (var value in removekey)
            {
                dict.Remove(value.Key);
            }
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultFunc"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueOrdefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> defaultFunc)
        {
            if (dict == null) return default(TValue);
            TValue value;
            if (dict.ContainsKey(key))
            {
                value = dict[key];
                return value;
            }
            if (defaultFunc == null) return default(TValue);
            value = defaultFunc();
            dict[key] = value;
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultFunc"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueOrdefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> defaultFunc)
        {
            if (dict == null) return default(TValue);
            TValue value;
            if (dict.ContainsKey(key))
            {
                value = dict[key];
                return value;
            }
            if (defaultFunc == null) return default(TValue);
            value = defaultFunc(key);
            dict[key] = value;
            return value;
        }

    }
}