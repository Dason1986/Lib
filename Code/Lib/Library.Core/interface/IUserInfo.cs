using System;

namespace Library
{
    /// <summary>
    /// 用戶信息
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// 名稱
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        SexEnum Sex { get; set; }

        /// <summary>
        /// 年齡
        /// </summary>
        int Age { get; }

        /// <summary>
        /// 出生日期
        /// </summary>
        DateTime Birthday { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IAccountInfo
    {
        /// <summary>
        /// 帳戶名稱
        /// </summary>
        string AccountID { get; set; }

        /// <summary>
        /// 帳戶密碼
        /// </summary>
        string AccountPWD { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IUserName
    {
        /// <summary>
        /// 
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string FirstName { get; set; }

    }
}