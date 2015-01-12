using System;

namespace Library.Date
{
    #region ChineseCalendarException
    /// <summary>
    /// 中国日历异常处理
    /// </summary>
    public class ChineseCalendarException : LibException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public ChineseCalendarException(string msg)
            : base(msg)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IHoliday
    {
        /// <summary>
        /// 月
        /// </summary>
        int Month { get; }
        /// <summary>
        /// 日
        /// </summary>
        int Day { get; }

        /// <summary>
        /// 節日名稱
        /// </summary>
        string HolidayName { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct SolarHoliday : IHoliday
    {
        public int Month { get; private set; }
        public int Day { get; private set; }

        public string HolidayName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="name"></param>
        public SolarHoliday(int month, int day, string name)
            : this()
        {
            Month = month;
            Day = day;
            HolidayName = name;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct LunarHoliday : IHoliday
    {
        public int Month { get; private set; }
        public int Day { get; private set; }
        public string HolidayName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="name"></param>
        public LunarHoliday(int month, int day, string name)
            : this()
        {
            Month = month;
            Day = day;

            HolidayName = name;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct WeekHoliday
    {
        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; private set; }
        /// <summary>
        /// 第幾個星期
        /// </summary>
        public int WeekAtMonth { get; private set; }
        /// <summary>
        /// 星期
        /// </summary>
        public int WeekDay { get; private set; }
        /// <summary>
        /// 節日名稱
        /// </summary>
        public string HolidayName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="weekAtMonth"></param>
        /// <param name="weekDay"></param>
        /// <param name="name"></param>
        public WeekHoliday(int month, int weekAtMonth, int weekDay, string name)
            : this()
        {
            Month = month;
            WeekAtMonth = weekAtMonth;
            WeekDay = weekDay;
            HolidayName = name;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class CalendarInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly string[] MonthString =
        {
              "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "腊月"
        };
        /// <summary>
        /// 天幹
        /// </summary>
        public static readonly string[] Gan = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        /// <summary>
        /// 地支
        /// </summary>
        public static readonly string[] Zhi = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        /// <summary>
        /// 生肖
        /// </summary>
        public static readonly string[] Animal = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        #region 星座名称
        /// <summary>
        /// 星座名称
        /// </summary>
        public static readonly string[] ConstellationName = { 
                    "白羊座", "金牛座", "双子座", 
                    "巨蟹座", "狮子座", "处女座", 
                    "天秤座", "天蝎座", "射手座", 
                    "摩羯座", "水瓶座", "双鱼座"
                };
        #endregion

        #region 二十四节气
        /// <summary>
        /// 二十四节气
        /// </summary>
        public static readonly string[] LunarHolidayName = 
                    { 
                    "小寒", "大寒", "立春", "雨水", 
                    "惊蛰", "春分", "清明", "谷雨", 
                    "立夏", "小满", "芒种", "夏至", 
                    "小暑", "大暑", "立秋", "处暑", 
                    "白露", "秋分", "寒露", "霜降", 
                    "立冬", "小雪", "大雪", "冬至"
                    };
        #endregion

        #region 二十八星宿
        /// <summary>
        /// 二十八星宿
        /// </summary>
        public readonly static string[] ChineseConstellationName =
            {
                  //四        五      六         日        一      二      三  
                "角木蛟","亢金龙","女土蝠","房日兔","心月狐","尾火虎","箕水豹",
                "斗木獬","牛金牛","氐土貉","虚日鼠","危月燕","室火猪","壁水獝",
                "奎木狼","娄金狗","胃土彘","昴日鸡","毕月乌","觜火猴","参水猿",
                "井木犴","鬼金羊","柳土獐","星日马","张月鹿","翼火蛇","轸水蚓" 
            };
        #endregion

        #region 节气数据
        /// <summary>
        /// 节气数据
        /// </summary>
        public readonly static string[] SolarTerm = new string[] { "小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至" };
        internal static readonly int[] STermInfo = new int[] { 0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };
        #endregion

        #region 按公历计算的节日
        /// <summary>
        /// 按公历计算的节日
        /// </summary>
        public static readonly SolarHoliday[] SolarHolidays = new SolarHoliday[]{
            new SolarHoliday(1, 1, "元旦"),
            new SolarHoliday(2, 2,  "世界湿地日"),
            new SolarHoliday(2, 10,  "国际气象节"),
            new SolarHoliday(2, 14,  "情人节"),
            new SolarHoliday(3, 1,  "国际海豹日"),
            new SolarHoliday(3, 5,  "学雷锋纪念日"),
            new SolarHoliday(3, 8,  "妇女节"), 
            new SolarHoliday(3, 12,  "植树节 孙中山逝世纪念日"), 
            new SolarHoliday(3, 14,  "国际警察日"),
            new SolarHoliday(3, 15,  "消费者权益日"),
            new SolarHoliday(3, 17,  "中国国医节 国际航海日"),
            new SolarHoliday(3, 21,  "世界森林日 消除种族歧视国际日 世界儿歌日"),
            new SolarHoliday(3, 22,  "世界水日"),
            new SolarHoliday(3, 24,  "世界防治结核病日"),
            new SolarHoliday(4, 1,  "愚人节"),
            new SolarHoliday(4, 7,  "世界卫生日"),
            new SolarHoliday(4, 22,  "世界地球日"),
            new SolarHoliday(5, 1,  "劳动节"),  
            new SolarHoliday(5, 4,  "青年节"), 
            new SolarHoliday(5, 8,  "世界红十字日"),
            new SolarHoliday(5, 12,  "国际护士节"), 
            new SolarHoliday(5, 31,  "世界无烟日"), 
            new SolarHoliday(6, 1,  "国际儿童节"), 
            new SolarHoliday(6, 5,  "世界环境保护日"),
            new SolarHoliday(6, 26,  "国际禁毒日"),
            new SolarHoliday(7, 1,  "建党节 香港回归纪念 世界建筑日"),
            new SolarHoliday(7, 11,  "世界人口日"),
            new SolarHoliday(8, 1,  "建军节"), 
          //  new SolarHoliday(8, 8,  "中国男子节 父亲节"),
            new SolarHoliday(8, 15,  "抗日战争胜利纪念"),
            new SolarHoliday(9, 9,  "  逝世纪念"), 
            new SolarHoliday(9, 10,  "教师节"), 
            new SolarHoliday(9, 18,  "九·一八事变纪念日"),
            new SolarHoliday(9, 20,  "国际爱牙日"),
            new SolarHoliday(9, 27,  "世界旅游日"),
            new SolarHoliday(9, 28,  "孔子诞辰"),
            new SolarHoliday(10, 1,  "国庆节"),
            new SolarHoliday(10, 6,  "老人节"), 
            new SolarHoliday(10, 24,  "联合国日"),
            new SolarHoliday(11, 10,  "世界青年节"),
            new SolarHoliday(11, 12,  "孙中山诞辰纪念"), 
            new SolarHoliday(12, 1,  "世界艾滋病日"), 
            new SolarHoliday(12, 3,  "世界残疾人日"), 
            new SolarHoliday(12, 20,  "澳门回归纪念"), 
            new SolarHoliday(12, 24,  "平安夜"), 
            new SolarHoliday(12, 25,  "圣诞节"), 
            new SolarHoliday(12, 26,  " 诞辰纪念")
           };
        #endregion

        #region 按农历计算的节日
        /// <summary>
        /// 按农历计算的节日
        /// </summary>
        public static readonly LunarHoliday[] LunarHolidays =
        {
            new LunarHoliday(1, 1,  "春节"), 
            new LunarHoliday(1, 15,  "元宵节"), 
            new LunarHoliday(5, 5,  "端午节"), 
            new LunarHoliday(7, 7,  "七夕情人节"),
            new LunarHoliday(7, 15,  "中元节 盂兰盆节"), 
            new LunarHoliday(8, 15,  "中秋节"), 
            new LunarHoliday(9, 9, "重阳节"), 
            new LunarHoliday(12, 8,  "腊八节"),
            new LunarHoliday(12, 23, "北方小年(扫房)"),
            new LunarHoliday(12, 24,  "南方小年(掸尘)"),
            new LunarHoliday(12, 30,  "除夕")  //注意除夕需要其它方法进行计算
        };
        #endregion

        #region 按某月第几个星期几
        /// <summary>
        /// 按某月第几个星期几
        /// </summary>
        public static readonly WeekHoliday[] WeekHolidays = new WeekHoliday[]{
            new WeekHoliday(5, 2, 1, "母亲节"), 
            new WeekHoliday(5, 3, 1, "全国助残日"), 
            new WeekHoliday(6, 3, 1, "父亲节"), 
            new WeekHoliday(9, 3, 3, "国际和平日"), 
            new WeekHoliday(9, 4, 1, "国际聋人节"), 
            new WeekHoliday(10, 1, 2, "国际住房日"), 
            new WeekHoliday(10, 1, 4, "国际减轻自然灾害日"),
            new WeekHoliday(11, 4, 5, "感恩节")
        };
        #endregion
    }
    #endregion

    /// <summary>
    /// 中国农历类 版本V1.0 支持 1900.1.31日起至 2049.12.31日止的数据
    /// </summary>
    /// <remarks>
    /// 本程序使用数据来源于网上的万年历查询，并综合了一些其它数据
    /// </remarks>
    public class ChineseCalendar
    {
        #region 内部结构

        #endregion

        #region 内部变量

        private DateTime _date;


        private readonly int _cYear;
        private readonly int _cMonth; //农历月份
        private readonly int _cDay; //农历当月第几天
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
        /// 来源于网上的农历数据
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



        #region 农历相关数据

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
        public ChineseCalendar(DateTime dt)
        {
            int i;

            ValidateDateLimit(dt);

            _date = dt.Date;


            //农历日期计算部分
            int temp = 0;

            TimeSpan ts = _date - MinDay; //计算两天的基本差距[即1900到当天的天差]
            int offset = ts.Days;

            for (i = MinYear; i <= MaxYear; i++)
            {
                temp = GetChineseYearDays(i); //求当年农历年天数
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

        #region ChinaCalendar <农历日期初始化>

        /// <summary>
        /// 用农历的日期来初使化
        /// </summary>
        /// <param name="cy">农历年</param>
        /// <param name="cm">农历月</param>
        /// <param name="cd">农历日</param>
        /// <param name="leapMonthFlag"></param>
        public ChineseCalendar(int cy, int cm, int cd, bool leapMonthFlag)
        {
            int i, temp;

            ValidateChineseDateLimit(cy, cm, cd, leapMonthFlag);

            _cYear = cy;
            _cMonth = cm;
            _cDay = cd;

            int offset = 0;

            for (i = MinYear; i < cy; i++)
            {
                temp = GetChineseYearDays(i); //求当年农历年天数
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
                    throw new ChineseCalendarException("不合法的农历日期");
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
                        throw new ChineseCalendarException("不合法的农历日期");
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
                        throw new ChineseCalendarException("不合法的农历日期");
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
        /// 传回农历 y年m月的总天数
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
        /// 传回农历 y年闰哪个月 1-12 , 没闰传回 0
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
        /// 传回农历 y年闰月的天数
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
        /// 取农历年一年的天数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private int GetChineseYearDays(int year)
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
            return string.Format("{0}{1}", tmpGan[offset], CalendarInfo.Zhi[offset]);

        }

        #endregion

        #region CheckDateLimit

        /// <summary>
        /// 检查公历日期是否符合要求
        /// </summary>
        /// <param name="dt"></param>
        public static void ValidateDateLimit(DateTime dt)
        {
            if ((dt < MinDay) || (dt > MaxDay))
            {
                throw new ChineseCalendarException("超出可转换的日期");
            }
        }

        #endregion

        #region CheckChineseDateLimit

        /// <summary>
        /// 检查农历日期是否合理
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="leapMonth"></param>
        public static void ValidateChineseDateLimit(int year, int month, int day, bool leapMonth)
        {
            if ((year < MinYear) || (year > MaxYear))
            {
                throw new ChineseCalendarException("非法农历日期");
            }
            if ((month < 1) || (month > 12))
            {
                throw new ChineseCalendarException("非法农历日期");
            }
            if ((day < 1) || (day > 30)) //中国的月最多30天
            {
                throw new ChineseCalendarException("非法农历日期");
            }

            int leap = GetChineseLeapMonth(year); // 计算该年应该闰哪个月
            if (leapMonth && (month != leap))
            {
                throw new ChineseCalendarException("非法农历日期");
            }


        }

        #endregion

        #region ConvertNumToChineseNum

        ///// <summary>
        ///// 将0-9转成汉字形式
        ///// </summary>
        ///// <param name="n"></param>
        ///// <returns></returns>
        //private string ConvertNumToChineseNum(char n)
        //{
        //    if ((n < '0') || (n > '9')) return "";
        //    switch (n)
        //    {
        //        case '0':
        //            return HZNum[0].ToString(CultureInfo.InvariantCulture);
        //        case '1':
        //            return HZNum[1].ToString(CultureInfo.InvariantCulture);
        //        case '2':
        //            return HZNum[2].ToString(CultureInfo.InvariantCulture);
        //        case '3':
        //            return HZNum[3].ToString(CultureInfo.InvariantCulture);
        //        case '4':
        //            return HZNum[4].ToString(CultureInfo.InvariantCulture);
        //        case '5':
        //            return HZNum[5].ToString(CultureInfo.InvariantCulture);
        //        case '6':
        //            return HZNum[6].ToString(CultureInfo.InvariantCulture);
        //        case '7':
        //            return HZNum[7].ToString(CultureInfo.InvariantCulture);
        //        case '8':
        //            return HZNum[8].ToString(CultureInfo.InvariantCulture);
        //        case '9':
        //            return HZNum[9].ToString(CultureInfo.InvariantCulture);
        //        default:
        //            return string.Empty;
        //    }
        //}

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
                throw new Exception("Error Param: bitpostion[0-31]:" + bitpostion);

            int bit = 1 << bitpostion;

            return (num & bit) != 0;
        }

        #endregion

        #region ConvertDayOfWeek

        /// <summary>
        /// 将星期几转成数字表示
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        private static int ConvertDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 1;
                case DayOfWeek.Monday:
                    return 2;
                case DayOfWeek.Tuesday:
                    return 3;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 5;
                case DayOfWeek.Friday:
                    return 6;
                case DayOfWeek.Saturday:
                    return 7;
                default:
                    return 0;
            }
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
        private static bool CompareWeekDayHoliday(DateTime date, int month, int week, int day)
        {


            if (date.Month != month) return false;
            if (ConvertDayOfWeek(date.DayOfWeek) != day) return false;
            DateTime firstDay = new DateTime(date.Year, date.Month, 1); //生成当月第一天
            int i = ConvertDayOfWeek(firstDay.DayOfWeek);
            int firWeekDays = 7 - ConvertDayOfWeek(firstDay.DayOfWeek) + 1; //计算第一周剩余天数
            bool ret = false;
            if (i > day)
            {
                if ((week - 1) * 7 + day + firWeekDays == date.Day)
                {
                    ret = true;
                }
            }
            else
            {
                if (day + firWeekDays + (week - 2) * 7 == date.Day)
                {
                    ret = true;
                }
            }

            return ret;
        }

        #endregion

        #endregion

        #region  属性

        #region 节日

        private void SetDay()
        {
            SetChineseCalendarHoliday();
            SetWeekDayHoliday();
            SetDateHoliday();
            SetWeekDayHoliday();
            SetChineseConstellation();
            ChineseHour = GetChineseHour(Date);
            // SetConstellation();
            SetGanZhiMonth();
        }



        #region ChineseCalendarHoliday

        /// <summary>
        /// 计算中国农历节日
        /// </summary>
        public string ChineseCalendarHoliday { get; private set; }

        private void SetChineseCalendarHoliday()
        {

            if (this._cIsLeapMonth)
            {
                foreach (LunarHoliday lh in CalendarInfo.LunarHolidays)
                {
                    if ((lh.Month != this._cMonth) || (lh.Day != this._cDay)) continue;
                    ChineseCalendarHoliday = lh.HolidayName;
                    break;
                }
            }

            //对除夕进行特别处理
            if (this._cMonth != 12) return;

            int i = GetChineseMonthDays(this._cYear, 12); //计算当年农历12月的总天数
            if (this._cDay == i) //如果为最后一天
            {
                ChineseCalendarHoliday = "除夕";
            }

        }

        #endregion

        #region WeekDayHoliday

        /// <summary>
        /// 按某月第几周第几日计算的节日
        /// </summary>
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
        /// 按公历日计算的节日
        /// </summary>
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
        public DateTime Date
        {
            get { return _date; }

        }
        #endregion

        //#region WeekDay
        ///// <summary>
        ///// 取星期几
        ///// </summary>
        //public DayOfWeek WeekDay
        //{
        //    get { return _date.DayOfWeek; }
        //}
        //#endregion





        //#region IsLeapYear
        ///// <summary>
        ///// 当前是否公历闰年
        ///// </summary>
        //public bool IsLeapYear
        //{
        //    get
        //    {
        //        return DateTime.IsLeapYear(this._date.Year);
        //    }
        //}
        //#endregion

        #region ChineseConstellation
        /// <summary>
        /// 28星宿计算
        /// </summary>
        public string ChineseConstellation { get; private set; }

        private void SetChineseConstellation()
        {
            TimeSpan ts = this._date - ChineseConstellationReferDay;
            int offset = ts.Days;
            int modStarDay = offset % 28;
            ChineseConstellation = (modStarDay >= 0 ?
                CalendarInfo.ChineseConstellationName[modStarDay] :
                CalendarInfo.ChineseConstellationName[27 + modStarDay]);
        }
        #endregion

        #region ChineseHour
        /// <summary>
        /// 时辰
        /// </summary>
        public string ChineseHour { get; private set; }
        #endregion

        #endregion

        #region 农历日期
        #region IsChineseLeapMonth
        /// <summary>
        /// 是否闰月
        /// </summary>
        public bool IsChineseLeapMonth
        {
            get { return this._cIsLeapMonth; }
        }
        #endregion

        #region IsChineseLeapYear
        /// <summary>
        /// 当年是否有闰月
        /// </summary>
        public bool IsChineseLeapYear
        {
            get
            {
                return this._cIsLeapYear;
            }
        }
        #endregion

        //#region ChineseDay
        ///// <summary>
        ///// 农历日
        ///// </summary>
        //public int Day
        //{
        //    get { return this._cDay; }
        //}
        //#endregion

        #region ChineseDayString
        /// <summary>
        /// 农历日中文表示
        /// </summary>
        public string ChineseDay
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

        //#region ChineseMonth
        ///// <summary>
        ///// 农历的月份
        ///// </summary>
        //public int Month
        //{
        //    get { return this._cMonth; }
        //}
        //#endregion

        #region ChineseMonthString
        /// <summary>
        /// 农历月份字符串
        /// </summary>
        public string ChineseMonth
        {
            get
            {
                if (this._cMonth < 0 || this._cMonth > 12) throw new ChineseCalendarException("月份無效");
                return CalendarInfo.MonthString[this._cMonth - 1];
            }
        }
        #endregion

        //#region ChineseYear
        ///// <summary>
        ///// 取农历年份
        ///// </summary>
        //public int Year
        //{
        //    get { return this._cYear; }
        //}
        //#endregion

        #region ChineseYearString
        ///// <summary>
        ///// 取农历年字符串如，一九九七年
        ///// </summary>
        //public string ChineseYear
        //{
        //    get
        //    {
        //        string tempStr = "";
        //        string num = this._cYear.ToString(CultureInfo.InvariantCulture);
        //        for (int i = 0; i < 4; i++)
        //        {
        //            tempStr += ConvertNumToChineseNum(num[i]);
        //        }
        //        return tempStr + "年";
        //    }
        //}
        #endregion

        #region ChineseDateString
        ///// <summary>
        ///// 取农历日期表示法：农历一九九七年正月初五
        ///// </summary>
        //public string ChineseDate
        //{
        //    get
        //    {
        //        return string.Format(
        //            this._cIsLeapMonth ? "农历{0}闰{1}{2}" : "农历{0}{1}{2}",
        //            ChineseYear, ChineseMonth, ChineseDay);
        //    }
        //}
        #endregion

        #region ChineseTwentyFourDay
        /// <summary>
        /// 定气法计算二十四节气,二十四节气是按地球公转来计算的，并非是阴历计算的
        /// </summary>
        /// <remarks>
        /// 节气的定法有两种。古代历法采用的称为"恒气"，即按时间把一年等分为24份，
        /// 每一节气平均得15天有余，所以又称"平气"。现代农历采用的称为"定气"，即
        /// 按地球在轨道上的位置为标准，一周360°，两节气之间相隔15°。由于冬至时地
        /// 球位于近日点附近，运动速度较快，因而太阳在黄道上移动15°的时间不到15天。
        /// 夏至前后的情况正好相反，太阳在黄道上移动较慢，一个节气达16天之多。采用
        /// 定气时可以保证春、秋两分必然在昼夜平分的那两天。
        /// </remarks>
        public string ChineseTwentyFourDay
        {
            get
            {
                DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                string tempStr = string.Empty;

                int y = this._date.Year;

                for (int i = 1; i <= 24; i++)
                {
                    double num = 525948.76 * (y - 1900) + CalendarInfo.STermInfo[i - 1];

                    DateTime newDate = baseDateAndTime.AddMinutes(num);
                    if (newDate.DayOfYear != _date.DayOfYear) continue;
                    tempStr = CalendarInfo.SolarTerm[i - 1];
                    break;
                }
                return tempStr;
            }
        }

        /// <summary>
        /// 当前日期前一个最近节气
        /// </summary>
        /// <returns></returns>
        public string GetChineseTwentyFourPrevDay()
        {

            DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
            string tempStr = "";

            int y = this._date.Year;

            for (int i = 24; i >= 1; i--)
            {
                double num = 525948.76 * (y - 1900) + CalendarInfo.STermInfo[i - 1];

                DateTime newDate = baseDateAndTime.AddMinutes(num);

                if (newDate.DayOfYear >= _date.DayOfYear) continue;
                tempStr = string.Format("{0}[{1}]", CalendarInfo.SolarTerm[i - 1], newDate.ToString("yyyy-MM-dd"));
                break;
            }

            return tempStr;


        }

        /// <summary>
        /// 当前日期后一个最近节气
        /// </summary>
        /// <returns></returns>
        public string GetChineseTwentyFourNextDay()
        {

            DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
            string tempStr = "";

            int y = this._date.Year;

            for (int i = 1; i <= 24; i++)
            {
                double num = 525948.76 * (y - 1900) + CalendarInfo.STermInfo[i - 1];

                DateTime newDate = baseDateAndTime.AddMinutes(num);

                if (newDate.DayOfYear > _date.DayOfYear)
                {
                    tempStr = string.Format("{0}[{1}]", CalendarInfo.SolarTerm[i - 1], newDate.ToString("yyyy-MM-dd"));
                    break;
                }
            }
            return tempStr;


        }
        #endregion
        #endregion

        #region 星座
        //#region Constellation
        ///// <summary>
        ///// 计算指定日期的星座序号 
        ///// </summary>
        ///// <returns></returns>
        //public string Constellation { get; private set; }

        //private void SetConstellation()
        //{

        //    int index;
        //    int m = _date.Month;
        //    int d = _date.Day;
        //    int y = m * 100 + d;

        //    if (((y >= 321) && (y <= 419))) { index = 0; }
        //    else if ((y >= 420) && (y <= 520)) { index = 1; }
        //    else if ((y >= 521) && (y <= 620)) { index = 2; }
        //    else if ((y >= 621) && (y <= 722)) { index = 3; }
        //    else if ((y >= 723) && (y <= 822)) { index = 4; }
        //    else if ((y >= 823) && (y <= 922)) { index = 5; }
        //    else if ((y >= 923) && (y <= 1022)) { index = 6; }
        //    else if ((y >= 1023) && (y <= 1121)) { index = 7; }
        //    else if ((y >= 1122) && (y <= 1221)) { index = 8; }
        //    else if ((y >= 1222) || (y <= 119)) { index = 9; }
        //    else if ((y >= 120) && (y <= 218)) { index = 10; }
        //    else if ((y >= 219) && (y <= 320)) { index = 11; }
        //    else { index = 0; }

        //    Constellation = CalendarInfo.ConstellationName[index];
        //}
        //#endregion
        #endregion

        #region 属相


        #region AnimalString
        /// <summary>
        /// 取属相字符串
        /// </summary>
        public string Animal
        {
            get
            {
                // int offset = _date.Year - AnimalStartYear; //阳历计算
                int offset = this._cYear - AnimalStartYear;//　农历计算
                return CalendarInfo.Animal[offset % 12];
            }
        }
        #endregion
        #endregion

        #region 天干地支
        #region GanZhiYearString
        /// <summary>
        /// 取农历年的干支表示法如 乙丑年
        /// </summary>
        public string GanZhiYear
        {
            get
            {
                int i = (this._cYear - GanZhiStartYear) % 60; //计算干支
                string tempStr = string.Format("{0}{1}年", CalendarInfo.Gan[i % 10], CalendarInfo.Zhi[i % 12]);
                return tempStr;
            }
        }
        #endregion

        #region GanZhiMonthString
        /// <summary>
        /// 取干支的月表示字符串，注意农历的闰月不记干支
        /// </summary>
        public string GanZhiMonth { get; private set; }

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

            GanZhiMonth = gan + zhi + "月";
        }

        #endregion

        #region GanZhiDayString
        /// <summary>
        /// 取干支日表示法
        /// </summary>
        public string GanZhiDay
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
        public string GanZhiDate
        {
            get
            {
                return GanZhiYear + GanZhiMonth + GanZhiDay;
            }
        }
        #endregion
        #endregion
        #endregion


    }
}
