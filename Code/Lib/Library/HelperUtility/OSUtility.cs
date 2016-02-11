using System;
using System.Diagnostics;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class OSUtility
    {
        static OSUtility()
        {
            OSName = GetOSName(Environment.OSVersion.VersionString);
            ProcessStartTime = Process.GetCurrentProcess().StartTime;
            OSStartTime = DateTime.Now - GetOSUpTime();
        }

        /// <summary>
        /// 系統運行時間
        /// </summary>
        public static TimeSpan GetOSUpTime()
        {
            using (var uptime = new PerformanceCounter("System", "System Up Time"))
            {
                uptime.NextValue();       //Call this an extra time before reading its value
                return TimeSpan.FromSeconds(uptime.NextValue());
            }
        }

        /// <summary>
        /// 系統啟動時間
        /// </summary>
        public static DateTime OSStartTime { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public static string OSName { get; private set; }

        /// <summary>
        /// 根据 User Agent 获取操作系统名称
        /// </summary>
        public static string GetOSName(string userAgent)
        {
            //http://zh.wikipedia.org/wiki/Windows_NT
            string osVersion = "unkown";

            if (userAgent.IndexOf("Windows NT 6.1", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows 7";
            }
            else if (userAgent.IndexOf("Windows NT 6.2", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows 8/Server 2012/Windows Phone 8";
            }
            else if (userAgent.IndexOf("Windows NT 10.0", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows 10";
            }
            else if (userAgent.IndexOf("Windows NT 6.3", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows 8.1/Server 2012 R2";
            }
            else if (userAgent.IndexOf("Windows NT 6.0", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows Vista/Server 2008";
            }
            else if (userAgent.IndexOf("Windows NT 5.2", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows Server 2003/Windows Server 2003 R2/Windows XP Professional x64 Edition";
            }
            else if (userAgent.IndexOf("Windows NT 5.1", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows XP";
            }
            else if (userAgent.IndexOf("Windows NT 5", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows 2000";
            }
            else if (userAgent.IndexOf("Windows NT 4", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows NT4";
            }
            else if (userAgent.IndexOf("Windows Me", StringComparison.OrdinalIgnoreCase) != -1 || userAgent.IndexOf("Win 9x 4.90", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows Me";
            }
            else if (userAgent.IndexOf("Windows 98", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows 98";
            }
            else if (userAgent.IndexOf("Windows 95", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Windows 95";
            }
            else if (userAgent.IndexOf("Mac", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Mac OS X";
            }
            else if (userAgent.IndexOf("Unix", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "UNIX";
            }
            else if (userAgent.IndexOf("Linux", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "Linux";
            }
            else if (userAgent.IndexOf("SunOS", StringComparison.OrdinalIgnoreCase) != -1)
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }

        /// <summary>
        /// 程序 CPU时间
        /// </summary>
        /// <returns></returns>
        public static string GetProcessN()
        {
            string temp;
            try
            {
                temp = ((Double)System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2") + "M";
            }
            catch
            {
                temp = "unkown";
            }
            return temp;
        }

        /// <summary>
        /// 服務 CPU时间
        /// </summary>
        /// <returns></returns>
        public static string GetServesN()
        {
            string temp;
            try
            {
                temp = ((Double)GC.GetTotalMemory(false) / 1048576).ToString("N2") + "M";
            }
            catch
            {
                temp = "unkown";
            }
            return temp;
        }

        /// <summary>
        /// 进程开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime ProcessStartTime { get; private set; }
    }
}