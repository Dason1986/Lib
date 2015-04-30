using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Library.Att;
using Library.HelperUtility;
using Library.Web;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class MVCExtensions
    {
        /// <summary>
        /// 資源名稱
        /// </summary>
        public static string ResourceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Label(this HtmlHelper htmlHelper, string key)
        {

            TagBuilder tagBuilder = new TagBuilder("label");
            var sessionCultureInfo = SessionManager.GetSession<CultureInfo>("lang");
            tagBuilder.InnerHtml = LanguageResourceManagement.GetString(key, ResourceName, sessionCultureInfo);
            tagBuilder.MergeAttribute("for", key);

            return tagBuilder.ToString(TagRenderMode.SelfClosing);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            var member = expression.GetMemberInfo() as PropertyInfo;
            if (member == null) return string.Empty;
            TagBuilder tagBuilder = new TagBuilder("label");
            var sessionCultureInfo = SessionManager.GetSession<CultureInfo>("lang");
            tagBuilder.InnerHtml = LanguageResourceManagement.GetString(member.Name, ResourceName, sessionCultureInfo);
            tagBuilder.GenerateId(member.Name);
            tagBuilder.MergeAttribute("for", member.Name);
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return tagBuilder.ToString(TagRenderMode.SelfClosing);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string EditFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
         
            var member = expression.GetMemberInfo() as PropertyInfo;
            if (member == null) return string.Empty;
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("type", "textbox");
            tagBuilder.InnerHtml = ObjectUtility.Cast<string>(member.FastGetValue(html.ViewData.Model));
            tagBuilder.MergeAttribute("for", member.Name);
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return tagBuilder.ToString(TagRenderMode.SelfClosing);

        }
    }
}
