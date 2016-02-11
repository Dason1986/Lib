using System;
using System.Runtime.InteropServices;

namespace Library
{
    /// <summary>
    ///
    /// </summary>
    public static class IdentityGenerator
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);

        private static Guid SequentialGuid()
        {
            Guid guid;
            UuidCreateSequential(out guid);
            var s = guid.ToByteArray();
            var t = new byte[16];
            t[3] = s[0];
            t[2] = s[1];
            t[1] = s[2];
            t[0] = s[3];
            t[5] = s[4];
            t[4] = s[5];
            t[7] = s[6];
            t[6] = s[7];
            t[8] = s[8];
            t[9] = s[9];
            t[10] = s[10];
            t[11] = s[11];
            t[12] = s[12];
            t[13] = s[13];
            t[14] = s[14];
            t[15] = s[15];
            return new Guid(t);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Guid Next(Guid id)
        {
            var arry = id.ToByteArray();
            const byte sub = 1;
            var b = arry[3];
            if (b != byte.MaxValue)
            {
                arry[3] = (byte)(b + sub);
                return new Guid(arry);
            }
            arry[3] = 0;
            b = arry[2];
            if (b != byte.MaxValue)
            {
                arry[2] = (byte)(b + sub);
                return new Guid(arry);
            }
            arry[2] = 0;
            b = arry[1];
            if (b != byte.MaxValue)
            {
                arry[1] = (byte)(b + sub);
                return new Guid(arry);
            }
            arry[1] = 0;
            b = arry[0];
            arry[0] = (byte)(b + sub);
            return new Guid(arry);
        }

        static IdentityGenerator()
        {
            IsWinOS = (Environment.OSVersion.Platform == PlatformID.Win32S || Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows);
        }

        private static readonly bool IsWinOS;

        /// <summary>
        /// 該算法生成跨系統邊界secuential的GUID，非常適合數據庫
        /// </summary>
        /// <returns></returns>
        public static Guid NewGuid()
        {
            if (IsWinOS) return SequentialGuid();
            var uid = Guid.NewGuid().ToByteArray();
            var binDate = BitConverter.GetBytes(DateTime.Now.Ticks);
            var secuentialGuid = new byte[uid.Length];
            secuentialGuid[0] = binDate[0];
            secuentialGuid[1] = binDate[1];
            secuentialGuid[2] = binDate[2];
            secuentialGuid[3] = binDate[3];

            secuentialGuid[4] = uid[0];
            secuentialGuid[5] = uid[1];
            secuentialGuid[6] = uid[2];
            secuentialGuid[7] = uid[3];
            secuentialGuid[8] = uid[4];
            secuentialGuid[9] = uid[5];
            secuentialGuid[10] = uid[6];
            secuentialGuid[11] = uid[7];

            secuentialGuid[12] = binDate[4];
            secuentialGuid[13] = binDate[5];
            secuentialGuid[14] = binDate[6];
            secuentialGuid[15] = binDate[7];

            return new Guid(secuentialGuid);
        }
    }
}