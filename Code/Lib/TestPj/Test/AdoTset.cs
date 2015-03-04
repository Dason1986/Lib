using System.Data;
using Library.HelperUtility;
using NUnit.Framework;

namespace TestPj.Test
{
    [TestFixture]
    public class AdoTset
    {
        [Test]
        public void CastTest()
        {
            int count = 10;
            DataTable dt = new DataTable();
            dt.Columns.Add("Account");
            for (int i = 0; i < count; i++)
            {
                var row = dt.NewRow();
                row[0] = "Name" + i;
                dt.Rows.Add(row);
            }
            var list = dt.GetList<ParsonModle>();
            Assert.AreEqual(count, list.Count);
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual("Name" + i, list[i].Account);
            }

        }
    }
}