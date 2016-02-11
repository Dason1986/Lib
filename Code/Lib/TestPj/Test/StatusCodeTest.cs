using Library;
using Library.ComponentModel.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPj.Test
{
    [TestFixture]
    public class StatusCodeTest
    {
        [Test]
        public void TestStatus()
        {
            var state = StatusCode.Enabled | StatusCode.Disabled;
            StatusCode[] list = { StatusCode.Enabled, StatusCode.Disabled };

            var count = list.Where(n => (n & state) == n).Count();
            Assert.AreEqual(count, 2);
            StringBuilder builder = new StringBuilder();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < 9999; i++)
            {
                var str = IdentityGenerator.NewGuid().ToString();
                dic.Add(str, str);
                builder.AppendLine(str);
            }
            Console.WriteLine(builder.ToString());
        }
    }
}