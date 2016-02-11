using System;

namespace Library.IO
{
    /// <summary>
    ///
    /// </summary>
    public interface IOMonitor : System.IDisposable
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        DateTime? BeginTime { get; }

        /// <summary>
        /// 是否运行
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 启动
        /// </summary>
        void Start();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
    }
}