using System;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHoliday : IFormattable
    {
        /// <summary>
        /// ��
        /// </summary>
        int Month { get; }
        /// <summary>
        /// ��
        /// </summary>
        int Day { get; }

        /// <summary>
        /// �������Q
        /// </summary>
        string HolidayName { get; }

        /// <summary>
        /// �D�Q�ɹ�������
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DateTime ConvertDateTime(int year);
    }
}