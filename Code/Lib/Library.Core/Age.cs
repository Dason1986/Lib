using System;
using System.Globalization;

namespace Library
{
    /// <summary>
    ///
    /// </summary>
    public class AgeFormat : ICustomFormatter, IFormatProvider
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly AgeFormat FormatProvider = new AgeFormat();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is Age == false) throw new Exception();
            Age age = (Age)arg;
            CultureInfo cul = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
            if (age.Year == 0)
            {
                return string.Format("{0}個月", age.Month);
            }
            if (age.Year < 7)
            {
                return string.Format("{0}歲{1}個月", age.Year, age.Month);
            }
            return string.Format("{0}歲", age.Year);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct Age : IComparable, IComparable<Age>, IFormattable
    {
        /// <summary>
        /// 
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public static Age FromBirthday(DateTime birthday)
        {
            if (DateTime.Now < birthday) throw new Exception();
            var tiemsp = (DateTime.Now.Date - birthday.Date);

            var f = new DateTime().AddDays(tiemsp.TotalDays);
            return new Age { Year = f.Year, Month = f.Month };

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            return base.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {

            return AgeFormat.FormatProvider.Format(format, this, formatProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Age other)
        {
            return (Year == other.Year) ? Month.CompareTo(other.Month) : Year.CompareTo(other.Year);


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is Age == false)
                throw new NotImplementedException();

            return CompareTo((Age)obj);
        }
    }
}