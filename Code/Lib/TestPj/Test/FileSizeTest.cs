using Library;
using Library.HelperUtility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPj.Test
{
    [TestFixture(Category = "文件")]
    public class FileSizeTest
    {
        [Test]
        public void TestSize()
        {
            StringAssert.IsMatch(new ByteSize(20).ToString(), FileUtility.GetFileSizeDisplay(20));
            StringAssert.IsMatch(new KBSize(546464).ToString(), FileUtility.GetFileSizeDisplay(546464));
            StringAssert.IsMatch(new MBSize(546464 * 50).ToString(), FileUtility.GetFileSizeDisplay(546464 * 50));
            StringAssert.IsMatch(new GBSize(546464L * 5000).ToString(), FileUtility.GetFileSizeDisplay(546464L * 5000));
            StringAssert.IsMatch(new TBSize(546464L * 50000000).ToString(), FileUtility.GetFileSizeDisplay(546464L * 50000000));
            StringAssert.IsMatch(new PBSize(long.MaxValue).ToString(), FileUtility.GetFileSizeDisplay(long.MaxValue));

        }
    }
}
