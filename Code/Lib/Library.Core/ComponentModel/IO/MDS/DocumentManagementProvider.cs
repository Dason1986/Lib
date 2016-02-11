using System;
using System.Collections.Specialized;

namespace Library.ComponentModel.IO.MDS
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DocumentManagementProvider : IProvider
    {
        /// <summary>
        ///
        /// </summary>
        ~DocumentManagementProvider()
        {
            Dispose(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="apiProvider"></param>
        /// <param name="fileRepositoriey"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected DocumentManagementProvider(IDocumentManagementAPIProvider apiProvider, IFileRepositoriey fileRepositoriey)
        {
            if (apiProvider == null) throw new ArgumentNullException("apiProvider");
            if (fileRepositoriey == null) throw new ArgumentNullException("fileRepositoriey");
            APIProvider = apiProvider;
            FileRepositoriey = fileRepositoriey;
        }

        /// <summary>
        ///
        /// </summary>
        protected IDocumentManagementAPIProvider APIProvider { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected IFileRepositoriey FileRepositoriey { get; set; }

        /// <summary>
        ///
        /// </summary>
        public virtual void Initialize()
        {
            NameValueCollection collection = FileRepositoriey.GetConfig();
            APIProvider.Initialize(collection);
        }

        void IProvider.Initialize(NameValueCollection collection)
        {
            this.Initialize();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Connection()
        {
            APIProvider.Connection();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual DMSFileInfo GetFileInfo(string fileName)
        {
            var file = FileRepositoriey.GetFile(fileName);
            if (file == null) throw new DocumentManagementException("file not exist");
            return APIProvider.GetFileInfo(file.APIFullName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public virtual DMSFileInfo[] GetFiles(string dirPath)
        {
            return FileRepositoriey.GetFiles(dirPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual DMSFileInfo[] GetFilesOnRoot()
        {
            return APIProvider.GetFilesOnRoot();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual DMSDirectoryInfo[] GetDirectoriesOnRoot()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="DocumentManagementException"></exception>
        public virtual void DeleteFile(object id)
        {
            var file = FileRepositoriey.GetFile(id);
            if (file == null) throw new DocumentManagementException("file not exist");
            APIProvider.DeleteFile(file.APIFullName);
            FileRepositoriey.DeleteFile(id);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="DocumentManagementException"></exception>
        public virtual void DeleteFile(string filePath)
        {
            var file = FileRepositoriey.GetFile(filePath);

            if (file == null) throw new DocumentManagementException("file not exist");
            APIProvider.DeleteFile(filePath);
            FileRepositoriey.DeleteFile(file.RepositorieyID);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dirPath"></param>
        public virtual void DeleteDirectory(string dirPath)
        {
            APIProvider.DeleteDirectory(dirPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public virtual DMSDirectoryInfo[] GetDirectories(string dirPath)
        {
            return APIProvider.GetDirectories(dirPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileid"></param>
        /// <returns></returns>
        public virtual byte[] GetFileBuffer(object fileid)
        {
            var file = FileRepositoriey.GetFile(fileid);
            return APIProvider.GetFileBuffer(file.APIFullName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileid"></param>
        /// <returns></returns>
        public virtual DMSFileInfo GetFileInfo(object fileid)
        {
            var file = FileRepositoriey.GetFile(fileid);
            return file;
            //    return APIProvider.GetFileInfo(file.FullName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileBuffer"></param>
        /// <param name="fileName"></param>
        /// <param name="metas"></param>
        /// <exception cref="DocumentManagementException"></exception>
        public virtual void UpdateFile(object id, byte[] fileBuffer, string fileName, NameValueCollection metas)
        {
            if (fileBuffer == null) throw new DocumentManagementException("");
            if (string.IsNullOrEmpty(fileName)) throw new DocumentManagementException("");
            var file = FileRepositoriey.GetFile(id);
            if (file == null) throw new DocumentManagementException("");
            APIProvider.AddFile(fileBuffer, fileName, true);
            //       APIProvider.UpdateFile(file.ApiFileID, fileBuffer);
            FileRepositoriey.Edit(file.RepositorieyID, fileName, metas);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileBuffer"></param>
        /// <param name="fileName"></param>
        /// <param name="metas"></param>
        /// <returns></returns>
        public virtual DMSFileInfo AddFile(byte[] fileBuffer, string fileName, NameValueCollection metas)
        {
            var apiFileinfo = APIProvider.AddFile(fileBuffer, fileName, true);
            if (apiFileinfo == null) throw new DocumentManagementException("");
            var fileinfo = this.FileRepositoriey.Add(fileName, metas);
            apiFileinfo.RepositorieyID = fileinfo.RepositorieyID;
            //    var id = FileRepositoriey.Add(fileid, fileName, metas);
            //      return new DMSFileInfo();
            return apiFileinfo;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Close()
        {
            APIProvider.Close();
        }

        private bool _isDispose;

        private void Dispose(bool isDispose)
        {
            if (_isDispose) return;
            if (!isDispose) return;
            Close();
            _isDispose = true;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
        }
    }
}