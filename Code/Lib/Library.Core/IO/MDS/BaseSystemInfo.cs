using System;

namespace Library.ComponentModel.IO.MDS
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BaseSystemInfo
    {
        /// <summary>
        ///
        /// </summary>
        public bool IsDirectory { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsFile { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string APIFullName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object RepositorieyID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object ApiID { get; set; }
    }
}