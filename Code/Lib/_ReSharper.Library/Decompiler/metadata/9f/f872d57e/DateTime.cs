// Type: System.DateTime
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 3E47475E-95FE-42E6-A243-CF5A610648E9
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
  /// <summary>
  /// 表示時間的瞬間，通常以一天的日期和時間表示。
  /// </summary>
  /// <filterpriority>1</filterpriority>
  [Serializable]
  [StructLayout(LayoutKind.Auto)]
  public struct DateTime : IComparable, IFormattable, IConvertible, ISerializable, IComparable<DateTime>, IEquatable<DateTime>
  {
    /// <summary>
    /// 表示 <see cref="T:System.DateTime"/> 的最小可能值。 這個欄位是唯讀的。
    /// </summary>
    /// <filterpriority>1</filterpriority>
    public static readonly DateTime MinValue;
    /// <summary>
    /// 表示 <see cref="T:System.DateTime"/> 的最大可能值。 這個欄位是唯讀的。
    /// </summary>
    /// <filterpriority>1</filterpriority>
    public static readonly DateTime MaxValue;
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 類別的新執行個體初始化為刻度的指定數目。
    /// </summary>
    /// <param name="ticks">以西曆 0001 年 1 月 1 日 00:00:00.000 以來已經過的 100 奈秒間隔數表示的日期和時間。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="ticks"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception>
    public DateTime(long ticks);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定的刻度數以及 Coordinated Universal Time (UTC) 或本地時間。
    /// </summary>
    /// <param name="ticks">以西曆 0001 年 1 月 1 日 00:00:00.000 以來已經過的 100 奈秒間隔數表示的日期和時間。</param><param name="kind">其中一個列舉值，指出 <paramref name="ticks"/> 是指定本地時間或 Coordinated Universal Time (UTC)，還是兩者都不指定。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="ticks"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><exception cref="T:System.ArgumentException"><paramref name="kind"/> 不是其中一個 <see cref="T:System.DateTimeKind"/> 值。</exception>
    public DateTime(long ticks, DateTimeKind kind);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定的年、月和日。
    /// </summary>
    /// <param name="year">年份 (1 到 9999)。</param><param name="month">月份 (1 到 12)。</param><param name="day">日期 (1 到 <paramref name="month"/> 中的天數)。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 小於 1 或大於 9999。 -或- <paramref name="month"/> 小於 1 或大於 12。 -或- <paramref name="day"/> 小於 1 或大於 <paramref name="month"/> 中的天數。</exception>
    public DateTime(int year, int month, int day);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定之月曆的指定之年、月和日。
    /// </summary>
    /// <param name="year">年 (1 到 <paramref name="calendar"/> 中的年份)。</param><param name="month">月份 (1 到 <paramref name="calendar"/> 中的月份數目)。</param><param name="day">日期 (1 到 <paramref name="month"/> 中的天數)。</param><param name="calendar">用來解譯 <paramref name="year"/>、<paramref name="month"/> 和 <paramref name="day"/> 的行事曆。</param><exception cref="T:System.ArgumentNullException"><paramref name="calendar"/> 為 null。</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 不在 <paramref name="calendar"/> 所支援的範圍中。 -或- <paramref name="month"/> 小於 1 或大於 <paramref name="calendar"/> 的月份數目。 -或- <paramref name="day"/> 小於 1 或大於 <paramref name="month"/> 中的天數。</exception>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DateTime(int year, int month, int day, Calendar calendar);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定的年、月、日、時、分和秒。
    /// </summary>
    /// <param name="year">年份 (1 到 9999)。</param><param name="month">月份 (1 到 12)。</param><param name="day">日期 (1 到 <paramref name="month"/> 中的天數)。</param><param name="hour">小時 (0 到 23)。</param><param name="minute">分鐘 (0 到 59)。</param><param name="second">秒數 (0 到 59)。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 小於 1 或大於 9999。 -或- <paramref name="month"/> 小於 1 或大於 12。 -或- <paramref name="day"/> 小於 1 或大於 <paramref name="month"/> 中的天數。 -或- <paramref name="hour"/> 小於 0 或大於 23。 -或- <paramref name="minute"/> 小於 0 或大於 59。 -或- <paramref name="second"/> 小於 0 或大於 59。</exception>
    public DateTime(int year, int month, int day, int hour, int minute, int second);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定之年、月、日、時、分、秒以及 Coordinated Universal Time (UTC) 或本地時間。
    /// </summary>
    /// <param name="year">年份 (1 到 9999)。</param><param name="month">月份 (1 到 12)。</param><param name="day">日期 (1 到 <paramref name="month"/> 中的天數)。</param><param name="hour">小時 (0 到 23)。</param><param name="minute">分鐘 (0 到 59)。</param><param name="second">秒數 (0 到 59)。</param><param name="kind">其中一個列舉值，指出 <paramref name="year"/>、<paramref name="month"/>、<paramref name="day"/>、<paramref name="hour"/>、<paramref name="minute"/> 和 <paramref name="second"/> 是指定本地時間或 Coordinated Universal Time (UTC)，還是兩者都不指定。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 小於 1 或大於 9999。 -或- <paramref name="month"/> 小於 1 或大於 12。 -或- <paramref name="day"/> 小於 1 或大於 <paramref name="month"/> 中的天數。 -或- <paramref name="hour"/> 小於 0 或大於 23。 -或- <paramref name="minute"/> 小於 0 或大於 59。 -或- <paramref name="second"/> 小於 0 或大於 59。</exception><exception cref="T:System.ArgumentException"><paramref name="kind"/> 不是其中一個 <see cref="T:System.DateTimeKind"/> 值。</exception>
    public DateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定之月曆的指定之年、月、日、時、分和秒。
    /// </summary>
    /// <param name="year">年 (1 到 <paramref name="calendar"/> 中的年份)。</param><param name="month">月份 (1 到 <paramref name="calendar"/> 中的月份數目)。</param><param name="day">日期 (1 到 <paramref name="month"/> 中的天數)。</param><param name="hour">小時 (0 到 23)。</param><param name="minute">分鐘 (0 到 59)。</param><param name="second">秒數 (0 到 59)。</param><param name="calendar">用來解譯 <paramref name="year"/>、<paramref name="month"/> 和 <paramref name="day"/> 的行事曆。</param><exception cref="T:System.ArgumentNullException"><paramref name="calendar"/> 為 null。</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 不在 <paramref name="calendar"/> 所支援的範圍中。 -或- <paramref name="month"/> 小於 1 或大於 <paramref name="calendar"/> 的月份數目。 -或- <paramref name="day"/> 小於 1 或大於 <paramref name="month"/> 中的天數。 -或- <paramref name="hour"/> 小於 0 或大於 23 -或- <paramref name="minute"/> 小於 0 或大於 59。 -或- <paramref name="second"/> 小於 0 或大於 59。</exception>
    public DateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定的年、月、日、時、分、秒和毫秒 (Millisecond)。
    /// </summary>
    /// <param name="year">年份 (1 到 9999)。</param><param name="month">月份 (1 到 12)。</param><param name="day">日期 (1 到 <paramref name="month"/> 中的天數)。</param><param name="hour">小時 (0 到 23)。</param><param name="minute">分鐘 (0 到 59)。</param><param name="second">秒數 (0 到 59)。</param><param name="millisecond">毫秒 (0 到 999)。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 小於 1 或大於 9999。 -或- <paramref name="month"/> 小於 1 或大於 12。 -或- <paramref name="day"/> 小於 1 或大於 <paramref name="month"/> 中的天數。 -或- <paramref name="hour"/> 小於 0 或大於 23。 -或- <paramref name="minute"/> 小於 0 或大於 59。 -或- <paramref name="second"/> 小於 0 或大於 59。 -或- <paramref name="millisecond"/> 小於 0 或大於 999。</exception>
    public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定之年、月、日、時、分、秒、毫秒以及 Coordinated Universal Time (UTC) 或本地時間。
    /// </summary>
    /// <param name="year">年份 (1 到 9999)。</param><param name="month">月份 (1 到 12)。</param><param name="day">日期 (1 到 <paramref name="month"/> 中的天數)。</param><param name="hour">小時 (0 到 23)。</param><param name="minute">分鐘 (0 到 59)。</param><param name="second">秒數 (0 到 59)。</param><param name="millisecond">毫秒 (0 到 999)。</param><param name="kind">其中一個列舉值，指出 <paramref name="year"/>、<paramref name="month"/>、<paramref name="day"/>、<paramref name="hour"/>、<paramref name="minute"/>、<paramref name="second"/> 和 <paramref name="millisecond"/> 是指定本地時間或 Coordinated Universal Time (UTC)，還是兩者都不指定。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 小於 1 或大於 9999。 -或- <paramref name="month"/> 小於 1 或大於 12。 -或- <paramref name="day"/> 小於 1 或大於 <paramref name="month"/> 中的天數。 -或- <paramref name="hour"/> 小於 0 或大於 23。 -或- <paramref name="minute"/> 小於 0 或大於 59。 -或- <paramref name="second"/> 小於 0 或大於 59。 -或- <paramref name="millisecond"/> 小於 0 或大於 999。</exception><exception cref="T:System.ArgumentException"><paramref name="kind"/> 不是其中一個 <see cref="T:System.DateTimeKind"/> 值。</exception>
    public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定之月曆的指定之年、月、日、時、分、秒和毫秒。
    /// </summary>
    /// <param name="year">年 (1 到 <paramref name="calendar"/> 中的年份)。</param><param name="month">月份 (1 到 <paramref name="calendar"/> 中的月份數目)。</param><param name="day">日期 (1 到 <paramref name="month"/> 中的天數)。</param><param name="hour">小時 (0 到 23)。</param><param name="minute">分鐘 (0 到 59)。</param><param name="second">秒數 (0 到 59)。</param><param name="millisecond">毫秒 (0 到 999)。</param><param name="calendar">用來解譯 <paramref name="year"/>、<paramref name="month"/> 和 <paramref name="day"/> 的行事曆。</param><exception cref="T:System.ArgumentNullException"><paramref name="calendar"/> 為 null。</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 不在 <paramref name="calendar"/> 所支援的範圍中。 -或- <paramref name="month"/> 小於 1 或大於 <paramref name="calendar"/> 的月份數目。 -或- <paramref name="day"/> 小於 1 或大於 <paramref name="month"/> 中的天數。 -或- <paramref name="hour"/> 小於 0 或大於 23。 -或- <paramref name="minute"/> 小於 0 或大於 59。 -或- <paramref name="second"/> 小於 0 或大於 59。 -或- <paramref name="millisecond"/> 小於 0 或大於 999。</exception>
    public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar);
    /// <summary>
    /// 將 <see cref="T:System.DateTime"/> 結構的新執行個體初始化為指定之日曆的指定之年、月、日、時、分、秒、毫秒以及 Coordinated Universal Time (UTC) 或本地時間。
    /// </summary>
    /// <param name="year">年 (1 到 <paramref name="calendar"/> 中的年份)。</param><param name="month">月份 (1 到 <paramref name="calendar"/> 中的月份數目)。</param><param name="day">日期 (1 到 <paramref name="month"/> 中的天數)。</param><param name="hour">小時 (0 到 23)。</param><param name="minute">分鐘 (0 到 59)。</param><param name="second">秒數 (0 到 59)。</param><param name="millisecond">毫秒 (0 到 999)。</param><param name="calendar">用來解譯 <paramref name="year"/>、<paramref name="month"/> 和 <paramref name="day"/> 的行事曆。</param><param name="kind">其中一個列舉值，指出 <paramref name="year"/>、<paramref name="month"/>、<paramref name="day"/>、<paramref name="hour"/>、<paramref name="minute"/>、<paramref name="second"/> 和 <paramref name="millisecond"/> 是指定本地時間或 Coordinated Universal Time (UTC)，還是兩者都不指定。</param><exception cref="T:System.ArgumentNullException"><paramref name="calendar"/> 為 null。</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 不在 <paramref name="calendar"/> 所支援的範圍中。 -或- <paramref name="month"/> 小於 1 或大於 <paramref name="calendar"/> 的月份數目。 -或- <paramref name="day"/> 小於 1 或大於 <paramref name="month"/> 中的天數。 -或- <paramref name="hour"/> 小於 0 或大於 23。 -或- <paramref name="minute"/> 小於 0 或大於 59。 -或- <paramref name="second"/> 小於 0 或大於 59。 -或- <paramref name="millisecond"/> 小於 0 或大於 999。</exception><exception cref="T:System.ArgumentException"><paramref name="kind"/> 不是其中一個 <see cref="T:System.DateTimeKind"/> 值。</exception>
    public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind);
    /// <summary>
    /// 將指定的時間間隔加上指定的日期和時間，產生新的日期和時間。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，這個物件是 <paramref name="d"/> 和 <paramref name="t"/> 之值的總和。
    /// </returns>
    /// <param name="d">要加上的日期和時間值。</param><param name="t">要加入的時間間隔。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>3</filterpriority>
    public static DateTime operator +(DateTime d, TimeSpan t);
    /// <summary>
    /// 將指定的日期和時間減去指定的時間間隔，並傳回新的日期和時間。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為 <paramref name="d"/> 值減掉 <paramref name="t"/> 值的差異值。
    /// </returns>
    /// <param name="d">位於減號左邊的日期和時間值。</param><param name="t">要減去的時間間隔。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>3</filterpriority>
    public static DateTime operator -(DateTime d, TimeSpan t);
    /// <summary>
    /// 將另一個指定的日期和時間減去指定的日期和時間，並傳回時間間隔。
    /// </summary>
    /// 
    /// <returns>
    /// <paramref name="d1"/> 和 <paramref name="d2"/> 之間的時間間隔，也就是 <paramref name="d1"/> 減 <paramref name="d2"/>。
    /// </returns>
    /// <param name="d1">位於減號左邊的日期和時間值 (被減數)。</param><param name="d2">位於減號右邊的日期和時間值 (減數)。</param><filterpriority>3</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static TimeSpan operator -(DateTime d1, DateTime d2);
    /// <summary>
    /// 判斷 <see cref="T:System.DateTime"/> 的兩個指定之執行個體是否相等。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="d1"/> 和 <paramref name="d2"/> 表示相同的日期和時間，則為 true，否則為 false。
    /// </returns>
    /// <param name="d1">要比較的第一個物件。</param><param name="d2">要比較的第二個物件。</param><filterpriority>3</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool operator ==(DateTime d1, DateTime d2);
    /// <summary>
    /// 判斷 <see cref="T:System.DateTime"/> 的兩個指定之執行個體是否不相等。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="d1"/> 和 <paramref name="d2"/> 不表示相同的日期和時間，則為 true，否則為 false。
    /// </returns>
    /// <param name="d1">要比較的第一個物件。</param><param name="d2">要比較的第二個物件。</param><filterpriority>3</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool operator !=(DateTime d1, DateTime d2);
    /// <summary>
    /// 判斷某個指定的 <see cref="T:System.DateTime"/> 是否小於另一個指定的 <see cref="T:System.DateTime"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="t1"/> 小於 <paramref name="t2"/>，則為 true，否則為 false。
    /// </returns>
    /// <param name="t1">要比較的第一個物件。</param><param name="t2">要比較的第二個物件。</param><filterpriority>3</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool operator <(DateTime t1, DateTime t2);
    /// <summary>
    /// 判斷某個指定的 <see cref="T:System.DateTime"/> 是否小於或等於另一個指定的 <see cref="T:System.DateTime"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="t1"/> 小於或等於 <paramref name="t2"/> 則為 true，否則為 false。
    /// </returns>
    /// <param name="t1">要比較的第一個物件。</param><param name="t2">要比較的第二個物件。</param><filterpriority>3</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool operator <=(DateTime t1, DateTime t2);
    /// <summary>
    /// 判斷某個指定的 <see cref="T:System.DateTime"/> 是否大於另一個指定的 <see cref="T:System.DateTime"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="t1"/> 大於 <paramref name="t2"/>，則為 true，否則為 false。
    /// </returns>
    /// <param name="t1">要比較的第一個物件。</param><param name="t2">要比較的第二個物件。</param><filterpriority>3</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool operator >(DateTime t1, DateTime t2);
    /// <summary>
    /// 判斷某個指定的 <see cref="T:System.DateTime"/> 是否大於或等於另一個指定的 <see cref="T:System.DateTime"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="t1"/> 大於或等於 <paramref name="t2"/> 則為 true，否則為 false。
    /// </returns>
    /// <param name="t1">要比較的第一個物件。</param><param name="t2">要比較的第二個物件。</param><filterpriority>3</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool operator >=(DateTime t1, DateTime t2);
    /// <summary>
    /// 傳回新的 <see cref="T:System.DateTime"/>，這是將這個執行個體的值加上指定之 <see cref="T:System.TimeSpan"/> 值的結果。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為這個執行個體所表示日期和時間加上 <paramref name="value"/> 所表示時間間隔的總和。
    /// </returns>
    /// <param name="value">正數或負數時間間隔。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    public DateTime Add(TimeSpan value);
    /// <summary>
    /// 傳回新的 <see cref="T:System.DateTime"/>，這是將這個執行個體的值加上指定之天數的結果。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為這個執行個體所表示日期和時間加上 <paramref name="value"/> 所表示天數的總和。
    /// </returns>
    /// <param name="value">整數和小數的天數。 <paramref name="value"/> 參數可以是負數或正數。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DateTime AddDays(double value);
    /// <summary>
    /// 傳回新的 <see cref="T:System.DateTime"/>，這是將這個執行個體的值加上指定之時數的結果。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為這個執行個體所表示日期和時間加上 <paramref name="value"/> 所表示小時數的總和。
    /// </returns>
    /// <param name="value">整數和小數的時數。 <paramref name="value"/> 參數可以是負數或正數。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DateTime AddHours(double value);
    /// <summary>
    /// 傳回新的 <see cref="T:System.DateTime"/>，這是將這個執行個體的值加上指定之毫秒數的結果。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為這個執行個體所表示日期和時間加上 <paramref name="value"/> 所表示毫秒數的總和。
    /// </returns>
    /// <param name="value">整數和小數的毫秒數。 <paramref name="value"/> 參數可以是負數或正數。 請注意，這個值會四捨五入至最接近的整數。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DateTime AddMilliseconds(double value);
    /// <summary>
    /// 傳回新的 <see cref="T:System.DateTime"/>，這是將這個執行個體的值加上指定之分鐘數的結果。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為這個執行個體所表示日期和時間加上 <paramref name="value"/> 所表示分鐘數的總和。
    /// </returns>
    /// <param name="value">整數和小數的分鐘數。 <paramref name="value"/> 參數可以是負數或正數。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DateTime AddMinutes(double value);
    /// <summary>
    /// 傳回新的 <see cref="T:System.DateTime"/>，這是將這個執行個體的值加上指定之月數的結果。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為這個執行個體所表示日期和時間加上 <paramref name="months"/> 的總和。
    /// </returns>
    /// <param name="months">月份數。 <paramref name="months"/> 參數可以是負數或正數。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。 -或- <paramref name="months"/> 小於 -120,000 或大於 120,000。</exception><filterpriority>2</filterpriority>
    public DateTime AddMonths(int months);
    /// <summary>
    /// 傳回新的 <see cref="T:System.DateTime"/>，這是將這個執行個體的值加上指定之秒數的結果。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為這個執行個體所表示日期和時間加上 <paramref name="value"/> 所表示秒數的總和。
    /// </returns>
    /// <param name="value">整數和小數的秒數。 <paramref name="value"/> 參數可以是負數或正數。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DateTime AddSeconds(double value);
    /// <summary>
    /// 傳回新的 <see cref="T:System.DateTime"/>，這是將這個執行個體的值加上指定之刻度數的結果。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為這個執行個體所表示日期和時間加上 <paramref name="value"/> 所表示時間的總和。
    /// </returns>
    /// <param name="value">100 毫微秒刻度數。 <paramref name="value"/> 參數可以為正數或是負數。</param><exception cref="T:System.ArgumentOutOfRangeException">產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    public DateTime AddTicks(long value);
    /// <summary>
    /// 傳回新的 <see cref="T:System.DateTime"/>，這是將這個執行個體的值加上指定之年數的結果。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為這個執行個體所表示日期和時間加上 <paramref name="value"/> 所表示年份數的總和。
    /// </returns>
    /// <param name="value">年份數。 <paramref name="value"/> 參數可以是負數或正數。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="value"/> 或產生的 <see cref="T:System.DateTime"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    public DateTime AddYears(int value);
    /// <summary>
    /// 比較 <see cref="T:System.DateTime"/> 的兩個執行個體，並傳回整數，這個整數表示第一個執行個體早於、同於或晚於第二個執行個體。
    /// </summary>
    /// 
    /// <returns>
    /// 帶正負號的數字，表示 <paramref name="t1"/> 和 <paramref name="t2"/> 的相對值。 實值型別 條件 小於零 <paramref name="t1"/> 早於 <paramref name="t2"/>。 Zero <paramref name="t1"/> 與 <paramref name="t2"/> 相同。 大於零 <paramref name="t1"/> 晚於 <paramref name="t2"/>。
    /// </returns>
    /// <param name="t1">要比較的第一個物件。</param><param name="t2">要比較的第二個物件。</param><filterpriority>1</filterpriority>
    public static int Compare(DateTime t1, DateTime t2);
    /// <summary>
    /// 比較這個執行個體的值與含有指定之 <see cref="T:System.DateTime"/> 值的指定物件，並且傳回一個整數，指出這個執行個體是早於、同於或晚於指定的 <see cref="T:System.DateTime"/> 值。
    /// </summary>
    /// 
    /// <returns>
    /// 帶正負號的數字，指出這個執行個體與 <paramref name="value"/> 的相對值。 值 描述 小於零 這個執行個體早於 <paramref name="value"/>。 Zero 這個執行個體和 <paramref name="value"/> 相同。 大於零 這個執行個體晚於 <paramref name="value"/>，或者 <paramref name="value"/> 是 null。
    /// </returns>
    /// <param name="value">要比較的 Boxed 物件，或 null。</param><exception cref="T:System.ArgumentException"><paramref name="value"/> 不是 <see cref="T:System.DateTime"/>。</exception><filterpriority>2</filterpriority>
    public int CompareTo(object value);
    /// <summary>
    /// 比較這個執行個體的值與指定的 <see cref="T:System.DateTime"/> 值，並且傳回一個整數，指出這個執行個體是早於、同於或晚於指定的 <see cref="T:System.DateTime"/> 值。
    /// </summary>
    /// 
    /// <returns>
    /// 帶正負號的數字，指出這個執行個體與 <paramref name="value"/> 參數的相對值。 值 描述 小於零 這個執行個體早於 <paramref name="value"/>。 Zero 這個執行個體和 <paramref name="value"/> 相同。 大於零 這個執行個體晚於 <paramref name="value"/>。
    /// </returns>
    /// <param name="value">要與目前執行個體相比較的物件。</param><filterpriority>2</filterpriority>
    public int CompareTo(DateTime value);
    /// <summary>
    /// 傳回指定之月份和年份中的天數。
    /// </summary>
    /// 
    /// <returns>
    /// 指定 <paramref name="year"/> 的 <paramref name="month"/> 中的天數。 例如，如果 <paramref name="month"/> 等於 2 (表示二月)，傳回值是 28 或 29 (根據 <paramref name="year"/> 是否為閏年)。
    /// </returns>
    /// <param name="year">年。</param><param name="month">月 (範圍從 1 到 12 的數字)。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="month"/> 小於 1 或大於 12。 -或- <paramref name="year"/> 小於 1 或大於 9999。</exception><filterpriority>1</filterpriority>
    public static int DaysInMonth(int year, int month);
    /// <summary>
    /// 傳回值，指出這個執行個體 (Instance) 是否和指定的物件相等。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="value"/> 是 <see cref="T:System.DateTime"/> 的執行個體，並且等於這個執行個體的值，則為 true，否則為 false。
    /// </returns>
    /// <param name="value">要與這個執行個體相比較的物件。</param><filterpriority>2</filterpriority>
    public override bool Equals(object value);
    /// <summary>
    /// 傳回值，指出這個執行個體的值是否等於指定的 <see cref="T:System.DateTime"/> 執行個體的值。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="value"/> 參數等於這個執行個體的值，則為 true，否則為 false。
    /// </returns>
    /// <param name="value">要與這個執行個體相比較的物件。</param><filterpriority>2</filterpriority>
    public bool Equals(DateTime value);
    /// <summary>
    /// 傳回值，指出兩個 <see cref="T:System.DateTime"/> 執行個體是否有相同的日期和時間值。
    /// </summary>
    /// 
    /// <returns>
    /// 如果兩個值相等，則為 true，否則為 false。
    /// </returns>
    /// <param name="t1">要比較的第一個物件。</param><param name="t2">要比較的第二個物件。</param><filterpriority>1</filterpriority>
    public static bool Equals(DateTime t1, DateTime t2);
    /// <summary>
    /// 還原序列化 64 位元的二進位值，並重新建立原始的序列化 <see cref="T:System.DateTime"/> 物件。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，與 <see cref="M:System.DateTime.ToBinary"/> 方法所序列化的 <see cref="T:System.DateTime"/> 物件相等。
    /// </returns>
    /// <param name="dateData">64 位元帶正負號的整數，可在 2 位元欄位中編碼 <see cref="P:System.DateTime.Kind"/> 屬性，並在 62 位元欄位中編碼 <see cref="P:System.DateTime.Ticks"/> 屬性。</param><exception cref="T:System.ArgumentException"><paramref name="dateData"/> 小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>1</filterpriority>
    public static DateTime FromBinary(long dateData);
    /// <summary>
    /// 將指定的 Windows 檔案時間轉換成相等的本地時間。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，表示 <paramref name="fileTime"/> 參數所表示日期和時間的本地時間對應項。
    /// </returns>
    /// <param name="fileTime">Windows 檔案時間以刻度表示。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="fileTime"/> 小於 0，或表示大於 <see cref="F:System.DateTime.MaxValue"/> 的時間。</exception><filterpriority>1</filterpriority>
    public static DateTime FromFileTime(long fileTime);
    /// <summary>
    /// 將指定的 Windows 檔案時間轉換成相等的 UTC 時間。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，表示 <paramref name="fileTime"/> 參數所表示日期和時間的 UTC 時間對應項。
    /// </returns>
    /// <param name="fileTime">Windows 檔案時間以刻度表示。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="fileTime"/> 小於 0，或表示大於 <see cref="F:System.DateTime.MaxValue"/> 的時間。</exception><filterpriority>1</filterpriority>
    public static DateTime FromFileTimeUtc(long fileTime);
    /// <summary>
    /// 傳回等於指定 OLE Automation 日期的 <see cref="T:System.DateTime"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，表示和 <paramref name="d"/> 相同的日期和時間。
    /// </returns>
    /// <param name="d">OLE Automation 日期值。</param><exception cref="T:System.ArgumentException">日期不是有效的 OLE Automation 日期值。</exception><filterpriority>1</filterpriority>
    public static DateTime FromOADate(double d);
    /// <summary>
    /// 使用序列化目前 <see cref="T:System.DateTime"/> 物件所需的資料來填入 <see cref="T:System.Runtime.Serialization.SerializationInfo"/> 物件。
    /// </summary>
    /// <param name="info">要填入資料的物件。</param><param name="context">這個序列化的目的端。 (不使用這個參數；請指定 null)。</param><exception cref="T:System.ArgumentNullException"><paramref name="info"/> 為 null。</exception>
    [SecurityCritical]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context);
    /// <summary>
    /// 指出這個 <see cref="T:System.DateTime"/> 執行個體是否在目前時區的日光節約時間範圍內。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <see cref="P:System.DateTime.Kind"/> 為 <see cref="F:System.DateTimeKind.Local"/> 或 <see cref="F:System.DateTimeKind.Unspecified"/>，而且這個 <see cref="T:System.DateTime"/> 執行個體的值位於目前時區的日光節約時間範圍內，則為 true。 如果 <see cref="P:System.DateTime.Kind"/> 為 <see cref="F:System.DateTimeKind.Utc"/>，則為 false，
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public bool IsDaylightSavingTime();
    /// <summary>
    /// 建立新的 <see cref="T:System.DateTime"/> 物件，此物件的刻度數與指定的 <see cref="T:System.DateTime"/> 相同，但依指定的 <see cref="T:System.DateTimeKind"/> 值所示，指定為本地時間、國際標準時間 (Coordinated Universal Time，UTC)，或兩者都不是。
    /// </summary>
    /// 
    /// <returns>
    /// 新的物件，這個物件的刻度數與 <paramref name="value"/> 參數表示的物件相同，且具有 <paramref name="kind"/> 參數指定的 <see cref="T:System.DateTimeKind"/> 值。
    /// </returns>
    /// <param name="value">日期和時間。</param><param name="kind">一個列舉值，表示新的物件是表示本地時間、UTC，還是兩者都不是。</param><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static DateTime SpecifyKind(DateTime value, DateTimeKind kind);
    /// <summary>
    /// 將目前的 <see cref="T:System.DateTime"/> 物件序列化成 64 位元的二進位值，之後可以用這個值來重新建立 <see cref="T:System.DateTime"/> 物件。
    /// </summary>
    /// 
    /// <returns>
    /// 64 位元帶正負號的整數，可以編碼 <see cref="P:System.DateTime.Kind"/> 和 <see cref="P:System.DateTime.Ticks"/> 屬性。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public long ToBinary();
    /// <summary>
    /// 傳回這個執行個體的雜湊程式碼。
    /// </summary>
    /// 
    /// <returns>
    /// 32 位元帶正負號的整數雜湊程式碼。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public override int GetHashCode();
    /// <summary>
    /// 傳回指定年份是否為閏年的指示。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="year"/> 是閏年，則為 true，否則為 false。
    /// </returns>
    /// <param name="year">4 位數的年份。</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="year"/> 小於 1 或大於 9999。</exception><filterpriority>1</filterpriority>
    public static bool IsLeapYear(int year);
    /// <summary>
    /// 將指定之日期和時間的字串表示轉換為它的 <see cref="T:System.DateTime"/> 對應項。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，與 <paramref name="s"/> 中包含的日期和時間相等。
    /// </returns>
    /// <param name="s">字串，含有要轉換的日期和時間。</param><exception cref="T:System.ArgumentNullException"><paramref name="s"/> 為 null。</exception><exception cref="T:System.FormatException"><paramref name="s"/> 不包含日期和時間的有效字串表示。</exception><filterpriority>1</filterpriority>
    public static DateTime Parse(string s);
    /// <summary>
    /// 使用指定的特定文化特性格式資訊，將日期和時間的指定字串表示轉換為其對等的 <see cref="T:System.DateTime"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，與 <paramref name="s"/> 中包含的日期和時間相等，如 <paramref name="provider"/> 所指定。
    /// </returns>
    /// <param name="s">字串，含有要轉換的日期和時間。</param><param name="provider">提供關於 <paramref name="s"/> 之文化特性特有格式資訊的物件。</param><exception cref="T:System.ArgumentNullException"><paramref name="s"/> 為 null。</exception><exception cref="T:System.FormatException"><paramref name="s"/> 不包含日期和時間的有效字串表示。</exception><filterpriority>1</filterpriority>
    public static DateTime Parse(string s, IFormatProvider provider);
    /// <summary>
    /// 使用指定的特定文化特性格式資訊和格式樣式，將日期和時間的指定字串表示轉換為其對等的 <see cref="T:System.DateTime"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，與 <paramref name="s"/> 中包含的日期和時間相等，如 <paramref name="provider"/> 和 <paramref name="styles"/> 所指定。
    /// </returns>
    /// <param name="s">字串，含有要轉換的日期和時間。</param><param name="provider">物件，提供關於 <paramref name="s"/> 的文化特性特定格式資訊。</param><param name="styles">列舉值的位元組合，表示可以在 <paramref name="s"/> 中出現使剖析作業成功，以及定義如何解譯剖析的日期與目前時區或目前日期之間關聯性的樣式項目。 一般會指定的值是 <see cref="F:System.Globalization.DateTimeStyles.None"/>。</param><exception cref="T:System.ArgumentNullException"><paramref name="s"/> 為 null。</exception><exception cref="T:System.FormatException"><paramref name="s"/> 不包含日期和時間的有效字串表示。</exception><exception cref="T:System.ArgumentException"><paramref name="styles"/> 包含無效的 <see cref="T:System.Globalization.DateTimeStyles"/> 值組合。 例如，<see cref="F:System.Globalization.DateTimeStyles.AssumeLocal"/> 和 <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal"/> 兩者。</exception><filterpriority>1</filterpriority>
    public static DateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles);
    /// <summary>
    /// 使用指定的格式以及特定文化特性的格式資訊，將日期和時間的指定字串表示轉換為其對等的 <see cref="T:System.DateTime"/>。 字串表示的格式必須完全符合指定的格式。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，與 <paramref name="s"/> 中包含的日期和時間相等，如 <paramref name="format"/> 和 <paramref name="provider"/> 所指定。
    /// </returns>
    /// <param name="s">字串，包含要轉換的日期和時間。</param><param name="format">格式規範，定義 <paramref name="s"/> 的所需格式。</param><param name="provider">提供關於 <paramref name="s"/> 之文化特性特有格式資訊的物件。</param><exception cref="T:System.ArgumentNullException"><paramref name="s"/> 或 <paramref name="format"/> 是 null。</exception><exception cref="T:System.FormatException"><paramref name="s"/> 或 <paramref name="format"/> 是空字串。 -或- <paramref name="s"/> 不包含對應至 <paramref name="format"/> 中指定模式的日期和時間。 -或- <paramref name="s"/> 中的小時元件和 AM/PM 指示項不一致。</exception><filterpriority>2</filterpriority>
    public static DateTime ParseExact(string s, string format, IFormatProvider provider);
    /// <summary>
    /// 使用指定的格式、特定文化特性格式資訊以及樣式，將日期和時間的指定字串表示轉換為其對等的 <see cref="T:System.DateTime"/>。 字串表示的格式必須完全符合指定的格式，否則會擲回例外狀況。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，與 <paramref name="s"/> 中包含的日期和時間相等，如 <paramref name="format"/>、<paramref name="provider"/> 和 <paramref name="style"/> 所指定。
    /// </returns>
    /// <param name="s">字串，含有要轉換的日期和時間。</param><param name="format">格式規範，定義 <paramref name="s"/> 的所需格式。</param><param name="provider">物件，提供關於 <paramref name="s"/> 的文化特性特定格式資訊。</param><param name="style">列舉值的位元組合，提供有關 <paramref name="s"/>、可以出現在 <paramref name="s"/> 中的樣式項目，或是從 <paramref name="s"/> 轉換成 <see cref="T:System.DateTime"/> 值的詳細資訊。 一般會指定的值是 <see cref="F:System.Globalization.DateTimeStyles.None"/>。</param><exception cref="T:System.ArgumentNullException"><paramref name="s"/> 或 <paramref name="format"/> 是 null。</exception><exception cref="T:System.FormatException"><paramref name="s"/> 或 <paramref name="format"/> 是空字串。 -或- <paramref name="s"/> 不包含對應至 <paramref name="format"/> 中指定模式的日期和時間。 -或- <paramref name="s"/> 中的小時元件和 AM/PM 指示項不一致。</exception><exception cref="T:System.ArgumentException"><paramref name="style"/> 包含無效的 <see cref="T:System.Globalization.DateTimeStyles"/> 值組合。 例如，<see cref="F:System.Globalization.DateTimeStyles.AssumeLocal"/> 和 <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal"/> 兩者。</exception><filterpriority>2</filterpriority>
    public static DateTime ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style);
    /// <summary>
    /// 使用指定的格式陣列、特定文化特性格式資訊以及樣式，將日期和時間的指定字串表示轉換為其對等的 <see cref="T:System.DateTime"/>。 字串表示的格式必須至少完全符合其中一個指定的格式，否則會擲回例外狀況。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，與 <paramref name="s"/> 中包含的日期和時間相等，如 <paramref name="formats"/>、<paramref name="provider"/> 和 <paramref name="style"/> 所指定。
    /// </returns>
    /// <param name="s">字串，含有要轉換的一個或多個日期和時間。</param><param name="formats"><paramref name="s"/> 允許的格式陣列。</param><param name="provider">提供關於 <paramref name="s"/> 之文化特性特有格式資訊的物件。</param><param name="style">列舉值的位元組合，表示 <paramref name="s"/> 的允許格式。 一般會指定的值是 <see cref="F:System.Globalization.DateTimeStyles.None"/>。</param><exception cref="T:System.ArgumentNullException"><paramref name="s"/> 或 <paramref name="formats"/> 是 null。</exception><exception cref="T:System.FormatException"><paramref name="s"/> 為空字串。 -或- <paramref name="formats"/> 的某個項目是空字串。 -或- <paramref name="s"/> 不包含對應至 <paramref name="formats"/> 任何項目的日期和時間。 -或- <paramref name="s"/> 中的小時元件和 AM/PM 指示項不一致。</exception><exception cref="T:System.ArgumentException"><paramref name="style"/> 包含無效的 <see cref="T:System.Globalization.DateTimeStyles"/> 值組合。 例如，<see cref="F:System.Globalization.DateTimeStyles.AssumeLocal"/> 和 <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal"/> 兩者。</exception><filterpriority>2</filterpriority>
    public static DateTime ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style);
    /// <summary>
    /// 將這個執行個體減去指定的日期和時間。
    /// </summary>
    /// 
    /// <returns>
    /// 時間間隔，等於由此執行個體所表示的日期和時間減去由 <paramref name="value"/> 所表示的日期和時間。
    /// </returns>
    /// <param name="value">要減去的日期和時間。</param><exception cref="T:System.ArgumentOutOfRangeException">該結果小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public TimeSpan Subtract(DateTime value);
    /// <summary>
    /// 將這個執行個體減去指定的持續期間。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，等於由此執行個體所表示的日期和時間減去由 <paramref name="value"/> 所表示的時間間隔。
    /// </returns>
    /// <param name="value">要減去的時間間隔。</param><exception cref="T:System.ArgumentOutOfRangeException">該結果小於 <see cref="F:System.DateTime.MinValue"/> 或大於 <see cref="F:System.DateTime.MaxValue"/>。</exception><filterpriority>2</filterpriority>
    public DateTime Subtract(TimeSpan value);
    /// <summary>
    /// 將這個執行個體的值轉換為對等的 OLE Automation 日期。
    /// </summary>
    /// 
    /// <returns>
    /// 雙精確度浮點數，含有等於這個執行個體值的 OLE Automation 日期。
    /// </returns>
    /// <exception cref="T:System.OverflowException">這個執行個體的值無法顯示為 OLE Automation 日期。</exception><filterpriority>2</filterpriority>
    public double ToOADate();
    /// <summary>
    /// 將目前 <see cref="T:System.DateTime"/> 物件的值轉換成 Windows 檔案時間。
    /// </summary>
    /// 
    /// <returns>
    /// 以 Windows 檔案時間表示的目前 <see cref="T:System.DateTime"/> 物件的值。
    /// </returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">產生的檔案時間將會表示在西元 1601 年 1 月 1 日午夜 12:00 以前的日期和時間， 時區為 UTC。</exception><filterpriority>2</filterpriority>
    public long ToFileTime();
    /// <summary>
    /// 將目前 <see cref="T:System.DateTime"/> 物件的值轉換成 Windows 檔案時間。
    /// </summary>
    /// 
    /// <returns>
    /// 以 Windows 檔案時間表示的目前 <see cref="T:System.DateTime"/> 物件的值。
    /// </returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">產生的檔案時間將會表示在西元 1601 年 1 月 1 日午夜 12:00 以前的日期和時間， 時區為 UTC。</exception><filterpriority>2</filterpriority>
    public long ToFileTimeUtc();
    /// <summary>
    /// 將目前 <see cref="T:System.DateTime"/> 物件的值轉換成本地時間。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其 <see cref="P:System.DateTime.Kind"/> 屬性為 <see cref="F:System.DateTimeKind.Local"/>，而其值為與目前 <see cref="T:System.DateTime"/> 物件的值相等的本地時間；如果轉換過的值太大，而無法由 <see cref="T:System.DateTime"/> 物件表示，則為 <see cref="F:System.DateTime.MaxValue"/>；如果轉換過的值太小，而無法表示為 <see cref="T:System.DateTime"/> 物件，則為 <see cref="F:System.DateTime.MinValue"/>。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DateTime ToLocalTime();
    /// <summary>
    /// 將目前 <see cref="T:System.DateTime"/> 物件的值轉換為其對等的完整日期 (Long Date) 字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 字串，內含目前 <see cref="T:System.DateTime"/> 物件的完整日期字串表示。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public string ToLongDateString();
    /// <summary>
    /// 將目前 <see cref="T:System.DateTime"/> 物件的值轉換為其對等的完整時間 (Long Time) 字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 字串，內含目前 <see cref="T:System.DateTime"/> 物件的完整時間字串表示。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public string ToLongTimeString();
    /// <summary>
    /// 將目前 <see cref="T:System.DateTime"/> 物件的值轉換為其對等的簡短日期 (Short Date) 字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 字串，內含目前 <see cref="T:System.DateTime"/> 物件的簡短日期字串表示。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public string ToShortDateString();
    /// <summary>
    /// 將目前 <see cref="T:System.DateTime"/> 物件的值轉換為其對等的簡短時間 (Short Time) 字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 字串，內含目前 <see cref="T:System.DateTime"/> 物件的簡短時間字串表示。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public string ToShortTimeString();
    /// <summary>
    /// 將目前 <see cref="T:System.DateTime"/> 物件的值轉換為其對等的字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 目前 <see cref="T:System.DateTime"/> 物件值的字串表示。
    /// </returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">日期與時間超出目前文化特性使用之行事曆所支援的日期範圍。</exception><filterpriority>1</filterpriority>
    public override string ToString();
    /// <summary>
    /// 使用指定的格式，將目前 <see cref="T:System.DateTime"/> 物件的值轉換為其對等字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 如 <paramref name="format"/> 所指定之目前 <see cref="T:System.DateTime"/> 物件值的字串表示。
    /// </returns>
    /// <param name="format">標準或自訂日期和時間格式字串 (請參閱＜備註＞)。</param><exception cref="T:System.FormatException"><paramref name="format"/> 的長度為 1，而且不是為 <see cref="T:System.Globalization.DateTimeFormatInfo"/> 所定義的格式規範字元之一。 -或- <paramref name="format"/> 並沒不包含有效的自訂格式模式。</exception><exception cref="T:System.ArgumentOutOfRangeException">日期與時間超出目前文化特性使用之行事曆所支援的日期範圍。</exception><filterpriority>1</filterpriority>
    public string ToString(string format);
    /// <summary>
    /// 使用指定的特定文化特性的格式資訊，將目前 <see cref="T:System.DateTime"/> 物件的值轉換為其對等字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 目前 <see cref="T:System.DateTime"/> 物件值的字串表示，如 <paramref name="provider"/> 所指定。
    /// </returns>
    /// <param name="provider">物件，提供文化特性特定格式資訊。</param><exception cref="T:System.ArgumentOutOfRangeException">日期與時間超出 <paramref name="provider"/> 使用之日曆所支援的日期範圍。</exception><filterpriority>1</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string ToString(IFormatProvider provider);
    /// <summary>
    /// 使用指定的格式和特定文化特性的格式資訊，將目前 <see cref="T:System.DateTime"/> 物件的值轉換為其對等字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 依 <paramref name="format"/> 和 <paramref name="provider"/> 指定之目前 <see cref="T:System.DateTime"/> 物件值的字串表示。
    /// </returns>
    /// <param name="format">標準或自訂的日期和時間格式字串。</param><param name="provider">物件，提供文化特性特定格式資訊。</param><exception cref="T:System.FormatException"><paramref name="format"/> 的長度為 1，而且不是為 <see cref="T:System.Globalization.DateTimeFormatInfo"/> 所定義的格式規範字元之一。 -或- <paramref name="format"/> 並沒不包含有效的自訂格式模式。</exception><exception cref="T:System.ArgumentOutOfRangeException">日期與時間超出 <paramref name="provider"/> 使用之日曆所支援的日期範圍。</exception><filterpriority>1</filterpriority>
    public string ToString(string format, IFormatProvider provider);
    /// <summary>
    /// 將目前 <see cref="T:System.DateTime"/> 物件的值轉換成 Coordinated Universal Time (UTC)。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其 <see cref="P:System.DateTime.Kind"/> 屬性為 <see cref="F:System.DateTimeKind.Utc"/>，而其值為與目前 <see cref="T:System.DateTime"/> 物件的值相等的 UTC 時間；如果轉換過的值太大，而無法由 <see cref="T:System.DateTime"/> 物件表示，則為 <see cref="F:System.DateTime.MaxValue"/>；如果轉換過的值太小，而無法由 <see cref="T:System.DateTime"/> 物件表示，則為 <see cref="F:System.DateTime.MinValue"/>。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public DateTime ToUniversalTime();
    /// <summary>
    /// 將日期及時間的指定字串表示轉換為它的 <see cref="T:System.DateTime"/> 對等用法，並且傳回指出轉換是否成功的值。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="s"/> 參數轉換成功，則為 true，否則為 false。
    /// </returns>
    /// <param name="s">字串，含有要轉換的日期和時間。</param><param name="result">當這個方法傳回時，如果轉換成功，則會包含相當於 <paramref name="s"/> 中所含之日期和時間的 <see cref="T:System.DateTime"/> 值；如果轉換失敗，則為 <see cref="F:System.DateTime.MinValue"/>。 如果 <paramref name="s"/> 參數為 null、為空字串 ("") 或者不包含日期和時間的有效字串表示，則轉換會失敗。 這個參數會以未初始化的狀態傳遞。</param><filterpriority>1</filterpriority>
    public static bool TryParse(string s, out DateTime result);
    /// <summary>
    /// 使用指定的文化特性特定格式資訊和格式樣式，將日期和時間的指定字串表示轉換為其對等的 <see cref="T:System.DateTime"/>，並傳回值，這個值表示轉換是否成功。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="s"/> 參數轉換成功，則為 true，否則為 false。
    /// </returns>
    /// <param name="s">字串，含有要轉換的日期和時間。</param><param name="provider">物件，提供關於 <paramref name="s"/> 的文化特性特定格式資訊。</param><param name="styles">列舉值之位元組合的指標，這些列舉值會定義如何根據目前時區或目前日期解譯已剖析的日期。 一般會指定的值是 <see cref="F:System.Globalization.DateTimeStyles.None"/>。</param><param name="result">當這個方法傳回時，如果轉換成功，則會包含相當於 <paramref name="s"/> 中所含之日期和時間的 <see cref="T:System.DateTime"/> 值；如果轉換失敗，則為 <see cref="F:System.DateTime.MinValue"/>。 如果 <paramref name="s"/> 參數為 null、為空字串 ("") 或者不包含日期和時間的有效字串表示，則轉換會失敗。 這個參數會以未初始化的狀態傳遞。</param><exception cref="T:System.ArgumentException"><paramref name="styles"/> 不是有效的 <see cref="T:System.Globalization.DateTimeStyles"/> 值。 -或- <paramref name="styles"/> 包含 <see cref="T:System.Globalization.DateTimeStyles"/> 值的無效組合 (例如 <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal"/> 和 <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal"/> 兩者)。</exception><exception cref="T:System.NotSupportedException"><paramref name="provider"/> 屬於中性的文化特性，而且不能用於剖析作業中。</exception><filterpriority>1</filterpriority>
    public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result);
    /// <summary>
    /// 使用指定的格式、特定文化特性格式資訊以及樣式，將日期和時間的指定字串表示轉換為其對等的 <see cref="T:System.DateTime"/>。 字串表示的格式必須完全符合指定的格式。 此方法會傳回值，這個值表示轉換是否成功。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="s"/> 轉換成功則為 true，否則為 false。
    /// </returns>
    /// <param name="s">字串，含有要轉換的日期和時間。</param><param name="format"><paramref name="s"/> 的必要格式。</param><param name="provider">物件，提供關於 <paramref name="s"/> 的文化特性特定格式資訊。</param><param name="style">一個或多個列舉值的位元組合，表示可以在 <paramref name="s"/> 中使用的格式。</param><param name="result">當這個方法傳回時，如果轉換成功，則會包含相當於 <paramref name="s"/> 中所含之日期和時間的 <see cref="T:System.DateTime"/> 值；如果轉換失敗，則為 <see cref="F:System.DateTime.MinValue"/>。 如果 <paramref name="s"/> 或 <paramref name="format"/> 參數為 null、空字串，或不包含與 <paramref name="format"/> 中指定之模式相對應的日期和時間，則此轉換作業會失敗。 這個參數會以未初始化的狀態傳遞。</param><exception cref="T:System.ArgumentException"><paramref name="styles"/> 不是有效的 <see cref="T:System.Globalization.DateTimeStyles"/> 值。 -或- <paramref name="styles"/> 包含 <see cref="T:System.Globalization.DateTimeStyles"/> 值的無效組合 (例如 <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal"/> 和 <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal"/> 兩者)。</exception><filterpriority>1</filterpriority>
    public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style, out DateTime result);
    /// <summary>
    /// 使用指定的格式陣列、特定文化特性格式資訊以及樣式，將日期和時間的指定字串表示轉換為其對等的 <see cref="T:System.DateTime"/>。 字串表示的格式必須至少完全符合其中一個指定格式。 此方法會傳回值，這個值表示轉換是否成功。
    /// </summary>
    /// 
    /// <returns>
    /// 如果 <paramref name="s"/> 參數轉換成功，則為 true，否則為 false。
    /// </returns>
    /// <param name="s">字串，含有要轉換的一個或多個日期和時間。</param><param name="formats"><paramref name="s"/> 允許的格式陣列。</param><param name="provider">提供關於 <paramref name="s"/> 之文化特性特有格式資訊的物件。</param><param name="style">列舉值的位元組合，表示 <paramref name="s"/> 的允許格式。 一般會指定的值是 <see cref="F:System.Globalization.DateTimeStyles.None"/>。</param><param name="result">當這個方法傳回時，如果轉換成功，則會包含相當於 <paramref name="s"/> 中所含之日期和時間的 <see cref="T:System.DateTime"/> 值；如果轉換失敗，則為 <see cref="F:System.DateTime.MinValue"/>。 如果 <paramref name="s"/> 或 <paramref name="formats"/> 為 null、<paramref name="s"/> 或 <paramref name="formats"/> 的一個項目為空字串，或是 <paramref name="s"/> 的格式沒有與 <paramref name="formats"/> 中的至少一個格式模式所指定的內容完全相同，則此轉換作業會失敗。 這個參數會以未初始化的狀態傳遞。</param><exception cref="T:System.ArgumentException"><paramref name="styles"/> 不是有效的 <see cref="T:System.Globalization.DateTimeStyles"/> 值。 -或- <paramref name="styles"/> 包含 <see cref="T:System.Globalization.DateTimeStyles"/> 值的無效組合 (例如 <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal"/> 和 <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal"/> 兩者)。</exception><filterpriority>1</filterpriority>
    public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out DateTime result);
    /// <summary>
    /// 將這個執行個體的值轉換為標準日期和時間格式規範所支援的所有字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 字串陣列，其中的每個元素都是這個執行個體的值使用其中一個標準日期和時間格式規範所格式化的表示。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public string[] GetDateTimeFormats();
    /// <summary>
    /// 將這個執行個體的值轉換為標準日期和時間格式規範和指定的特定文化特性格式資訊所支援的所有字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 字串陣列，其中的每個元素都是這個執行個體的值使用其中一個標準日期和時間格式規範所格式化的表示。
    /// </returns>
    /// <param name="provider">物件，提供關於這個執行個體的文化特性格式資訊。</param><filterpriority>2</filterpriority>
    public string[] GetDateTimeFormats(IFormatProvider provider);
    /// <summary>
    /// 將這個執行個體的值轉換為指定的標準日期和時間格式規範所支援的所有字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 字串陣列，其中的每個元素都是這個執行個體的值使用 <paramref name="format"/> 標準日期和時間格式規範所格式化的表示。
    /// </returns>
    /// <param name="format">標準日期和時間格式字串 (請參閱＜備註＞)。</param><exception cref="T:System.FormatException"><paramref name="format"/> 不是有效的標準日期和時間格式規範字元。</exception><filterpriority>2</filterpriority>
    public string[] GetDateTimeFormats(char format);
    /// <summary>
    /// 將這個執行個體的值轉換為指定的標準日期和時間格式規範和特定文化特性格式資訊所支援的所有字串表示。
    /// </summary>
    /// 
    /// <returns>
    /// 字串陣列，其中的每個元素都是這個執行個體的值使用其中一個標準日期和時間格式規範所格式化的表示。
    /// </returns>
    /// <param name="format">日期和時間格式字串 (請參閱＜備註＞)。</param><param name="provider">物件，提供關於這個執行個體的文化特性格式資訊。</param><exception cref="T:System.FormatException"><paramref name="format"/> 不是有效的標準日期和時間格式規範字元。</exception><filterpriority>2</filterpriority>
    public string[] GetDateTimeFormats(char format, IFormatProvider provider);
    /// <summary>
    /// 傳回實值型別 <see cref="T:System.DateTime"/> 的 <see cref="T:System.TypeCode"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 列舉常數 <see cref="F:System.TypeCode.DateTime"/>。
    /// </returns>
    /// <filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public TypeCode GetTypeCode();
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    bool IConvertible.ToBoolean(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    char IConvertible.ToChar(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    sbyte IConvertible.ToSByte(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    byte IConvertible.ToByte(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    short IConvertible.ToInt16(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    ushort IConvertible.ToUInt16(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    int IConvertible.ToInt32(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    uint IConvertible.ToUInt32(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    long IConvertible.ToInt64(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    ulong IConvertible.ToUInt64(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    float IConvertible.ToSingle(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    double IConvertible.ToDouble(IFormatProvider provider);
    /// <summary>
    /// 這個轉換不支援。 嘗試使用這個方法會擲回 <see cref="T:System.InvalidCastException"/>。
    /// </summary>
    /// 
    /// <returns>
    /// 不使用此成員的傳回值。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.InvalidCastException">在所有情況下。</exception>
    decimal IConvertible.ToDecimal(IFormatProvider provider);
    /// <summary>
    /// 傳回目前的 <see cref="T:System.DateTime"/> 物件。
    /// </summary>
    /// 
    /// <returns>
    /// 目前的物件。
    /// </returns>
    /// <param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    DateTime IConvertible.ToDateTime(IFormatProvider provider);
    /// <summary>
    /// 將目前的 <see cref="T:System.DateTime"/> 物件轉換為指定型別的物件。
    /// </summary>
    /// 
    /// <returns>
    /// <paramref name="type"/> 參數所指定之型別的物件，其中含有等於目前 <see cref="T:System.DateTime"/> 物件的值。
    /// </returns>
    /// <param name="type">所要的型別。</param><param name="provider">實作 <see cref="T:System.IFormatProvider"/> 介面的物件 (不使用這個參數；請指定 null)。</param><exception cref="T:System.ArgumentNullException"><paramref name="type"/> 為 null。</exception><exception cref="T:System.InvalidCastException">不支援 <see cref="T:System.DateTime"/> 型別進行這種轉換。</exception>
    object IConvertible.ToType(Type type, IFormatProvider provider);
    /// <summary>
    /// 取得這個執行個體的日期部分。
    /// </summary>
    /// 
    /// <returns>
    /// 新的物件，具有與這個執行個體相同的日期，並將時間值設定為午夜 12:00:00 (00:00:00)。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public DateTime Date { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get; }
    /// <summary>
    /// 取得由這個執行個體表示的月份天數。
    /// </summary>
    /// 
    /// <returns>
    /// 日期元件，以 1 到 31 之間的值表示。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public int Day { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    /// <summary>
    /// 取得由這個執行個體表示的一週天數。
    /// </summary>
    /// 
    /// <returns>
    /// 列舉的常數，指出這個 <see cref="T:System.DateTime"/> 值代表的是一週的哪一天。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public DayOfWeek DayOfWeek { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get; }
    /// <summary>
    /// 取得由這個執行個體表示的一年天數。
    /// </summary>
    /// 
    /// <returns>
    /// 一年中的日期，以 1 到 366 之間的值表示。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public int DayOfYear { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    /// <summary>
    /// 取得這個執行個體所表示日期的小時部分。
    /// </summary>
    /// 
    /// <returns>
    /// 小時元件，以 0 到 23 之間的值表示。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public int Hour { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get; }
    /// <summary>
    /// 取得值，指出這個執行個體表示的時間是根據本地時間、Coordinated Universal Time (UTC)，或兩者皆非。
    /// </summary>
    /// 
    /// <returns>
    /// 一個列舉值，表示目前時間表示的是什麼時間。 預設值為 <see cref="F:System.DateTimeKind.Unspecified"/>。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public DateTimeKind Kind { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get; }
    /// <summary>
    /// 取得這個執行個體所表示日期的毫秒部分。
    /// </summary>
    /// 
    /// <returns>
    /// 毫秒元件，以 0 到 999 之間的值表示。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public int Millisecond { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get; }
    /// <summary>
    /// 取得這個執行個體所表示日期的分鐘部分。
    /// </summary>
    /// 
    /// <returns>
    /// 分鐘元件，以 0 到 59 之間的值表示。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public int Minute { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get; }
    /// <summary>
    /// 取得這個執行個體所表示日期的月份部分。
    /// </summary>
    /// 
    /// <returns>
    /// 月份元件，以 1 到 12 之間的值表示。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public int Month { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    /// <summary>
    /// 取得 <see cref="T:System.DateTime"/> 物件，此物件會設定為這部電腦上目前的日期和時間，以本地時間表示。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為目前的本地日期和時間。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public static DateTime Now { get; }
    /// <summary>
    /// 取得 <see cref="T:System.DateTime"/> 物件，此物件會設定為這部電腦上目前的日期和時間，以 Coordinated Universal Time (UTC) 表示。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，其值為目前的 UTC 日期和時間。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public static DateTime UtcNow { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries"), SecuritySafeCritical] get; }
    /// <summary>
    /// 取得這個執行個體所表示日期的秒數部分。
    /// </summary>
    /// 
    /// <returns>
    /// 秒數元件，以 0 到 59 之間的值表示。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public int Second { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get; }
    /// <summary>
    /// 取得表示這個執行個體日期和時間的刻度數目。
    /// </summary>
    /// 
    /// <returns>
    /// 刻度數目，表示這個執行個體的日期和時間。 值介於 DateTime.MinValue.Ticks 和 DateTime.MaxValue.Ticks 之間。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public long Ticks { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
    /// <summary>
    /// 取得這個執行個體的一天時間。
    /// </summary>
    /// 
    /// <returns>
    /// 時間間隔，表示從午夜以來已經過的當天部分。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public TimeSpan TimeOfDay { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get; }
    /// <summary>
    /// 取得目前的日期。
    /// </summary>
    /// 
    /// <returns>
    /// 物件，設定為今天的日期，且時間元件設定為 00:00:00。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public static DateTime Today { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get; }
    /// <summary>
    /// 取得這個執行個體所表示日期的年份部分。
    /// </summary>
    /// 
    /// <returns>
    /// 年份，在 1 和 9999 之間。
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public int Year { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; }
  }
}
