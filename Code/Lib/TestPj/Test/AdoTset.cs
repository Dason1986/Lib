using Library.HelperUtility;
using NUnit.Framework;
using System.Data;

namespace TestPj.Test
{
    [TestFixture(Category = "ADO")]
    public class AdoTset
    {
        [Test,Category("實體")]
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