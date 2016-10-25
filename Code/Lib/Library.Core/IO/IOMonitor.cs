using System;

namespace Library.IO
{
    /// <summary>
    ///
    /// </summary>
    public interface IOMonitor : System.IDisposable
    {
        /// <summary>
        /// 羲宎奀潔
        /// </summary>
        DateTime? BeginTime { get; }

        /// <summary>
        /// 岆瘁堍俴
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 雄
        /// </summary>
        void Start();

        /// <summary>
        /// 礿砦
        /// </summary>
        void Stop();

        /// <summary>
        /// 壽敕
        /// </summary>
        void Close();
    }
}