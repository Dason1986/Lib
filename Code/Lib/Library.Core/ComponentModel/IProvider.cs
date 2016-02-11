using System;
using System.Collections.Specialized;

namespace Library.ComponentModel
{
    /// <summary>
    ///
    /// </summary>
    public interface IProvider : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        void Connection();

        /// <summary>
        ///
        /// </summary>
        void Close();

        /// <summary>
        ///
        /// </summary>
        /// <param name="collection"></param>
        void Initialize(NameValueCollection collection);
    }
}