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