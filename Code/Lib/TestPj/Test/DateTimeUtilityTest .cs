using Library.Date;
using Library.HelperUtility;
using Library.ComponentModel.Test;
using NUnit.Framework;
using System;

namespace TestPj.Test
{
    [TestFixture(Category = "日期")]
    public class DateTimeUtilityTest
    {


        [Test, Category("时间戳轉換")]
        public void ConverterTimeStamp()
        {
            var date = DateTime.Now;
            var timestamp = date.TimeStamp();
            var newdate = DateTimeUtility.FromTimeStamp(timestamp);
            var timpsp = date - newdate;
            Assert.AreEqual((int)timpsp.TotalSeconds,0);
    
        }


    }
}