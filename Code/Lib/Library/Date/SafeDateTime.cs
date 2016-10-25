using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public static class SafeDateTime
    {
        const int SAFE_CYCLE = 7200;
        const int CYCLE_INTERVAL = 500;
        const int ALLOW_DIFF = 650;

        readonly static string[] _hosts = new[] { // list of NTP servers
        "ntp.sjtu.edu.cn", // 上海交通大学
        "time-nw.nist.gov", // Microsoft, Redmond, Washington 
        "s1a.time.edu.cn", // 北京邮电大学
        "time-b.timefreq.bldrdoc.gov", // NIST, Boulder, Colorado 
        "133.100.11.8", // 日本 福冈大学
    };
        readonly static IPEndPoint[] _eps = null;

        static int _sIndex = 0;
        static int _safeCycle = 0;
        static bool _onGetingTime = false;
        static DateTime _localUtcTime;
        static DateTime _networkUtcTime;

        static SafeDateTime()
        {
            // convert hosts to IPEndPoints
            //_eps = _hosts.Select(s => (IEnumerable<IPAddress>)Dns.GetHostAddresses(s)).Aggregate((x, y) => x.Concat(y)).Select(s => new IPEndPoint(s, 123)).ToArray();
            var list = new List<IPEndPoint>();
            foreach (var host in _hosts)
            {
                try
                {
                    foreach (var ip in Dns.GetHostAddresses(host))
                        list.Add(new IPEndPoint(ip, 123));
                }
                catch { }
            }
            _eps = list.ToArray();

            new Thread(() =>
            {
                var currentThread = Thread.CurrentThread;
                currentThread.Priority = ThreadPriority.Highest;
                currentThread.IsBackground = true;

                DateTime lastSafeTime = DateTime.MinValue;
                DateTime currentSafeTime = DateTime.MinValue;

                while (true)
                {
                    if (_safeCycle-- <= 0) // expire the safe times
                    {
                        AsyncNetworkUtcTime();
                        _safeCycle = SAFE_CYCLE;
                    }
                    else
                    {
                        currentSafeTime = GetSafeDateTime();
                        var diff = (currentSafeTime - lastSafeTime).Ticks;
                        if (Math.Abs(diff) > ALLOW_DIFF) // out of threshold
                            AsyncNetworkUtcTime();
                    }

                    lastSafeTime = GetSafeDateTime();
                    Thread.Sleep(CYCLE_INTERVAL);
                }
            }).Start();
        }

        private static DateTime GetNetworkUtcTime(IPEndPoint ep)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.Connect(ep);

            byte[] ntpData = new byte[48]; // RFC 2030 
            ntpData[0] = 0x1B;
            //for (int i = 1; i < 48; i++)
            //    ntpData[i] = 0;

            // t1, time request sent by client
            // t2, time request received by server
            // t3, time reply sent by server
            // t4, time reply received by client
            long t1, t2, t3, t4;

            t1 = DateTime.UtcNow.Ticks;
            s.Send(ntpData);
            s.Receive(ntpData);
            t4 = DateTime.UtcNow.Ticks;
            s.Close();

            t2 = ParseRaw(ntpData, 32);
            t3 = ParseRaw(ntpData, 40);

            long d = (t4 - t1) - (t3 - t2); // roundtrip delay
            long ticks = (t3 + (d >> 1));

            var timeSpan = TimeSpan.FromTicks(ticks);
            var dateTime = new DateTime(1900, 1, 1) + timeSpan;

            return dateTime; // return Utc time
        }

        private static long ParseRaw(byte[] ntpData, int offsetTransmitTime)
        {
            ulong intpart = 0;
            ulong fractpart = 0;

            for (int i = 0; i <= 3; i++)
                intpart = (intpart << 8) | ntpData[offsetTransmitTime + i];

            for (int i = 4; i <= 7; i++)
                fractpart = (fractpart << 8) | ntpData[offsetTransmitTime + i];

            ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);

            return (long)milliseconds * TimeSpan.TicksPerMillisecond;
        }

        private static void AsyncNetworkUtcTime()
        {
            if (_onGetingTime) // simple to avoid thread conflict
                return;
            _onGetingTime = true;

            bool fail = true;
            do
            {
                try
                {
                    _networkUtcTime = GetNetworkUtcTime(_eps[_sIndex]);
                    _localUtcTime = DateTime.UtcNow;
                    fail = false;
                }
                catch
                {
                    _sIndex = (_sIndex + 1) % _eps.Length;
                }
            } while (fail);

            _onGetingTime = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSafeDateTime()
        {
            var utcNow = DateTime.UtcNow;
            var interval = utcNow - _localUtcTime;
            return (_networkUtcTime + interval).ToLocalTime();
        }
    }
}
