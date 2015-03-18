using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using Library.HelperUtility;

namespace Library.Web
{
    /// <summary>
    /// 实体
    /// </summary>
    public static class HttpContextHelper
    {


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TryResult<TModel> TryGetModelWithGet<TModel>() where TModel : class  ,new()
        {
            var model = new TModel();
            var flag = SetProperty(HttpContext.Current.Request.QueryString, model);
            return flag == true ? new TryResult<TModel>(model) : new TryResult<TModel>(flag.Error);
        }

        private static TryResult SetProperty<TModel>(NameValueCollection collection, TModel model) where TModel : class
        {
            if (model == null) return new ArgumentNullException("model");
            var properties = model.GetType().GetProperties();

            List<Exception> elist = new List<Exception>();
            foreach (string name in collection.AllKeys)
            {
                PropertyInfo property = properties.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
                if (property == null) continue;
                try
                {
                    var obj = StringUtility.TryCast(collection[name], property.PropertyType);
                    if (!obj.HasError)
                        property.FastSetValue(model, obj);

                }
                catch (Exception ex)
                {
                    elist.Add(ex);
                }
            }
            return elist.HasRecord() ? new TryResult(elist) : new TryResult(true);

        }

        /// <summary>
        /// 
        /// </summary>

        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TryResult<TModel> TryGetModelWithPost<TModel>() where TModel : class  ,new()
        {
            var model = new TModel();
            var flag = SetProperty(HttpContext.Current.Request.Form, model);
            return flag == true ? new TryResult<TModel>(model) : new TryResult<TModel>(flag.Error);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"> </param>
        /// <param name="displayFileName"></param>
        /// <param name="buffer"></param>
        public static void DownloadFile(this HttpResponse response, string displayFileName, byte[] buffer)
        {

            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(displayFileName));
            response.AddHeader("Content-Length", buffer.Length.ToString());
            response.AddHeader("Content-Transfer-Encoding", "binary");
            response.ContentType = "application/octet-stream";
            response.BinaryWrite(buffer);
            response.Flush();
            response.End();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"> </param>
        /// <param name="displayFileName"></param>
        /// <param name="buffer"></param>
        public static void DownloadFile(this HttpResponseBase response, string displayFileName, byte[] buffer)
        {

            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(displayFileName));
            response.AddHeader("Content-Length", buffer.Length.ToString());
            response.AddHeader("Content-Transfer-Encoding", "binary");
            response.ContentType = "application/octet-stream";
            response.BinaryWrite(buffer);
            response.Flush();
            response.End();
        }
    }
}
