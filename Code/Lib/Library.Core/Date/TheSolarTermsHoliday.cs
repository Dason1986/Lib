using System;

namespace Library.Date
{
    /// <summary>
    /// 二十四節氣
    /// </summary>
    /// <remarks>节气指二十四时节和气候，是中国古代订立的一种用来指导农事的补充历法。</remarks>
    public struct TheSolarTermsHoliday : IHoliday
    {
        int IHoliday.Month
        {
            get { throw new NotImplementedException(); }
        }

        int IHoliday.Day
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 名稱
        /// </summary>
        public string HolidayName { get; private set; }

        /// <summary>
        /// 節氣數據
        /// </summary>
        public int TheSolarTermsData { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="data">節氣數據</param>
        /// <param name="flag">ture為時節，否則為氣候</param>
        private TheSolarTermsHoliday(string name, int data, bool flag) : this()
        {
            HolidayName = name;
            TheSolarTermsData = data;
            IsSeason = flag;
            IsClimate = !flag;
        }

        /// <summary>
        /// 时节
        /// </summary>
        public bool IsSeason { get; private set; }

        /// <summary>
        /// 气候
        /// </summary>
        public bool IsClimate { get; private set; }

        private static readonly DateTime BaseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#

        /// <summary>
        ///
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime ConvertDateTime(int year)
        {
            double y = 525948.76 * (year - 1900);
            double num = y + TheSolarTermsData;
            DateTime newDate = BaseDateAndTime.AddMinutes(num);
            return newDate;
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return HolidayFormat.FormatProvider.Format(format, this, formatProvider);
        }

        /// <summary>
        /// 小寒 Slight Cold
        /// </summary>
        public static readonly TheSolarTermsHoliday SlightCold = new TheSolarTermsHoliday("小寒", 0, true);

        /// <summary>
        /// 大寒 Great Cold
        /// </summary>
        public static readonly TheSolarTermsHoliday GreatCold = new TheSolarTermsHoliday("大寒", 21208, false);

        /// <summary>
        /// 立春 the Beginning of Spring
        /// </summary>
        public static readonly TheSolarTermsHoliday TheBeginningOfSpring = new TheSolarTermsHoliday("立春", 42467, true);

        /// <summary>
        /// 雨水 Rain Water
        /// </summary>
        public static readonly TheSolarTermsHoliday RainWater = new TheSolarTermsHoliday("雨水", 63836, false);

        /// <summary>
        /// 惊蛰 the Waking of Insects
        /// </summary>
        public static readonly TheSolarTermsHoliday TheWakingOfInsects = new TheSolarTermsHoliday("惊蛰", 85337, true);

        /// <summary>
        /// 春分 the Spring Equinox
        /// </summary>
        public static readonly TheSolarTermsHoliday TheSpringEquinox = new TheSolarTermsHoliday("春分", 107014, false);

        /// <summary>
        /// 清明 Pure Brightness
        /// </summary>
        public static readonly TheSolarTermsHoliday PureBrightness = new TheSolarTermsHoliday("清明", 128867, true);

        /// <summary>
        /// 谷雨 Grain Rain
        /// </summary>
        public static readonly TheSolarTermsHoliday GrainRain = new TheSolarTermsHoliday("谷雨", 150921, false);

        /// <summary>
        /// 立夏 the Beginning of Summer
        /// </summary>
        public static readonly TheSolarTermsHoliday TheBeginningOfSummer = new TheSolarTermsHoliday("立夏", 173149, true);

        /// <summary>
        /// 小满 Grain Full
        /// </summary>
        public static readonly TheSolarTermsHoliday GrainFull = new TheSolarTermsHoliday("小满", 195551, false);

        /// <summary>
        /// 芒种 Grain in Ear
        /// </summary>
        public static readonly TheSolarTermsHoliday GraininEar = new TheSolarTermsHoliday("芒种", 218072, true);

        /// <summary>
        /// 夏至 the Summer Solstice
        /// </summary>
        public static readonly TheSolarTermsHoliday TheSummerSolstice = new TheSolarTermsHoliday("夏至", 240693, false);

        /// <summary>
        /// 小暑 Slight Heat
        /// </summary>
        public static readonly TheSolarTermsHoliday SlightHeat = new TheSolarTermsHoliday("小暑", 263343, true);

        /// <summary>
        /// 大暑 Great Heat
        /// </summary>
        public static readonly TheSolarTermsHoliday GreatHeat = new TheSolarTermsHoliday("大暑", 285989, false);

        /// <summary>
        /// 立秋 the Beginning of Autumn
        /// </summary>
        public static readonly TheSolarTermsHoliday TheBeginningOfAutumn = new TheSolarTermsHoliday("立秋", 308563, true);

        /// <summary>
        /// 处暑 the Limit of Heat
        /// </summary>
        public static readonly TheSolarTermsHoliday TheLimitOfHeat = new TheSolarTermsHoliday("处暑", 331033, false);

        /// <summary>
        /// 白露 White Dew
        /// </summary>
        public static readonly TheSolarTermsHoliday WhiteDew = new TheSolarTermsHoliday("白露", 353350, true);

        /// <summary>
        /// 秋分 the Autumnal Equinox
        /// </summary>
        public static readonly TheSolarTermsHoliday TheAutumnalEquinox = new TheSolarTermsHoliday("秋分", 375494, false);

        /// <summary>
        /// 寒露 Cold Dew
        /// </summary>
        public static readonly TheSolarTermsHoliday ColdDew = new TheSolarTermsHoliday("寒露", 397447, true);

        /// <summary>
        /// 霜降 Frost's Descent
        /// </summary>
        public static readonly TheSolarTermsHoliday FrostsDescent = new TheSolarTermsHoliday("霜降", 419210, false);

        /// <summary>
        /// 立冬 the Beginning of Winter
        /// </summary>
        public static readonly TheSolarTermsHoliday TheBeginningOfWinter = new TheSolarTermsHoliday("立冬", 440795, true);

        /// <summary>
        /// 小雪 Slight Snow
        /// </summary>
        public static readonly TheSolarTermsHoliday SlightSnow = new TheSolarTermsHoliday("小雪", 462224, false);

        /// <summary>
        /// 大雪 Great Snow
        /// </summary>
        public static readonly TheSolarTermsHoliday GreatSnow = new TheSolarTermsHoliday("大雪", 483532, true);

        /// <summary>
        /// 冬至 the Winter Solstice
        /// </summary>
        public static readonly TheSolarTermsHoliday TheWinterSolstice = new TheSolarTermsHoliday("冬至", 504758, false);
    }
}