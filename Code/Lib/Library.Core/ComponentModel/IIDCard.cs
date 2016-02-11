using System;

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
        /// 身份證
        /// </summary>
        /// <returns></returns>
        string IDNumber { get; }

        /// <summary>
        /// 驗證身份證號碼
        /// </summary>
        /// <returns></returns>
        void Validate();
    }

    /// <summary>
    ///
    /// </summary>
    public interface IIDCardProvider
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IIDCard CreateNew();
    }

    /// <summary>
    ///
    /// </summary>
    public enum SexEnum
    {
        /// <summary>
        /// 女
        /// </summary>
        Woman,

        /// <summary>
        /// 男
        /// </summary>
        Man,
    }
}