namespace Library.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueryDataProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        object GetDataSource(IQueryControl control);
        /// <summary>
        /// 
        /// </summary>
        void ClearCache();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryID"></param>
        void ChangeCache(string queryID);
    }
}