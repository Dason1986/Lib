using System;
using System.Collections.Specialized;

namespace Library.ComponentModel
{
    /// <summary>
    ///
    /// </summary>
    public interface IIOProvider : IDisposable, IClose
    {
        /// <summary>
        ///
        /// </summary>
        void Connection();
 

        /// <summary>
        ///
        /// </summary>
        /// <param name="collection"></param>
        void Initialize(NameValueCollection collection);
    }
}