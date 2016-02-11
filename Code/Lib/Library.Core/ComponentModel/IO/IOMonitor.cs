using System;

namespace Library.IO
{
    /// <summary>
    ///
    /// </summary>
    public interface IOMonitor : System.IDisposable
    {
        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        DateTime? BeginTime { get; }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// ����
        /// </summary>
        void Start();

        /// <summary>
        /// ֹͣ
        /// </summary>
        void Stop();

        /// <summary>
        /// �ر�
        /// </summary>
        void Close();
    }
}