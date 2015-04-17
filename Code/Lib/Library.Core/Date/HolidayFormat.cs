using System;
using System.Globalization;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public class HolidayFormat : ICustomFormatter, IFormatProvider
    {
        /*  
     Customized format patterns:
     P.S. Format in the table below is the internal number format used to display the pattern.
 
     Patterns   Format      Description                           Example
     =========  ==========  ===================================== ========
     
 
        "d"     "0"         day w/o leading zero                  1
        "dd"    "00"        day with leading zero                 01
        "ddd"               short weekday name (abbreviation)     Mon
        "dddd"              full weekday name                     Monday
        "dddd*"             full weekday name                     Monday
        
 
        "M"     "0"         month w/o leading zero                2
        "MM"    "00"        month with leading zero               02
        "MMM"               short month name (abbreviation)       Feb
        "MMMM"              full month name                       Febuary
        "MMMM*"             full month name                       Febuary
     
  
 
        "g*"                the current era name                  A.D.
 
        ":"                 time separator                        : -- DEPRECATED - Insert separator directly into pattern (eg: "H.mm.ss")
        "/"                 date separator                        /-- DEPRECATED - Insert separator directly into pattern (eg: "M-dd-yyyy")
        "'"                 quoted string                         'ABC' will insert ABC into the formatted string.
        '"'                 quoted string                         "ABC" will insert ABC into the formatted string.
        "%"                 used to quote a single pattern characters      E.g.The format character "%y" is to print two digit year.
        "\"                 escaped character                     E.g. '\d' insert the character 'd' into the format string.
        other characters    insert the character into the format string. 
 
    Pre-defined format characters: 
        (U) to indicate Universal time is used.
        (G) to indicate Gregorian calendar is used.
    
        Format              Description                             Real format                             Example
        =========           =================================       ======================                  =======================
        "d"                 short date                              culture-specific                        10/31/1999
        "D"                 long data                               culture-specific                        Sunday, October 31, 1999
        "f"                 full date (long date + short time)      culture-specific                        Sunday, October 31, 1999 2:00 AM
        "F"                 full date (long date + long time)       culture-specific                        Sunday, October 31, 1999 2:00:00 AM
        "g"                 general date (short date + short time)  culture-specific                        10/31/1999 2:00 AM
        "G"                 general date (short date + long time)   culture-specific                        10/31/1999 2:00:00 AM
        "m"/"M"             Month/Day date                          culture-specific                        October 31
        "y"/"Y"             Year/Month day                          culture-specific                        October, 1999
 
    */


        /// <summary>
        /// 
        /// </summary>
        protected HolidayFormat()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly HolidayFormat FormatProvider = new HolidayFormat();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        private const string NStr1 = "日一二三四五六七八九";
        private const string NStr2 = "初十廿卅";
        private static readonly string[] Weeks = { "最後一個", "第一個", "第二個", "第三個", "第四個" };
        private static readonly string[] Weeksshort = { "{0} last {1}", "the first {1} of {0}", "the sec {1} of {0}", "the 3rd {1} of {0}", "the fourth {1} of {0}" };//Tuesday July first
        private static readonly string[] Weekslong = { "the last {1} of {0}", "the first {1} of {0}", "the second {1} of {0}", "the third {1} of {0}", "the fourth  {1} of {0}" };
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is IHoliday == false) throw new ChineseDateTimeException(11002.106);
            IHoliday holiday = arg as IHoliday;
            CultureInfo cul = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;


            HolidayToString holidayToString;
            switch (cul.Name.Substring(0, 2))
            {
                case "en":
                    holidayToString = new En(format, holiday, cul);
                    break;
                case "zh":
                    {
                        holidayToString = new Cn(format, holiday, cul);

                    }
                    break;
                default:
                    holidayToString = new En(format, holiday, cul);
                    break;
            }


            return holidayToString.ToFormat();
        }

        private abstract class HolidayToString
        {
            protected readonly string FormatStr;
            protected readonly IHoliday Holiday;
            protected readonly CultureInfo Cul;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="format"></param>
            /// <param name="obj"></param>
            /// <param name="cul"></param>
            protected HolidayToString(string format, object obj, CultureInfo cul)
            {
                FormatStr = format;
                Holiday = obj as IHoliday;
                Cul = cul;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public virtual string ToFormat()
            {
                if (FormatStr.Length != 1) return FormatStr;
                if (Holiday is TheSolarTermsHoliday) return string.Format("{0}", Holiday.HolidayName);
                if (Holiday is WeekHoliday)
                {
                    return WeekHoliday();
                }
                if (Holiday is LunarHoliday)
                {
                    return LunarHoliday();
                }
                return SolarHoliday();

            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public abstract string WeekHoliday();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public abstract string LunarHoliday();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public abstract string SolarHoliday();
        }

        class Cn : HolidayToString
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="format"></param>
            /// <param name="obj"></param>
            /// <param name="cul"></param>
            public Cn(string format, object obj, CultureInfo cul)
                : base(format, obj, cul)
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string WeekHoliday()
            {
                var formatDateTime = Cul.DateTimeFormat;
                WeekHoliday week = (WeekHoliday)Holiday;
                var result = string.Empty;
                switch (FormatStr[0])
                {
                    case 'd':
                        {
                            var tmpweek = week.WeekAtMonth == 0 ? "最後1個" : string.Format("第{0}個", week.WeekAtMonth);
                            result = string.Format("{0}月的{1}{2}", week.Month, tmpweek,
                                formatDateTime.AbbreviatedDayNames[(int)week.WeekDay]);

                            break;
                        }

                    case 'D':
                        {
                            result = string.Format("{0}的{1}{2}", formatDateTime.AbbreviatedMonthNames[week.Month - 1],
                                Weeks[week.WeekAtMonth], formatDateTime.DayNames[(int)week.WeekDay]);

                            break;
                        }
                    case 'f':
                        {
                            var tmpweek = week.WeekAtMonth == 0 ? "最後1個" : string.Format("第{0}個", week.WeekAtMonth);
                            result = string.Format("{0}月的{1}{2},{3}", week.Month, tmpweek,
                                formatDateTime.AbbreviatedDayNames[(int)week.WeekDay], week.HolidayName);
                            break;
                        }
                    case 'F':
                        {
                            result = string.Format("{0}的{1}{2},{3}", formatDateTime.AbbreviatedMonthNames[week.Month - 1],
                                Weeks[week.WeekAtMonth], formatDateTime.DayNames[(int)week.WeekDay], week.HolidayName);
                            break;
                        }
                }
                return result;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string LunarHoliday()
            {
                LunarHoliday lunar = (LunarHoliday)Holiday;
                string daystr, result = string.Empty;
                switch (lunar.Day)
                {
                    case 0:
                        daystr = "";
                        break;
                    case 10:
                        daystr = "初十";
                        break;
                    case 20:
                        daystr = "二十";
                        break;
                    case 30:
                        daystr = "三十";
                        break;
                    default:
                        daystr = string.Format("{0}{1}", NStr2[lunar.Day / 10], NStr1[lunar.Day % 10]);
                        break;
                }
                switch (FormatStr[0])
                {
                    case 'd':
                        result = string.Format("{0:d2}/{1:d2}", Holiday.Month, Holiday.Day);
                        break;
                    case 'D':
                        result = string.Format("{0}{1}", CalendarInfo.ChineseMonths[Holiday.Month - 1], daystr);
                        break;
                    case 'f':
                        result = string.Format("{0:d2}/{1:d2},{2}", Holiday.Month, Holiday.Day, Holiday.HolidayName);
                        break;
                    case 'F':
                        result = string.Format("{0}{1},{2}", CalendarInfo.ChineseMonths[Holiday.Month - 1], daystr,
                            Holiday.HolidayName);
                        break;
                }
                return result;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string SolarHoliday()
            {
                var result = string.Empty;
                switch (FormatStr[0])
                {
                    case 'd':
                        result = string.Format("{0:d2}-{1:d2}", Holiday.Month, Holiday.Day);
                        break;
                    case 'D':
                        result = string.Format("{0}月{1}日", Holiday.Month, Holiday.Day);
                        break;
                    case 'f':
                        result = string.Format("{0:d2}-{1:d2},{2}", Holiday.Month, Holiday.Day, Holiday.HolidayName);
                        break;
                    case 'F':
                        result = string.Format("{0}月{1}日,{2}", Holiday.Month, Holiday.Day, Holiday.HolidayName);
                        break;
                }
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        class En : HolidayToString
        {
            public En(string format, object obj, CultureInfo cul)
                : base(format, obj, cul)
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string WeekHoliday()
            {
                string result = string.Empty;
                var formatDateTime = Cul.DateTimeFormat;
                WeekHoliday week = (WeekHoliday)Holiday;
                switch (FormatStr[0])
                {
                    case 'd':
                        {
                            result = string.Format(Weeksshort[week.WeekAtMonth], formatDateTime.AbbreviatedMonthNames[week.Month - 1],
                                formatDateTime.AbbreviatedDayNames[(int)week.WeekDay]);
                            break;
                        }

                    case 'D':
                        {
                            result = string.Format(Weekslong[week.WeekAtMonth], formatDateTime.MonthNames[week.Month - 1],
                                formatDateTime.DayNames[(int)week.WeekDay]);
                            break;

                        }
                    case 'f':
                        {
                            result = string.Format(Weeksshort[week.WeekAtMonth], formatDateTime.AbbreviatedMonthNames[week.Month - 1],
                                formatDateTime.AbbreviatedDayNames[(int)week.WeekDay]);
                            result = string.Format("{0},{1}", result, week.HolidayName);

                            break;
                        }
                    case 'F':
                        {
                            result = string.Format(Weekslong[week.WeekAtMonth], formatDateTime.MonthNames[week.Month - 1],
                                formatDateTime.DayNames[(int)week.WeekDay]);
                            result = string.Format("{0},{1}", result, week.HolidayName);
                            break;
                        }
                    //
                }
                return result;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string LunarHoliday()
            {
                return SolarHoliday();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string SolarHoliday()
            {
                var result = FormatStr;
                var formatDateTime = Cul.DateTimeFormat;
                switch (FormatStr[0])
                {
                    case 'd':
                        result = string.Format("{0:d2}/{1:d2}", Holiday.Month, Holiday.Day);
                        break;
                    case 'D':
                        result = string.Format("{0} {1}", formatDateTime.AbbreviatedMonthNames[Holiday.Month - 1], Holiday.Day);
                        break;
                    case 'f':
                        result = string.Format("{0:d2}/{1:d2},{2}", Holiday.Month, Holiday.Day, Holiday.HolidayName);
                        break;
                    case 'F':
                        result = string.Format("{0} {1},{2}", formatDateTime.MonthNames[Holiday.Month - 1], Holiday.Day,
                            Holiday.HolidayName);
                        break;
                }
                return result;
            }
        }



    }
}