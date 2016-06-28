using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Library.ComponentModel.Logic
{
    /// <summary>
    ///
    /// </summary>
    public interface IStart
    {
        /// <summary>
        ///
        /// </summary>
        void Start();

        /// <summary>
        ///
        /// </summary>
        void Stop();
    }

    /// <summary>
    /// 管理
    /// </summary>
    public interface IManagement
    {
    }
}