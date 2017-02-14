using Library;
using Library.ComponentModel.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPj.Test
{
    [TestFixture(Category = "枚舉")]
    public class StatusCodeTest
    {
        [Test,Category("枚舉"), Category("查詢")]
        public void TestStatus()
        {
            var state = StatusCode.Enabled | StatusCode.Disabled;
            StatusCode[] list = { StatusCode.Enabled, StatusCode.Disabled };

            var count = list.Where(n => (n & state) == n).Count();
            Assert.AreEqual(count, 2);
    
        }
    }
}