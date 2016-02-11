using System;

namespace Library.Date
{
    /// <summary>
    /// ��ʮ�Ĺ���
    /// </summary>
    /// <remarks>����ָ��ʮ��ʱ�ں��������й��Ŵ�������һ������ָ��ũ�µĲ���������</remarks>
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
        /// ���Q
        /// </summary>
        public string HolidayName { get; private set; }

        /// <summary>
        /// ���┵��
        /// </summary>
        public int TheSolarTermsData { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name">���Q</param>
        /// <param name="data">���┵��</param>
        /// <param name="flag">ture��r������t����</param>
        private TheSolarTermsHoliday(string name, int data, bool flag) : this()
        {
            HolidayName = name;
            TheSolarTermsData = data;
            IsSeason = flag;
            IsClimate = !flag;
        }

        /// <summary>
        /// ʱ��
        /// </summary>
        public bool IsSeason { get; private set; }

        /// <summary>
        /// ����
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
        /// С�� Slight Cold
        /// </summary>
        public static readonly TheSolarTermsHoliday SlightCold = new TheSolarTermsHoliday("С��", 0, true);

        /// <summary>
        /// �� Great Cold
        /// </summary>
        public static readonly TheSolarTermsHoliday GreatCold = new TheSolarTermsHoliday("��", 21208, false);

        /// <summary>
        /// ���� the Beginning of Spring
        /// </summary>
        public static readonly TheSolarTermsHoliday TheBeginningOfSpring = new TheSolarTermsHoliday("����", 42467, true);

        /// <summary>
        /// ��ˮ Rain Water
        /// </summary>
        public static readonly TheSolarTermsHoliday RainWater = new TheSolarTermsHoliday("��ˮ", 63836, false);

        /// <summary>
        /// ���� the Waking of Insects
        /// </summary>
        public static readonly TheSolarTermsHoliday TheWakingOfInsects = new TheSolarTermsHoliday("����", 85337, true);

        /// <summary>
        /// ���� the Spring Equinox
        /// </summary>
        public static readonly TheSolarTermsHoliday TheSpringEquinox = new TheSolarTermsHoliday("����", 107014, false);

        /// <summary>
        /// ���� Pure Brightness
        /// </summary>
        public static readonly TheSolarTermsHoliday PureBrightness = new TheSolarTermsHoliday("����", 128867, true);

        /// <summary>
        /// ���� Grain Rain
        /// </summary>
        public static readonly TheSolarTermsHoliday GrainRain = new TheSolarTermsHoliday("����", 150921, false);

        /// <summary>
        /// ���� the Beginning of Summer
        /// </summary>
        public static readonly TheSolarTermsHoliday TheBeginningOfSummer = new TheSolarTermsHoliday("����", 173149, true);

        /// <summary>
        /// С�� Grain Full
        /// </summary>
        public static readonly TheSolarTermsHoliday GrainFull = new TheSolarTermsHoliday("С��", 195551, false);

        /// <summary>
        /// â�� Grain in Ear
        /// </summary>
        public static readonly TheSolarTermsHoliday GraininEar = new TheSolarTermsHoliday("â��", 218072, true);

        /// <summary>
        /// ���� the Summer Solstice
        /// </summary>
        public static readonly TheSolarTermsHoliday TheSummerSolstice = new TheSolarTermsHoliday("����", 240693, false);

        /// <summary>
        /// С�� Slight Heat
        /// </summary>
        public static readonly TheSolarTermsHoliday SlightHeat = new TheSolarTermsHoliday("С��", 263343, true);

        /// <summary>
        /// ���� Great Heat
        /// </summary>
        public static readonly TheSolarTermsHoliday GreatHeat = new TheSolarTermsHoliday("����", 285989, false);

        /// <summary>
        /// ���� the Beginning of Autumn
        /// </summary>
        public static readonly TheSolarTermsHoliday TheBeginningOfAutumn = new TheSolarTermsHoliday("����", 308563, true);

        /// <summary>
        /// ���� the Limit of Heat
        /// </summary>
        public static readonly TheSolarTermsHoliday TheLimitOfHeat = new TheSolarTermsHoliday("����", 331033, false);

        /// <summary>
        /// ��¶ White Dew
        /// </summary>
        public static readonly TheSolarTermsHoliday WhiteDew = new TheSolarTermsHoliday("��¶", 353350, true);

        /// <summary>
        /// ��� the Autumnal Equinox
        /// </summary>
        public static readonly TheSolarTermsHoliday TheAutumnalEquinox = new TheSolarTermsHoliday("���", 375494, false);

        /// <summary>
        /// ��¶ Cold Dew
        /// </summary>
        public static readonly TheSolarTermsHoliday ColdDew = new TheSolarTermsHoliday("��¶", 397447, true);

        /// <summary>
        /// ˪�� Frost's Descent
        /// </summary>
        public static readonly TheSolarTermsHoliday FrostsDescent = new TheSolarTermsHoliday("˪��", 419210, false);

        /// <summary>
        /// ���� the Beginning of Winter
        /// </summary>
        public static readonly TheSolarTermsHoliday TheBeginningOfWinter = new TheSolarTermsHoliday("����", 440795, true);

        /// <summary>
        /// Сѩ Slight Snow
        /// </summary>
        public static readonly TheSolarTermsHoliday SlightSnow = new TheSolarTermsHoliday("Сѩ", 462224, false);

        /// <summary>
        /// ��ѩ Great Snow
        /// </summary>
        public static readonly TheSolarTermsHoliday GreatSnow = new TheSolarTermsHoliday("��ѩ", 483532, true);

        /// <summary>
        /// ���� the Winter Solstice
        /// </summary>
        public static readonly TheSolarTermsHoliday TheWinterSolstice = new TheSolarTermsHoliday("����", 504758, false);
    }
}