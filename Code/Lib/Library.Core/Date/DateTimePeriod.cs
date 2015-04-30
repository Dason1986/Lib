namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public enum DateTimePeriod
    {
        /// <summary>
        /// ���t����
        /// </summary>
        After = 8,
        /// <summary>
        /// �@һ��
        /// </summary>
        ThisYear=7,
        /// <summary>
        /// �@����
        /// </summary>
        ThisMonth=6,
        /// <summary>
        /// ������
        /// </summary>
        NextMonth=5,
        /// <summary>
        /// ������
        /// </summary>
        NextWeek=4,
        /// <summary>
        /// �@������
        /// </summary>
        ThisWeek=3,
        /// <summary>
        /// ����
        /// </summary>
        TheDayAfterTomorrow=2,
        /// <summary>
        /// ����
        /// </summary>
        Tomorrow=1,
        /// <summary>
        /// ����
        /// </summary>
        Today=0,
        /// <summary>
        /// ����
        /// </summary>
        Yesterday=-1,
        /// <summary>
        /// ǰ��
        /// </summary>
        TheDayBeforeYesterday=-2,
        /// <summary>
        /// �ς�����
        /// </summary>
        LastWeek=-3,
        /// <summary>
        /// �ς���
        /// </summary>
        LastMonth=-4,
        /// <summary>
        /// ����֮ǰ
        /// </summary>
        Earlier = -5,
    }
}