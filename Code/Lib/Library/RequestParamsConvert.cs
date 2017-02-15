using Library.HelperUtility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using Library.Annotations;

namespace Library
{
    /// <summary>
    /// 轉換參數
    /// </summary>
    public class RequestParamsConvert
    {
        private readonly NameValueCollection _collection;

        /// <summary>
        ///
        /// </summary>
        /// <param name="collection"></param>
        public RequestParamsConvert(NameValueCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            this._collection = collection;
            Prefixes = prefixes;
        }

        private static readonly string[] prefixes = new[] { "btn", "txt", "cmb", "dtp", "chk", "cbl", "ddl", "hdn", "cal", "lab" };

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestParam"></param>
        /// <returns></returns>
        public T GetValue<T>(string requestParam) where T : struct
        {
            return GetValue<T>(requestParam, default(T));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestParam"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValue<T>(string requestParam, T defaultValue) where T : struct
        {
            var value = GetParamStringValue(requestParam);
            if (!value.Item1) return default(T);

            return StringUtility.TryCast(value.Item2, defaultValue);
        }

        /// <summary>
        ///
        /// </summary>
        public string ParamNamePageSize = "rows";

        /// <summary>
        ///
        /// </summary>
        public string ParamNamePageNo = "page";

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int GetPageSize()
        {
            return GetValue<int>(ParamNamePageSize, 5);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int GetPageNo()
        {
            return GetValue<int>(ParamNamePageNo, 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestParam"></param>
        /// <returns></returns>
        public T? GetValueType<T>(string requestParam) where T : struct
        {
            var value = GetParamStringValue(requestParam);
            if (!value.Item1) return null;

            var obj = StringUtility.TryCast<T>(value.Item2);
            if (obj.HasError) return null;
            return obj.Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestParam"></param>
        /// <returns></returns>
        public T[] GetEnums<T>(string requestParam)
        {
            var value = GetParamStringValues(requestParam);
            if (!value.Item1) return null;
            T[] array = new T[value.Item2.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (T)StringUtility.Cast(value.Item2[i], typeof(T));
            }
            return array;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestParam"></param>
        /// <returns></returns>
        public T GetEnum<T>(string requestParam)
        {
            var value = GetParamStringValue(requestParam);
            if (!value.Item1) return default(T);

            return StringUtility.TryCast<T>(value.Item2);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="requestParam"></param>
        /// <returns></returns>
        public string GetString(string requestParam)
        {
            var value = GetParamStringValue(requestParam);

            return value.Item2;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="requestParam"></param>
        /// <returns></returns>
        private Tuple<bool, string> GetParamStringValue(string requestParam)
        {
            if (string.IsNullOrEmpty(requestParam)) return new Tuple<bool, string>(false, string.Empty);
            var strParam = _collection[requestParam];
            if (string.IsNullOrWhiteSpace(strParam)) return new Tuple<bool, string>(false, string.Empty);

            return new Tuple<bool, string>(true, strParam.Trim());
        }

        private Tuple<bool, string[]> GetParamStringValues(string requestParam)
        {
            if (string.IsNullOrEmpty(requestParam)) return new Tuple<bool, string[]>(false, null);
            var strParam = _collection[requestParam];
            if (string.IsNullOrWhiteSpace(strParam)) return new Tuple<bool, string[]>(false, null);

            return new Tuple<bool, string[]>(true, strParam.Trim().Split(ch));
        }

        private static readonly char[] ch = { ',', '|' };

        /// <summary>
        ///
        /// </summary>
        /// <param name="requestParam"></param>
        /// <returns></returns>
        public Guid GetGuid(string requestParam)
        {
            var value = GetParamStringValue(requestParam);
            if (!value.Item1) return Guid.Empty;
            return StringUtility.TryCast<Guid>(value.Item2, Guid.Empty);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="requestParam"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public DateTime? GetDateTimeOrNull(string requestParam, string format = "dd/MM/yyyy")
        {
            var value = GetParamStringValue(requestParam);
            if (!value.Item1) return null;
            if (string.IsNullOrWhiteSpace(format))
                format = "dd/MM/yyyy";
            if (value.Item2.Length != format.Length) return null;

            DateTime result;
            if (DateTime.TryParseExact(value.Item2, format, null, System.Globalization.DateTimeStyles.None, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        public string[] Prefixes { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>

        public TryResult GetModel<TModel>(TModel model) where TModel : class
        {
            if (model == null) return new ArgumentNullException("model");
            var properties = model.GetType().GetProperties();

            List<Exception> elist = new List<Exception>();
            foreach (string name in this._collection.AllKeys)
            {
                var tepname = name;

                if (Prefixes != null && Prefixes.Any(n => name.StartsWith(n)))
                    tepname = name.Substring(3);

                PropertyInfo property = properties.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase)
                || string.Equals(p.Name, tepname, StringComparison.CurrentCultureIgnoreCase)
                );
                if (property == null)
                {
                    continue;
                }
                try
                {
                    var obj = StringUtility.Cast(_collection[name], property.PropertyType);

                    property.FastSetValue(model, obj);
                }
                catch (Exception ex)
                {
                    elist.Add(ex);
                }
            }
            return elist.HasRecord() ? new TryResult(elist) : new TryResult(true);
        }
    }
}