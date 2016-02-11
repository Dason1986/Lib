using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Library.ComponentModel.IO.MDS
{
    /// <summary>
    ///
    /// </summary>
    public interface IFileRepositoriey
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        DMSFileInfo GetFile(string fileName);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        NameValueCollection GetConfig();

        /// <summary>
        ///
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        DMSFileInfo[] GetFiles(string dirPath);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NameValueCollection GetMetas(object id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="metas"></param>
        DMSFileInfo Add(string fileName, NameValueCollection metas);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <param name="metas"></param>
        void Edit(object id, string fileName, NameValueCollection metas);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DMSFileInfo GetFile(object id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        void DeleteFile(object id);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IDictionary<Guid, string> GetFloders();
    }
}