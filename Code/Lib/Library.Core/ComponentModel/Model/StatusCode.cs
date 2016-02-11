using System;

namespace Library.ComponentModel.Model
{
    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum StatusCode
    {
        /// <summary>
        /// 邏輯刪除，不能返回去前端
        /// </summary>
        Delete = 0,

        /// <summary>
        /// 無效
        /// </summary>
        Disabled = 1,

        /// <summary>
        /// 有效
        /// </summary>
        Enabled = 2,

        /// <summary>
        /// 只讀
        /// </summary>
        ReadOnly = 4,
    }
}