namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public enum DateTimePeriod
    {
        /// <summary>
        /// 更遲以後
        /// </summary>
        After = 8,
        /// <summary>
        /// 這一年
        /// </summary>
        ThisYear=7,
        /// <summary>
        /// 這個月
        /// </summary>
        ThisMonth=6,
        /// <summary>
        /// 下星期
        /// </summary>
        NextMonth=5,
        /// <summary>
        /// 下星期
        /// </summary>
        NextWeek=4,
        /// <summary>
        /// 這個星期
        /// </summary>
        ThisWeek=3,
        /// <summary>
        /// 後天
        /// </summary>
        TheDayAfterTomorrow=2,
        /// <summary>
        /// 明天
        /// </summary>
        Tomorrow=1,
        /// <summary>
        /// 今天
        /// </summary>
        Today=0,
        /// <summary>
        /// 昨天
        /// </summary>
        Yesterday=-1,
        /// <summary>
        /// 前天
        /// </summary>
        TheDayBeforeYesterday=-2,
        /// <summary>
        /// 上個星期
        /// </summary>
        LastWeek=-3,
        /// <summary>
        /// 上個月
        /// </summary>
        LastMonth=-4,
        /// <summary>
        /// 更早之前
        /// </summary>
        Earlier = -5,
    }
}