using System;
using System.ComponentModel;
using System.Globalization;

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
        [Browsable(false)]
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

    /// <summary>
    ///
    /// </summary>
    public class UserName : IUserName
    {
        /// <summary>
        ///
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return UserNameFormat.FormatProvider.Format(format, this, formatProvider);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class UserNameFormat : ICustomFormatter, IFormatProvider
    {
        /// <summary>
        ///
        /// </summary>
        protected UserNameFormat()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static readonly UserNameFormat FormatProvider = new UserNameFormat();

        /// <summary>
        ///
        /// </summary>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is IUserName == false) throw new CodeException();
            var user = arg as IUserName;
            CultureInfo cul = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;

            switch (cul.Name.Substring(0, 2))
            {
                case "en":
                    return string.Format("{0}˙{1}", user.FirstName, user.LastName);

                default:
                    {
                        return string.Format("{0}{1}", user.FirstName, user.LastName);
                    }
            }
        }
    }
}