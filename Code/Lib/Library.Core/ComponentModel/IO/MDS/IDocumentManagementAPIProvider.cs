namespace Library.ComponentModel.IO.MDS
{
    /// <summary>
    ///
    /// </summary>
    public interface IDocumentManagementAPIProvider : IProvider
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        byte[] GetFileBuffer(string path);

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        DMSFileInfo GetFileInfo(string path);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileBuffer"></param>
        /// <param name="fileName"></param>
        /// <param name="isOverWrite"></param>
        /// <returns></returns>
        DMSFileInfo AddFile(byte[] fileBuffer, string fileName, bool isOverWrite);

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        void DeleteDirectory(string path);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DMSFileInfo[] GetFilesOnRoot();

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        void CreateDirector(string path);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        DMSDirectoryInfo[] GetDirectories(string dirPath);

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        void DeleteFile(string filePath);
    }
}