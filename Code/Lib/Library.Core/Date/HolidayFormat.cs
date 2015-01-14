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
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }
      
        private static readonly string[] Weeks = { "最後一個", "第一個", "第二個", "第三個", "第四個" };
        private static readonly string[] Weeksshort = { "{0} last {1}", "the first {1} of {0}", "the sec {1} of {0}", "the 3rd {1} of {0}", "the fourth {1} of {0}", };//Tuesday July first
        private static readonly string[] Weekslong = { "the last {1} of {0}", "the first {1} of {0}", "the second {1} of {0}", "the third {1} of {0}", "the fourth  {1} of {0}" };
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is IHoliday == false) throw new ChineseDateTimeException("不支持類型格式化輸了", new FormatException());
            IHoliday holiday = arg as IHoliday;
            var result = format;
            CultureInfo cul = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
            
            
            var fomart = cul.DateTimeFormat;

            switch (cul.Name.Substring(0, 2))
            {
                case "en":
                    #region en
                    if (format.Length == 1)
                    {
                        if (holiday is WeekHoliday)
                        {
                            WeekHoliday week = (WeekHoliday)holiday;
                            switch (format[0])
                            {
                                case 'd':
                                    {
                                        result = string.Format(Weeksshort[week.WeekAtMonth], fomart.AbbreviatedMonthNames[week.Month - 1], fomart.AbbreviatedDayNames[(int)week.WeekDay]); break;
                                        break;
                                    }

                                case 'D':
                                    {
                                        result = string.Format(Weekslong[week.WeekAtMonth], fomart.MonthNames[week.Month - 1], fomart.DayNames[(int)week.WeekDay]); break;
                                        break;
                                    }
                                case 'f':
                                    {
                                        result = string.Format(Weeksshort[week.WeekAtMonth], fomart.AbbreviatedMonthNames[week.Month - 1], fomart.AbbreviatedDayNames[(int)week.WeekDay]);
                                        result = string.Format("{0},{1}", result, week.HolidayName);
                                        
                                        break;


                                    }
                                case 'F':
                                    {
                                        result = string.Format(Weekslong[week.WeekAtMonth], fomart.MonthNames[week.Month - 1], fomart.DayNames[(int)week.WeekDay]);
                                        result = string.Format("{0},{1}", result, week.HolidayName);
                                        break;
                                      
                                    }
                                //
                            }
                        }
                        else
                        {
                            switch (format[0])
                            {
                                case 'd': result = string.Format("{0:d2}/{1:d2}", holiday.Month, holiday.Day); break;
                                case 'D': result = string.Format("{0} {1}", fomart.AbbreviatedMonthNames[holiday.Month - 1], holiday.Day); break;
                                case 'f': result = string.Format("{0:d2}/{1:d2},{2}", holiday.Month, holiday.Day, holiday.HolidayName); break;
                                case 'F': result = string.Format("{0} {1},{2}", fomart.MonthNames[holiday.Month - 1], holiday.Day, holiday.HolidayName); break;
                            }
                        }

                    }
                    #endregion
                    break;
                case "zh":
                    {
                        #region zh-CN
                        if (format.Length == 1)
                        {
                            if (holiday is WeekHoliday)
                            {
                                WeekHoliday week = (WeekHoliday)holiday;
                                switch (format[0])
                                {
                                    case 'd':
                                    {
                                        var tmpweek = week.WeekAtMonth == 0 ? "最後1個" : string.Format("第{0}個", week.WeekAtMonth);
                                        result = string.Format("{0}月的{1}{2}", week.Month, tmpweek, fomart.AbbreviatedDayNames[(int)week.WeekDay]); break;
                                        break;
                                    }

                                    case 'D':
                                    {
                                        result = string.Format("{0}的{1}{2}", fomart.AbbreviatedMonthNames[week.Month - 1], Weeks[week.WeekAtMonth], fomart.DayNames[(int)week.WeekDay]); break;
                                        break;
                                    }
                                    case 'f':
                                    {
                                        var tmpweek = week.WeekAtMonth == 0 ? "最後1個" : string.Format("第{0}個", week.WeekAtMonth);
                                        result = string.Format("{0}月的{1}{2},{3}", week.Month, tmpweek, fomart.AbbreviatedDayNames[(int)week.WeekDay], week.HolidayName); break;
                                        
                                    }
                                    case 'F':
                                    {
                                        result = string.Format("{0}的{1}{2},{3}", fomart.AbbreviatedMonthNames[week.Month - 1], Weeks[week.WeekAtMonth], fomart.DayNames[(int)week.WeekDay], week.HolidayName); break;
                                    }
                                    //
                                }
                            }
                            else
                            {
                                switch (format[0])
                                {
                                    case 'd': result = string.Format("{0:d2}-{1:d2}", holiday.Month, holiday.Day); break;
                                    case 'D': result = string.Format("{0}月{1}日", holiday.Month, holiday.Day); break;
                                    case 'f': result = string.Format("{0:d2}-{1:d2},{2}", holiday.Month, holiday.Day, holiday.HolidayName); break;
                                    case 'F': result = string.Format("{0}月{1}日,{2}", holiday.Month, holiday.Day, holiday.HolidayName); break;
                                }
                            }

                        }
                        #endregion
                    }
                    break;
            }


            return result;
        }
    }
}