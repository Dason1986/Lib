using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 插件編號
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// 插件名稱
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 說明
        /// </summary>
        string Description { get; }
    }
}
