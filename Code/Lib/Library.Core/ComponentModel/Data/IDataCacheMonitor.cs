using System.IO;
using System.Reflection;

namespace Library.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataCacheMonitor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string[] GetCacheIDs();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Contains(string id);
        /// <summary>
        /// 
        /// </summary>
        void Clear();
        /// <summary>
        /// 
        /// </summary>
        void AddCurrentAssembly();
        /// <summary>
        /// 
        /// </summary>
        void AddCurrentDirectory();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        void AddDataCache(Assembly assembly);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        void AddDataCache(Stream stream);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object GetCache(string id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataCacheMonitor<T> : IDataCacheMonitor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new T GetCache(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataCache"></param>
        void AddDataCache(T dataCache);
    }
}