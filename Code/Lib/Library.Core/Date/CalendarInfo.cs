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
            "����", "����", "����", "����", "����", "����", "����", "����", "����", "ʮ��", "����", "����"
        };

        /// <summary>
        /// ���
        /// </summary>
        public static readonly string[] Gan = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };

        /// <summary>
        /// ��֧
        /// </summary>
        public static readonly string[] Zhi = { "��", "��", "��", "î", "��", "��", "��", "δ", "��", "��", "��", "��" };

        /// <summary>
        /// ��Ф
        /// </summary>
        public static readonly string[] ChineseZodiac = { "��", "ţ", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };

        #region ��������

        /// <summary>
        /// ��������
        /// </summary>
        public static readonly string[] ConstellationName = {
            "������", "��ţ��", "˫����",
            "��з��", "ʨ����", "��Ů��",
            "�����", "��Ы��", "������",
            "Ħ����", "ˮƿ��", "˫����"
        };

        #endregion ��������

        #region ��ʮ�Ĺ���

        /// <summary>
        /// ��ʮ�Ĺ���
        /// </summary>
        public static readonly TheSolarTermsHoliday[] TheSolarTermsHolidays =
        {    //(��) (��)    (��)    (��)
            //"С��", "��", "����", "��ˮ",
            TheSolarTermsHoliday.SlightCold,             TheSolarTermsHoliday.GreatCold,          TheSolarTermsHoliday.TheBeginningOfSpring, TheSolarTermsHoliday.RainWater,
            //"����", "����", "����", "����",
            TheSolarTermsHoliday.TheWakingOfInsects,     TheSolarTermsHoliday.TheSpringEquinox,   TheSolarTermsHoliday.PureBrightness,       TheSolarTermsHoliday.GrainRain,
            //"����", "С��", "â��", "����",
             TheSolarTermsHoliday.TheBeginningOfSummer,  TheSolarTermsHoliday.GrainFull,          TheSolarTermsHoliday.GraininEar,           TheSolarTermsHoliday.TheSummerSolstice,
            //"С��", "����", "����", "����",
            TheSolarTermsHoliday.SlightHeat,             TheSolarTermsHoliday.GreatHeat,          TheSolarTermsHoliday.TheBeginningOfAutumn, TheSolarTermsHoliday.TheLimitOfHeat,
            //"��¶", "���", "��¶", "˪��",
            TheSolarTermsHoliday.WhiteDew,               TheSolarTermsHoliday.TheAutumnalEquinox, TheSolarTermsHoliday.ColdDew,              TheSolarTermsHoliday.FrostsDescent,
            //"����", "Сѩ", "��ѩ", "����"
            TheSolarTermsHoliday.TheBeginningOfWinter,   TheSolarTermsHoliday.SlightSnow,         TheSolarTermsHoliday.GreatSnow,            TheSolarTermsHoliday.TheWinterSolstice
        };

        #endregion ��ʮ�Ĺ���

        #region ��ʮ������

        /// <summary>
        /// ��ʮ������
        /// </summary>
        public readonly static string[] ChineseConstellationName =
        {
            //��   ��    ��      ��    һ      ��      ��
            "����","����","ص��","����","����","β��","����",
            "����","ţ��","Ů��","����","Σ��","����","����",
            "����","¦��","θ��","����","����","����","����",
            "����","����","����","����","����","����","����"
        };

        /// <summary>
        /// ��ʮ������
        /// </summary>
        public readonly static string[] ChineseConstellationAnimalName =
        {
            //��        ��      ��         ��        һ      ��      ��
            "��ľ��","������","ص����","������","���º�","β��","��ˮ��",
            "��ľ�","ţ��ţ","Ů����","������","Σ����","�һ���","��ˮ��",
            "��ľ��","¦��","θ����","���ռ�","������","�����","��ˮԳ",
            "��ľ��","�����","�����","������","����¹","�����","��ˮ�"
        };

        #endregion ��ʮ������

        #region ��������

        /// <summary>
        /// ��������
        /// </summary>

        internal static readonly int[] STermInfo = new int[] { 0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };

        #endregion ��������

        #region ����������Ĺ���

        /// <summary>
        /// ����������Ĺ���
        /// </summary>
        public static readonly SolarHoliday[] SolarHolidays = new SolarHoliday[]{
            new SolarHoliday(1, 1, "Ԫ��"),
            new SolarHoliday(2, 2,  "����ʪ����"),
            new SolarHoliday(2, 10,  "���������"),
            new SolarHoliday(2, 14,  "���˽�"),
            new SolarHoliday(3, 1,  "���ʺ�����"),
            new SolarHoliday(3, 8,  "��Ů��"),
            new SolarHoliday(3, 12,  "ֲ����"),
            new SolarHoliday(3, 14,  "���ʾ�����"),
            new SolarHoliday(3, 15,  "������Ȩ����"),
            new SolarHoliday(3, 17,  "�й���ҽ��"),
            new SolarHoliday(3, 17,  "���ʺ�����"),
            new SolarHoliday(3, 21,  "����ɭ����"),
            new SolarHoliday(3, 21,  "�����������ӹ�����"),
            new SolarHoliday(3, 21,  "���������"),
            new SolarHoliday(3, 22,  "����ˮ��"),
            new SolarHoliday(3, 24,  "������ν�˲���"),
            new SolarHoliday(4, 1,  "���˽�"),
            new SolarHoliday(4, 7,  "����������"),
            new SolarHoliday(4, 22,  "���������"),
            new SolarHoliday(5, 1,  "�Ͷ���"),
            new SolarHoliday(5, 4,  "�����"),
            new SolarHoliday(5, 8,  "�����ʮ����"),
            new SolarHoliday(5, 12,  "���ʻ�ʿ��"),
            new SolarHoliday(5, 31,  "����������"),
            new SolarHoliday(6, 1,  "���ʶ�ͯ��"),
            new SolarHoliday(6, 5,  "���绷��������"),
            new SolarHoliday(6, 26,  "���ʽ�����"),
            new SolarHoliday(7, 1,  "������"),
            new SolarHoliday(7, 1,  "��ۻع����"),
            new SolarHoliday(7, 1,  " ���罨����"),
            new SolarHoliday(7, 11,  "�����˿���"),
            new SolarHoliday(8, 1,  "������"),
            //  new SolarHoliday(8, 8,  "�й����ӽ� ���׽�"),
            new SolarHoliday(8, 15,  "����ս��ʤ������"),

            new SolarHoliday(9, 10,  "��ʦ��"),
            //   new SolarHoliday(9, 18,  "�š�һ���±������"),
            new SolarHoliday(9, 20,  "���ʰ�����"),
            new SolarHoliday(9, 27,  "����������"),
            //    new SolarHoliday(9, 28,  "���ӵ���"),
            new SolarHoliday(10, 1,  "�����"),
            //    new SolarHoliday(10, 6,  "���˽�"),
            new SolarHoliday(10, 24,  "���Ϲ���"),
            new SolarHoliday(11, 10,  "���������"),
            //    new SolarHoliday(11, 12,  "����ɽ��������"),
            new SolarHoliday(12, 1,  "���簬�̲���"),
            new SolarHoliday(12, 3,  "����м�����"),
            new SolarHoliday(12, 20,  "���Żع����"),
            new SolarHoliday(12, 24,  "ƽ��ҹ"),
            new SolarHoliday(12, 25,  "ʥ����")
        };

        #endregion ����������Ĺ���

        #region ���r�Ѽ���Ĺ���

        /// <summary>
        /// ���r�Ѽ���Ĺ���
        /// </summary>
        public static readonly LunarHoliday[] LunarHolidays =
        {
            new LunarHoliday(1, 1,  "����"),
            new LunarHoliday(1, 15,  "Ԫ����"),
            new LunarHoliday(5, 5,  "�����"),
            new LunarHoliday(7, 7,  "��Ϧ���˽�"),
            new LunarHoliday(7, 15,  "��Ԫ��"),
            new LunarHoliday(8, 15,  "�����"),
            new LunarHoliday(9, 9, "������"),
            new LunarHoliday(12, 8,  "���˽�"),
            new LunarHoliday(12, 30,  "��Ϧ")  //ע���Ϧ��Ҫ�����������м���
        };

        #endregion ���r�Ѽ���Ĺ���

        #region ��ĳ�µڼ������ڼ�

        /// <summary>
        /// ��ĳ�µڼ������ڼ�
        /// </summary>
        public static readonly WeekHoliday[] WeekHolidays = new WeekHoliday[]{
            new WeekHoliday(5, 2,  DayOfWeek.Sunday, "ĸ�׽�"),
            new WeekHoliday(5, 3,  DayOfWeek.Sunday, "ȫ��������"),
            new WeekHoliday(6, 3,  DayOfWeek.Sunday, "���׽�"),
            new WeekHoliday(3, 0,  DayOfWeek.Saturday, "����һСʱ"),
            new WeekHoliday(9, 4,  DayOfWeek.Thursday, "�������˽�"),
            new WeekHoliday(10, 1,  DayOfWeek.Monday, "����ס����"),
            new WeekHoliday(10, 2, DayOfWeek.Wednesday, "���ʼ�����Ȼ�ֺ���"),
            new WeekHoliday(11, 0, DayOfWeek.Thursday, "�ж���")
        };

        #endregion ��ĳ�µڼ������ڼ�
    }
}