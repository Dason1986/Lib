using Library.HelperUtility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;

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
        public static TryResult<TModel> TryGetModelWithGet<TModel>() where TModel : class, new()
        {
            var model = new TModel();
            var flag = HttpContext.Current.Request.QueryString.GetModel(model);
            return flag == true ? new TryResult<TModel>(model) : new TryResult<TModel>(flag.Error);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static RequestParamsConvert CreateFormConvert(this HttpContext context)
        {
            return new RequestParamsConvert(context.Request.Form);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static RequestParamsConvert CreateQueryConvert(this HttpContext context)
        {
            return new RequestParamsConvert(context.Request.QueryString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TryResult<TModel> TryGetModelWithPost<TModel>() where TModel : class, new()
        {
            var model = new TModel();
            var flag = HttpContext.Current.Request.Form.GetModel(model);
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