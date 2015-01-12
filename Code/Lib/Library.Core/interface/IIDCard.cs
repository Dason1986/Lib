using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// 身份證
    /// </summary>
    public interface IIDCard
    {
        /// <summary>
        /// 第幾代身份證
        /// </summary>
        int Version { get; }

        /// <summary>
        /// 
        /// </summary>
        Guid CardTypeID { get; }


        /// <summary>
        /// 身份證類型名稱
        /// </summary>
        string CardTypeName { get; }

        /// <summary>
        /// 新身份證
        /// </summary>
        /// <returns></returns>
        string NewID();

        /// <summary>
        /// 驗證身份證號碼
        /// </summary>
        /// <param name="idnumber"></param>
        /// <returns></returns>
        bool Validate(string idnumber);
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SexEnum
    {
        /// <summary>
        /// 男
        /// </summary>
        Man,
        /// <summary>
        /// 女
        /// </summary>
        Woman,
        /// <summary>
        /// 未知
        /// </summary>
        Secrecy
    }


}
