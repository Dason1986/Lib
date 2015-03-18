namespace Library.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TModel GetSession<TModel>(string key) where TModel : class,new()
        {
            var session = System.Web.HttpContext.Current.Session;
            var obj = session[key];
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
            var session = System.Web.HttpContext.Current.Session;
            return session[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetSession(string key, object value)
        {
            var session = System.Web.HttpContext.Current.Session;
            session.Add(key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            var session = System.Web.HttpContext.Current.Session;
            session.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RemoveAll()
        {
            System.Web.HttpContext.Current.Session.RemoveAll();
        }



    }
}
