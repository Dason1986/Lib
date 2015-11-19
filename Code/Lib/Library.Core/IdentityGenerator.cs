using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Library
{

    /// <summary>
    ///
    /// </summary>
    public static class IdentityGenerator
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static DateTime GetGuidDateTime(Guid gid)
        {
            var buffter = gid.ToByteArray();
            byte[] binDate = new byte[8];
            binDate[0] = buffter[0];
            binDate[1] = buffter[1];
            binDate[2] = buffter[2];
            binDate[3] = buffter[3];
            binDate[4] = buffter[4];
            binDate[5] = buffter[5];
            binDate[6] = buffter[6];
            binDate[7] = buffter[7];
            var longtime = BitConverter.ToInt64(binDate, 0);
            var time = DateTime.FromBinary(longtime);
            return time;
        }

        /// <summary>
        /// 該算法生成跨系統邊界secuential的GUID，非常適合數據庫
        /// </summary>
        /// <returns></returns>
        public static Guid NewSequentialGuid()
        {
            var uid = Guid.NewGuid().ToByteArray();
            var binDate = BitConverter.GetBytes(DateTime.Now.Ticks);
            var secuentialGuid = new byte[uid.Length];
            secuentialGuid[0] = binDate[0];
            secuentialGuid[1] = binDate[1];
            secuentialGuid[2] = binDate[2];
            secuentialGuid[3] = binDate[3];
            secuentialGuid[4] = binDate[4];
            secuentialGuid[5] = binDate[5];
            secuentialGuid[6] = binDate[6];
            secuentialGuid[7] = binDate[7];


            secuentialGuid[8] = uid[0];
            secuentialGuid[9] = uid[1];
            secuentialGuid[10] = uid[2];
            secuentialGuid[11] = uid[3];
            secuentialGuid[12] = uid[4];
            secuentialGuid[13] = uid[5];
            secuentialGuid[14] = uid[6];
            secuentialGuid[15] = uid[7];
            return new Guid(secuentialGuid);

        }

    }

}

