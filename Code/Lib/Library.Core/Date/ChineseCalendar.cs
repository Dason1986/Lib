using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Library.Date
{
    #region ChineseCalendarException
    /// <summary>
    /// 中国日历异常处理
    /// </summary>
    public class ChineseDateTimeException : LibException
    {

        public ChineseDateTimeException()
        {
        }
        public ChineseDateTimeException(string message, double resultCode)
            : base(message, resultCode)
        {

        }

        public ChineseDateTimeException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {

        }

        public ChineseDateTimeException(string message)
            : base(message)
        {
        }

        public ChineseDateTimeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ChineseDateTimeException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }

    #endregion

    /// <summary>
    /// 中国農曆类 版本V1.0 支持 1900.1.31日起至 2049.12.31日止的数据
    /// </summary>
    /// <remarks>
    /// 本程序使用数据来源于网上的万年历查询，并综合了一些其它数据
    /// </remarks>
    public class ChineseDateTime
    {
         

        #region 内部变量

        private DateTime _date;


        private readonly int _cYear;
        private readonly int _cMonth; //農曆月份
        private readonly int _cDay; //農曆当月第几天
        private readonly bool _cIsLeapMonth; //当月是否闰月
        private readonly bool _cIsLeapYear; //当年是否有闰月

        #endregion

        #region 基础数据

        #region 基本常量

        private const int MinYear = 1900;
        private const int MaxYear = 2050;
        private static readonly DateTime MinDay = new DateTime(1900, 1, 30);
        private static readonly DateTime MaxDay = new DateTime(2049, 12, 31);
        private const int GanZhiStartYear = 1864; //干支计算起始年
        private static readonly DateTime GanZhiStartDay = new DateTime(1899, 12, 22); //起始日

        private const int AnimalStartYear = 1900; //1900年为鼠年
        private static readonly DateTime ChineseConstellationReferDay = new DateTime(2007, 9, 13); //28星宿参考值,本日为角

        #endregion

        #region 阴历数据

        /// <summary>
        /// 来源于网上的農曆数据
        /// </summary>
        /// <remarks>
        /// 数据结构如下，共使用17位数据
        /// 第17位：表示闰月天数，0表示29天   1表示30天
        /// 第16位-第5位（共12位）表示12个月，其中第16位表示第一月，如果该月为30天则为1，29天为0
        /// 第4位-第1位（共4位）表示闰月是哪个月，如果当年没有闰月，则置0
        ///</remarks>
        private static readonly int[] LunarDateArray = new int[]
        {
            0x04BD8, 0x04AE0, 0x0A570, 0x054D5, 0x0D260, 0x0D950, 0x16554, 0x056A0, 0x09AD0, 0x055D2,
            0x04AE0, 0x0A5B6, 0x0A4D0, 0x0D250, 0x1D255, 0x0B540, 0x0D6A0, 0x0ADA2, 0x095B0, 0x14977,
            0x04970, 0x0A4B0, 0x0B4B5, 0x06A50, 0x06D40, 0x1AB54, 0x02B60, 0x09570, 0x052F2, 0x04970,
            0x06566, 0x0D4A0, 0x0EA50, 0x06E95, 0x05AD0, 0x02B60, 0x186E3, 0x092E0, 0x1C8D7, 0x0C950,
            0x0D4A0, 0x1D8A6, 0x0B550, 0x056A0, 0x1A5B4, 0x025D0, 0x092D0, 0x0D2B2, 0x0A950, 0x0B557,
            0x06CA0, 0x0B550, 0x15355, 0x04DA0, 0x0A5B0, 0x14573, 0x052B0, 0x0A9A8, 0x0E950, 0x06AA0,
            0x0AEA6, 0x0AB50, 0x04B60, 0x0AAE4, 0x0A570, 0x05260, 0x0F263, 0x0D950, 0x05B57, 0x056A0,
            0x096D0, 0x04DD5, 0x04AD0, 0x0A4D0, 0x0D4D4, 0x0D250, 0x0D558, 0x0B540, 0x0B6A0, 0x195A6,
            0x095B0, 0x049B0, 0x0A974, 0x0A4B0, 0x0B27A, 0x06A50, 0x06D40, 0x0AF46, 0x0AB60, 0x09570,
            0x04AF5, 0x04970, 0x064B0, 0x074A3, 0x0EA50, 0x06B58, 0x055C0, 0x0AB60, 0x096D5, 0x092E0,
            0x0C960, 0x0D954, 0x0D4A0, 0x0DA50, 0x07552, 0x056A0, 0x0ABB7, 0x025D0, 0x092D0, 0x0CAB5,
            0x0A950, 0x0B4A0, 0x0BAA4, 0x0AD50, 0x055D9, 0x04BA0, 0x0A5B0, 0x15176, 0x052B0, 0x0A930,
            0x07954, 0x06AA0, 0x0AD50, 0x05B52, 0x04B60, 0x0A6E6, 0x0A4E0, 0x0D260, 0x0EA65, 0x0D530,
            0x05AA0, 0x076A3, 0x096D0, 0x04BD7, 0x04AD0, 0x0A4D0, 0x1D0B6, 0x0D250, 0x0D520, 0x0DD45,
            0x0B5A0, 0x056D0, 0x055B2, 0x049B0, 0x0A577, 0x0A4B0, 0x0AA50, 0x1B255, 0x06D20, 0x0ADA0,
            0x14B63
        };

        #endregion



        #region 農曆相关数据

        private const string NStr1 = "日一二三四五六七八九";
        private const string NStr2 = "初十廿卅";

        #endregion



        #endregion

        #region 构造函数

        #region ChinaCalendar <公历日期初始化>

        /// <summary>
        /// 用一个标准的公历日期来初使化
        /// </summary>
        /// <param name="dt"></param>
        public ChineseDateTime(DateTime dt)
        {
            int i;

            ValidateDateLimit(dt);

            _date = dt.Date;


            //農曆日期计算部分
            int temp = 0;

            TimeSpan ts = _date - MinDay; //计算两天的基本差距[即1900到当天的天差]
            int offset = ts.Days;

            for (i = MinYear; i <= MaxYear; i++)
            {
                temp = GetChineseYearDays(i); //求当年農曆年天数
                if (offset - temp < 1) break;
                offset = offset - temp;
            }
            _cYear = i;

            int leap = GetChineseLeapMonth(_cYear);
            //设定当年是否有闰月
            _cIsLeapYear = leap > 0;

            _cIsLeapMonth = false;
            for (i = 1; i <= 12; i++)
            {
                //闰月
                if ((leap > 0) && (i == leap + 1) && (_cIsLeapMonth == false))
                {
                    _cIsLeapMonth = true;
                    i = i - 1;
                    temp = GetChineseLeapMonthDays(_cYear); //计算闰月天数
                }
                else
                {
                    _cIsLeapMonth = false;
                    temp = GetChineseMonthDays(_cYear, i); //计算非闰月天数
                }

                offset = offset - temp;
                if (offset <= 0) break;
            }

            offset = offset + temp;
            _cMonth = i;
            _cDay = offset;
            SetDay();
        }

        #endregion

        #region ChinaCalendar <農曆日期初始化>

        /// <summary>
        /// 用農曆的日期来初使化
        /// </summary>
        /// <param name="cy">農曆年</param>
        /// <param name="cm">農曆月</param>
        /// <param name="cd">農曆日</param>
        /// <param name="leapMonthFlag"></param>
        public ChineseDateTime(int cy, int cm, int cd, bool leapMonthFlag)
        {
            int i, temp;

            ValidateChineseDateLimit(cy, cm, cd, leapMonthFlag);

            _cYear = cy;
            _cMonth = cm;
            _cDay = cd;

            int offset = 0;

            for (i = MinYear; i < cy; i++)
            {
                temp = GetChineseYearDays(i); //求当年農曆年天数
                offset = offset + temp;
            }

            int leap = GetChineseLeapMonth(cy);
            this._cIsLeapYear = leap != 0;

            _cIsLeapMonth = cm == leap && leapMonthFlag;


            if ((_cIsLeapYear == false) || //当年没有闰月
                (cm < leap)) //计算月份小于闰月     
            {
                #region ...

                for (i = 1; i < cm; i++)
                {
                    temp = GetChineseMonthDays(cy, i); //计算非闰月天数
                    offset = offset + temp;
                }

                //检查日期是否大于最大天
                if (cd > GetChineseMonthDays(cy, cm))
                {
                    throw new ChineseDateTimeException("不合法的農曆日期,日不存在。", 11002.103);
                }
                offset = offset + cd; //加上当月的天数

                #endregion
            }
            else //是闰年，且计算月份大于或等于闰月
            {
                #region ...

                for (i = 1; i < cm; i++)
                {
                    temp = GetChineseMonthDays(cy, i); //计算非闰月天数
                    offset = offset + temp;
                }

                if (cm > leap) //计算月大于闰月
                {
                    temp = GetChineseLeapMonthDays(cy); //计算闰月天数
                    offset = offset + temp; //加上闰月天数

                    if (cd > GetChineseMonthDays(cy, cm))
                    {
                        throw new ChineseDateTimeException("不合法的農曆日期,日不存在。", 11002.103);
                    }
                    offset = offset + cd;
                }
                else //计算月等于闰月
                {
                    //如果需要计算的是闰月，则应首先加上与闰月对应的普通月的天数
                    if (this._cIsLeapMonth) //计算月为闰月
                    {
                        temp = GetChineseMonthDays(cy, cm); //计算非闰月天数
                        offset = offset + temp;
                    }

                    if (cd > GetChineseLeapMonthDays(cy))
                    {
                        throw new ChineseDateTimeException("不合法的農曆日期,日不存在。", 11002.103);
                    }
                    offset = offset + cd;
                }

                #endregion
            }


            _date = MinDay.AddDays(offset);
            SetDay();
        }

        #endregion

        #endregion

        #region 私有函数

        #region GetChineseMonthDays

        /// <summary>
        /// 传回農曆 y年m月的总天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetChineseMonthDays(int year, int month)
        {
            //0X0FFFF[0000 {1111 1111 1111} 1111]
            return BitTest32((LunarDateArray[year - MinYear] & 0x0000FFFF), (16 - month)) ? 30 : 29;
        }

        #endregion

        #region GetChineseLeapMonth

        /// <summary>
        /// 传回農曆 y年闰哪个月 1-12 , 没闰传回 0
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetChineseLeapMonth(int year)
        {
            //最后4位，即8，代表这一年的润月月份，为0则不润。首4位要与末4位搭配使用
            return LunarDateArray[year - MinYear] & 0xF;

        }

        #endregion

        #region GetChineseLeapMonthDays

        /// <summary>
        /// 传回農曆 y年闰月的天数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetChineseLeapMonthDays(int year)
        {
            if (GetChineseLeapMonth(year) != 0)
            {
                //前4位，即0在这一年是润年时才有意义，它代表这年润月的大小月。
                return (LunarDateArray[year - MinYear] & 0x10000) != 0 ? 30 : 29;
            }
            return 0;
        }

        #endregion

        #region GetChineseYearDays

        /// <summary>
        /// 取農曆年一年的天数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetChineseYearDays(int year)
        {
            int sumDay = 348;
            int i = 0x8000;
            int info = LunarDateArray[year - MinYear] & 0x0FFFF;
            //0x04BD8  & 0x0FFFF 中间12位，即4BD，每位代表一个月，为1则为大月，为0则为小月
            //计算12个月中有多少天为30天
            for (int m = 0; m < 12; m++)
            {
                int f = info & i;
                if (f != 0)
                {
                    sumDay++;
                }
                i = i >> 1;
            }
            return sumDay + GetChineseLeapMonthDays(year);
        }

        #endregion

        #region GetChineseHour

        /// <summary>
        /// 获得当前时间的时辰
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// 
        private string GetChineseHour(DateTime dt)
        {
            //string ganHour, zhiHour;

            //计算时辰的地支
            int hour = dt.Hour;
            int minute = dt.Minute;

            if (minute != 0) hour += 1;
            int offset = hour / 2;
            if (offset >= 12) offset = 0;
            //zhiHour = zhiStr[offset].ToString();

            //计算天干
            TimeSpan ts = this._date - GanZhiStartDay;
            int i = ts.Days % 60;

            int indexGan = ((i % 10 + 1) * 2 - 1) % 10 - 1;
            string tmpGan = CalendarInfo.Gan[indexGan];
            //ganHour = ganStr[((i % 10 + 1) * 2 - 1) % 10 - 1].ToString();
            // string tmpGan = GanStr.Substring(indexGan) + GanStr.Substring(0, indexGan + 2);
            return string.Format("{0}{1}時", tmpGan[offset], CalendarInfo.Zhi[offset]);

        }

        #endregion

        #region CheckDateLimit

        /// <summary>
        /// 检查公历日期是否符合要求
        /// </summary>
        /// <param name="dt"></param>
        public static void ValidateDateLimit(DateTime dt)
        {
            if ((dt < MinDay) || (dt > MaxDay)) throw new ChineseDateTimeException("超出可转换的日期",11002.1);
        }

        #endregion

        #region CheckChineseDateLimit

        /// <summary>
        /// 检查農曆日期是否合理
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="leapMonth"></param>
        public static void ValidateChineseDateLimit(int year, int month, int day, bool leapMonth)
        {
            if ((year < MinYear) || (year > MaxYear)) throw new ChineseDateTimeException("不合法的農曆日期,年份不在可轉換範圍。", 11002.101);

            if ((month < 1) || (month > 12)) throw new ChineseDateTimeException("不合法的農曆日期,月份不存在。", 11002.102);
            //中国的月最多30天
            if ((day < 1) || (day > 30)) throw new ChineseDateTimeException("不合法的農曆日期,日不存在。", 11002.103);


            int leap = GetChineseLeapMonth(year); // 计算该年应该闰哪个月
            if (leapMonth && (month != leap)) throw new ChineseDateTimeException("不合法的農曆日期,該月不為闰月。", 11002.104);



        }

        #endregion



        #region BitTest32

        /// <summary>
        /// 测试某位是否为真
        /// </summary>
        /// <param name="num"></param>
        /// <param name="bitpostion"></param>
        /// <returns></returns>
        private static bool BitTest32(int num, int bitpostion)
        {

            if ((bitpostion > 31) || (bitpostion < 0))
                throw new ChineseDateTimeException("天數不為有效值[0-31]:" + bitpostion, 11002.105);

            int bit = 1 << bitpostion;

            return (num & bit) != 0;
        }

        #endregion

      

        #region CompareWeekDayHoliday

        /// <summary>
        /// 比较当天是不是指定的第周几
        /// </summary>
        /// <param name="date"></param>
        /// <param name="month"></param>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private static bool CompareWeekDayHoliday(DateTime date, int month, int week, DayOfWeek day)
        {


            if (date.Month != month) return false;
            if (date.DayOfWeek != day) return false;
            DateTime firstDay = new DateTime(date.Year, date.Month, 1); //生成当月第一天

            int firstweekday = (int)firstDay.DayOfWeek + 1;
            int weekday = (int)day + 1;
            int firWeekDays = 8 - firstweekday; //计算第一周剩余天数
            bool ret = false;
            var weekAtMonth = week;
            if (week == 0)
            {
                weekAtMonth = (int)Math.Ceiling((DateTime.DaysInMonth(date.Year, date.Month) + firstweekday) / 7.0);

            }
            if (firstweekday > weekday)
            {
                if ((weekAtMonth - 1) * 7 + weekday + firWeekDays == date.Day)
                {
                    ret = true;
                }
            }
            else
            {
                if (weekday + firWeekDays + (weekAtMonth - 2) * 7 == date.Day)
                {
                    ret = true;
                }
            }

            return ret;
        }

        #endregion

        #endregion

        #region  属性

        #region 節日

        private void SetDay()
        {
            SetChineseCalendarHoliday();
            SetWeekDayHoliday();
            SetDateHoliday();
            SetWeekDayHoliday();
            SetChineseConstellation();
            ChineseHour = GetChineseHour(Date);
            SetTheSolarTerms();
            SetChineseTwentyFourPrevDay();
            SetChineseTwentyFourNextDay();
            SetGanZhiMonth();
        }



        #region ChineseCalendarHoliday

        /// <summary>
        /// 计算中国農曆節日
        /// </summary>
        [Category("節日"), DisplayName("農曆節日")]
        public virtual string ChineseCalendarHoliday { get; private set; }

        private void SetChineseCalendarHoliday()
        {
            if (this._cIsLeapMonth) return;
            foreach (LunarHoliday lh in CalendarInfo.LunarHolidays)
            {
                if ((lh.Month != this._cMonth) || (lh.Day != this._cDay)) continue;
                ChineseCalendarHoliday = lh.HolidayName;
                break;
            }  //对除夕进行特别处理
            if (this._cMonth != 12) return;

            int lastday = GetChineseMonthDays(this._cYear, 12); //计算当年農曆12月的总天数
            if (this._cDay == lastday) //如果为最后一天
            {
                ChineseCalendarHoliday = "除夕";
            }
        }

        #endregion

        #region WeekDayHoliday

        /// <summary>
        /// 按某月第几周第几日计算的節日
        /// </summary>
        [Category("節日"), DisplayName("星期節日")]
        public string WeekDayHoliday { get; private set; }

        private void SetWeekDayHoliday()
        {

            foreach (WeekHoliday wh in CalendarInfo.WeekHolidays)
            {
                if (!CompareWeekDayHoliday(_date, wh.Month, wh.WeekAtMonth, wh.WeekDay)) continue;
                WeekDayHoliday = wh.HolidayName;
                break;
            }

        }

        #endregion

        #region DateHoliday

        /// <summary>
        /// 按公历日计算的節日
        /// </summary>
        [Category("節日"), DisplayName("公曆節日")]
        public string DateHoliday { get; private set; }

        private void SetDateHoliday()
        {


            foreach (SolarHoliday sh in CalendarInfo.SolarHolidays)
            {
                if ((sh.Month != _date.Month) || (sh.Day != _date.Day)) continue;
                DateHoliday = sh.HolidayName;
                break;
            }

        }

        #endregion
        #endregion

        #region 公历日期
        #region Date
        /// <summary>
        /// 取对应的公历日期
        /// </summary>
        [Category("公曆"), DisplayName("公曆日期")]
        public DateTime Date
        {
            get { return _date; }

        }
        /// <summary>
        /// 取对应的公历日期
        /// </summary>
        [Category("公曆"), DisplayName("公曆星期")]
        public DayOfWeek WeekDay
        {
            get { return _date.DayOfWeek; }

        }
        #region Constellation
        /// <summary>
        /// 计算指定日期的星座序号 
        /// </summary>
        /// <returns></returns>
        [Category("公曆"), DisplayName("星座")]
        public string Constellation
        {
            get
            {
                int index;
                int m = _date.Month;
                int d = _date.Day;
                int y = m * 100 + d;

                if (((y >= 321) && (y <= 419))) { index = 0; }
                else if ((y >= 420) && (y <= 520)) { index = 1; }
                else if ((y >= 521) && (y <= 620)) { index = 2; }
                else if ((y >= 621) && (y <= 722)) { index = 3; }
                else if ((y >= 723) && (y <= 822)) { index = 4; }
                else if ((y >= 823) && (y <= 922)) { index = 5; }
                else if ((y >= 923) && (y <= 1022)) { index = 6; }
                else if ((y >= 1023) && (y <= 1121)) { index = 7; }
                else if ((y >= 1122) && (y <= 1221)) { index = 8; }
                else if ((y >= 1222) || (y <= 119)) { index = 9; }
                else if ((y >= 120) && (y <= 218)) { index = 10; }
                else if ((y >= 219) && (y <= 320)) { index = 11; }
                else { index = 0; }

                return CalendarInfo.ConstellationName[index];
            }
        }
        #endregion
        #endregion

      








        #region ChineseConstellation
        /// <summary>
        /// 28星宿
        /// </summary>
        [Category("天干地支"), DisplayName("廿八星宿")]
        public string ChineseConstellation { get; private set; }
        /// <summary>
        /// 28星宿動物
        /// </summary>
        [Category("天干地支"), DisplayName("廿八星宿動物")]
        public string ChineseConstellationAnimal { get; private set; }
        private void SetChineseConstellation()
        {
            TimeSpan ts = this._date - ChineseConstellationReferDay;
            int offset = ts.Days;
            int modStarDay = offset % 28;
            if (modStarDay < 0)
            {
                ChineseConstellation = CalendarInfo.ChineseConstellationName[27 + modStarDay];
                ChineseConstellationAnimal = CalendarInfo.ChineseConstellationAnimalName[27 + modStarDay];
            }
            else
            {
                ChineseConstellation = CalendarInfo.ChineseConstellationName[modStarDay];
                ChineseConstellationAnimal = CalendarInfo.ChineseConstellationAnimalName[modStarDay];
            }
        }
        #endregion

        #region ChineseHour
        /// <summary>
        /// 时辰
        /// </summary>   
        [Category("天干地支"), DisplayName("天干地支时辰")]
        public virtual string ChineseHour { get; private set; }
        #endregion

        #endregion

        #region 農曆日期

        /// <summary>
        /// 當前日期
        /// </summary>
        [Category("農曆"), DisplayName("當前日期")]
        public static ChineseDateTime Now
        {
            get { return new ChineseDateTime(DateTime.Now); }
        }
        #region IsChineseLeapMonth
        /// <summary>
        /// 是否闰月
        /// </summary>
        [Category("農曆"), DisplayName("是否闰月")]
        public virtual bool IsChineseLeapMonth
        {
            get { return this._cIsLeapMonth; }
        }
        #endregion

        #region IsChineseLeapYear
        /// <summary>
        /// 当年是否有闰月
        /// </summary>
        [Category("農曆"), DisplayName("是否有闰月")]
        public virtual bool IsChineseLeapYear
        {
            get
            {
                return this._cIsLeapYear;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        [Category("農曆"), DisplayName("農曆")]
        public virtual string ChineseDate
        {
            get { return string.Format("{0}{1}{2}", this._cIsLeapMonth ? "闰" : string.Empty, ChineseMonth, ChineseDay); }
        }

        #region ChineseDayString
        /// <summary>
        /// 農曆日中文表示
        /// </summary>
        [Category("農曆"), DisplayName("農曆日")]
        public virtual string ChineseDay
        {
            get
            {
                switch (this._cDay)
                {
                    case 0:
                        return "";
                    case 10:
                        return "初十";
                    case 20:
                        return "二十";
                    case 30:
                        return "三十";
                    default:
                        return string.Format("{0}{1}", NStr2[_cDay / 10], NStr1[_cDay % 10]);

                }
            }
        }
        #endregion



        #region ChineseMonthString
        /// <summary>
        /// 農曆月份字符串
        /// </summary>
        [Category("農曆"), DisplayName("農曆月份")]
        public virtual string ChineseMonth
        {
            get
            {
                if (this._cMonth < 0 || this._cMonth > 12) throw new ChineseDateTimeException("不合法的農曆日期,月份不存在。", 11002.102);
                return CalendarInfo.ChineseMonths[this._cMonth - 1];
            }
        }
        #endregion







        #region ChineseTwentyFourDay


        /// <summary>
        /// 定气法计算二十四節氣,二十四節氣是按地球公转来计算的，并非是阴历计算的
        /// </summary>
        /// <remarks>
        /// 節氣的定法有两种。古代历法采用的称为"恒气"，即按时间把一年等分为24份，
        /// 每一節氣平均得15天有余，所以又称"平气"。现代農曆采用的称为"定气"，即
        /// 按地球在轨道上的位置为标准，一周360°，两節氣之间相隔15°。由于冬至时地
        /// 球位于近日点附近，运动速度较快，因而太阳在黄道上移动15°的时间不到15天。
        /// 夏至前后的情况正好相反，太阳在黄道上移动较慢，一个節氣达16天之多。采用
        /// 定气时可以保证春、秋两分必然在昼夜平分的那两天。
        /// </remarks>
        [Category("二十四節氣"), DisplayName("當前節氣")]
        public virtual string TheSolarTerms { get; private set; }

        static readonly DateTime BaseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
        private void SetTheSolarTerms()
        {

            string tempStr = string.Empty;

            double y = 525948.76 * (this._date.Year - 1900);

            for (int i = 1; i <= 24; i++)
            {
                double num = y + CalendarInfo.TheSolarTermsHolidays[i - 1].TheSolarTermsData;

                DateTime newDate = BaseDateAndTime.AddMinutes(num);
                if (newDate.DayOfYear != _date.DayOfYear) continue;


                tempStr = CalendarInfo.TheSolarTermsHolidays[i - 1].HolidayName;
                break;
            }
            TheSolarTerms = tempStr;

        }

        private void SetChineseTwentyFourPrevDay()
        {

            string tempStr = string.Empty;

            double y = 525948.76 * (this._date.Year - 1900);

            for (int i = 24; i >= 1; i--)
            {
                double num = y + CalendarInfo.STermInfo[i - 1];

                DateTime newDate = BaseDateAndTime.AddMinutes(num);

                if (newDate.DayOfYear >= _date.DayOfYear) continue;
                tempStr = string.Format("{0}[{1}]", CalendarInfo.TheSolarTermsHolidays[i - 1].HolidayName, newDate.ToString("yyyy-MM-dd"));
                break;
            }
            ChineseTwentyFourPrevDay = tempStr;
        }

        /// <summary>
        /// 当前日期前一个最近節氣
        /// </summary>
        [Category("二十四節氣"), DisplayName("前一个節氣")]
        public string ChineseTwentyFourPrevDay { get; private set; }

        private void SetChineseTwentyFourNextDay()
        {

            string tempStr = string.Empty;

            double y = 525948.76 * (this._date.Year - 1900);

            for (int i = 1; i <= 24; i++)
            {
                double num = y + CalendarInfo.STermInfo[i - 1];

                DateTime newDate = BaseDateAndTime.AddMinutes(num);

                if (newDate.DayOfYear <= _date.DayOfYear) continue;
                tempStr = string.Format("{0}[{1}]", CalendarInfo.TheSolarTermsHolidays[i - 1].HolidayName, newDate.ToString("yyyy-MM-dd"));
                break;
            }
            ChineseTwentyFourNextDay = tempStr;
        }

        /// <summary>
        /// 当前日期后一个最近節氣
        /// </summary>
        [Category("二十四節氣"), DisplayName("后一个節氣")]
        public string ChineseTwentyFourNextDay { get; private set; }
        #endregion
        #endregion



        #region 属相


        #region AnimalString
        /// <summary>
        /// 取属相字符串
        /// </summary> 
        [Category("農曆"), DisplayName("十二生肖")]
        public virtual string ChineseZodiac
        {
            get
            {
                // int offset = _date.Year - AnimalStartYear; //阳历计算
                int offset = this._cYear - AnimalStartYear;//　農曆计算
                return CalendarInfo.ChineseZodiac[offset % 12];
            }
        }
        #endregion
        #endregion

        #region 天干地支
        #region Chinese era
        /// <summary>
        /// 取農曆年的干支表示法如 乙丑年
        /// </summary>
        [Category("天干地支"), DisplayName("天干地支年")]
        public virtual string ChineseEraYear
        {
            get
            {
                int i = (this._cYear - GanZhiStartYear) % 60; //计算干支
                return string.Format("{0}{1}年", CalendarInfo.Gan[i % 10], CalendarInfo.Zhi[i % 12]);

            }
        }
        #endregion

        #region GanZhiMonthString
        /// <summary>
        /// 取干支的月表示字符串，注意農曆的闰月不记干支
        /// </summary>
        [Category("天干地支"), DisplayName("天干地支月")]
        public virtual string ChineseEraMonth { get; private set; }

        private void SetGanZhiMonth()
        {
            //每个月的地支总是固定的,而且总是从寅月开始
            int zhiIndex;
            if (this._cMonth > 10)
            {
                zhiIndex = this._cMonth - 10;
            }
            else
            {
                zhiIndex = this._cMonth + 2;
            }
            string zhi = CalendarInfo.Zhi[zhiIndex - 1];

            //根据当年的干支年的干来计算月干的第一个
            int ganIndex = 1;
            int i = (this._cYear - GanZhiStartYear) % 60; //计算干支
            switch (i % 10)
            {
                #region ...
                case 0: //甲
                    ganIndex = 3;
                    break;
                case 1: //乙
                    ganIndex = 5;
                    break;
                case 2: //丙
                    ganIndex = 7;
                    break;
                case 3: //丁
                    ganIndex = 9;
                    break;
                case 4: //戊
                    ganIndex = 1;
                    break;
                case 5: //己
                    ganIndex = 3;
                    break;
                case 6: //庚
                    ganIndex = 5;
                    break;
                case 7: //辛
                    ganIndex = 7;
                    break;
                case 8: //壬
                    ganIndex = 9;
                    break;
                case 9: //癸
                    ganIndex = 1;
                    break;
                #endregion
            }
            string gan = CalendarInfo.Gan[(ganIndex + this._cMonth - 2) % 10];

            ChineseEraMonth = gan + zhi + "月";
        }

        #endregion

        #region GanZhiDayString
        /// <summary>
        /// 取干支日表示法
        /// </summary>
        [Category("天干地支"), DisplayName("天干地支日")]
        public virtual string ChineseEraDay
        {
            get
            {
                TimeSpan ts = this._date - GanZhiStartDay;
                int offset = ts.Days;
                int i = offset % 60;
                return CalendarInfo.Gan[i % 10] + CalendarInfo.Zhi[i % 12] + "日";
            }
        }
        #endregion

        #region GanZhiDateString
        /// <summary>
        /// 取当前日期的干支表示法如 甲子年乙丑月丙庚日
        /// </summary>
        [Category("天干地支"), DisplayName("天干地支八字")]
        public virtual string ChineseEraDate
        {
            get
            {
                return string.Format("{0} {1} {2} {3}", ChineseEraYear, ChineseEraMonth, ChineseEraDay, ChineseHour);
            }
        }
        #endregion
        #endregion
        #endregion

       
    }
}
