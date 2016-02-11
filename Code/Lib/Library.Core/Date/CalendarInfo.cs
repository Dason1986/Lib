using System;

namespace Library.Date
{
    /// <summary>
    ///
    /// </summary>
    public static class CalendarInfo
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly string[] ChineseMonths =
        {
            "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "寒月", "腊月"
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
        public static readonly string[] ChineseZodiac = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

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

        #endregion 星座名称

        #region 二十四節氣

        /// <summary>
        /// 二十四節氣
        /// </summary>
        public static readonly TheSolarTermsHoliday[] TheSolarTermsHolidays =
        {    //(节) (气)    (节)    (气)
            //"小寒", "大寒", "立春", "雨水",
            TheSolarTermsHoliday.SlightCold,             TheSolarTermsHoliday.GreatCold,          TheSolarTermsHoliday.TheBeginningOfSpring, TheSolarTermsHoliday.RainWater,
            //"惊蛰", "春分", "清明", "谷雨",
            TheSolarTermsHoliday.TheWakingOfInsects,     TheSolarTermsHoliday.TheSpringEquinox,   TheSolarTermsHoliday.PureBrightness,       TheSolarTermsHoliday.GrainRain,
            //"立夏", "小满", "芒种", "夏至",
             TheSolarTermsHoliday.TheBeginningOfSummer,  TheSolarTermsHoliday.GrainFull,          TheSolarTermsHoliday.GraininEar,           TheSolarTermsHoliday.TheSummerSolstice,
            //"小暑", "大暑", "立秋", "处暑",
            TheSolarTermsHoliday.SlightHeat,             TheSolarTermsHoliday.GreatHeat,          TheSolarTermsHoliday.TheBeginningOfAutumn, TheSolarTermsHoliday.TheLimitOfHeat,
            //"白露", "秋分", "寒露", "霜降",
            TheSolarTermsHoliday.WhiteDew,               TheSolarTermsHoliday.TheAutumnalEquinox, TheSolarTermsHoliday.ColdDew,              TheSolarTermsHoliday.FrostsDescent,
            //"立冬", "小雪", "大雪", "冬至"
            TheSolarTermsHoliday.TheBeginningOfWinter,   TheSolarTermsHoliday.SlightSnow,         TheSolarTermsHoliday.GreatSnow,            TheSolarTermsHoliday.TheWinterSolstice
        };

        #endregion 二十四節氣

        #region 二十八星宿

        /// <summary>
        /// 二十八星宿
        /// </summary>
        public readonly static string[] ChineseConstellationName =
        {
            //四   五    六      日    一      二      三
            "角宿","亢宿","氐宿","房宿","心宿","尾宿","箕宿",
            "斗宿","牛宿","女宿","虚宿","危宿","室宿","壁宿",
            "奎宿","娄宿","胃宿","昴宿","毕宿","觜宿","参宿",
            "井宿","鬼宿","柳宿","星宿","张宿","翼宿","轸宿"
        };

        /// <summary>
        /// 二十八星宿
        /// </summary>
        public readonly static string[] ChineseConstellationAnimalName =
        {
            //四        五      六         日        一      二      三
            "角木蛟","亢金龙","氐土貉","房日兔","心月狐","尾火虎","箕水豹",
            "斗木獬","牛金牛","女土蝠","虚日鼠","危月燕","室火猪","壁水獝",
            "奎木狼","娄金狗","胃土彘","昴日鸡","毕月乌","觜火猴","参水猿",
            "井木犴","鬼金羊","柳土獐","星日马","张月鹿","翼火蛇","轸水蚓"
        };

        #endregion 二十八星宿

        #region 節氣数据

        /// <summary>
        /// 節氣数据
        /// </summary>

        internal static readonly int[] STermInfo = new int[] { 0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };

        #endregion 節氣数据

        #region 按公历计算的節日

        /// <summary>
        /// 按公历计算的節日
        /// </summary>
        public static readonly SolarHoliday[] SolarHolidays = new SolarHoliday[]{
            new SolarHoliday(1, 1, "元旦"),
            new SolarHoliday(2, 2,  "世界湿地日"),
            new SolarHoliday(2, 10,  "国际气象节"),
            new SolarHoliday(2, 14,  "情人节"),
            new SolarHoliday(3, 1,  "国际海豹日"),
            new SolarHoliday(3, 8,  "妇女节"),
            new SolarHoliday(3, 12,  "植树节"),
            new SolarHoliday(3, 14,  "国际警察日"),
            new SolarHoliday(3, 15,  "消费者权益日"),
            new SolarHoliday(3, 17,  "中国国医节"),
            new SolarHoliday(3, 17,  "国际航海日"),
            new SolarHoliday(3, 21,  "世界森林日"),
            new SolarHoliday(3, 21,  "消除种族歧视国际日"),
            new SolarHoliday(3, 21,  "世界儿歌日"),
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
            new SolarHoliday(7, 1,  "建党节"),
            new SolarHoliday(7, 1,  "香港回归纪念"),
            new SolarHoliday(7, 1,  " 世界建筑日"),
            new SolarHoliday(7, 11,  "世界人口日"),
            new SolarHoliday(8, 1,  "建军节"),
            //  new SolarHoliday(8, 8,  "中国男子节 父亲节"),
            new SolarHoliday(8, 15,  "抗日战争胜利纪念"),

            new SolarHoliday(9, 10,  "教师节"),
            //   new SolarHoliday(9, 18,  "九·一八事变纪念日"),
            new SolarHoliday(9, 20,  "国际爱牙日"),
            new SolarHoliday(9, 27,  "世界旅游日"),
            //    new SolarHoliday(9, 28,  "孔子诞辰"),
            new SolarHoliday(10, 1,  "国庆节"),
            //    new SolarHoliday(10, 6,  "老人节"),
            new SolarHoliday(10, 24,  "联合国日"),
            new SolarHoliday(11, 10,  "世界青年节"),
            //    new SolarHoliday(11, 12,  "孙中山诞辰纪念"),
            new SolarHoliday(12, 1,  "世界艾滋病日"),
            new SolarHoliday(12, 3,  "世界残疾人日"),
            new SolarHoliday(12, 20,  "澳门回归纪念"),
            new SolarHoliday(12, 24,  "平安夜"),
            new SolarHoliday(12, 25,  "圣诞节")
        };

        #endregion 按公历计算的節日

        #region 按農曆计算的節日

        /// <summary>
        /// 按農曆计算的節日
        /// </summary>
        public static readonly LunarHoliday[] LunarHolidays =
        {
            new LunarHoliday(1, 1,  "春节"),
            new LunarHoliday(1, 15,  "元宵节"),
            new LunarHoliday(5, 5,  "端午节"),
            new LunarHoliday(7, 7,  "七夕情人节"),
            new LunarHoliday(7, 15,  "中元节"),
            new LunarHoliday(8, 15,  "中秋节"),
            new LunarHoliday(9, 9, "重阳节"),
            new LunarHoliday(12, 8,  "腊八节"),
            new LunarHoliday(12, 30,  "除夕")  //注意除夕需要其它方法进行计算
        };

        #endregion 按農曆计算的節日

        #region 按某月第几个星期几

        /// <summary>
        /// 按某月第几个星期几
        /// </summary>
        public static readonly WeekHoliday[] WeekHolidays = new WeekHoliday[]{
            new WeekHoliday(5, 2,  DayOfWeek.Sunday, "母亲节"),
            new WeekHoliday(5, 3,  DayOfWeek.Sunday, "全国助残日"),
            new WeekHoliday(6, 3,  DayOfWeek.Sunday, "父亲节"),
            new WeekHoliday(3, 0,  DayOfWeek.Saturday, "地球一小时"),
            new WeekHoliday(9, 4,  DayOfWeek.Thursday, "国际聋人节"),
            new WeekHoliday(10, 1,  DayOfWeek.Monday, "国际住房日"),
            new WeekHoliday(10, 2, DayOfWeek.Wednesday, "国际减轻自然灾害日"),
            new WeekHoliday(11, 0, DayOfWeek.Thursday, "感恩节")
        };

        #endregion 按某月第几个星期几
    }
}