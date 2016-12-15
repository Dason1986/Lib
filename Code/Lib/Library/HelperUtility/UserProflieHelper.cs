using System;

namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    public static class UserProflieHelper
    {
        /// <summary>
        /// 根据传入出生日期字符串判断当前年龄
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>        
        public static int GetAge(DateTime birthday)
        {
            if (DateTime.Now < birthday) throw new Exception();
            int year = System.DateTime.Now.Year;

            return year - birthday.Year;
        }

       

       
    }
   
}
