using Library.HelperUtility;
using NUnit.Framework;
using System.Data;

namespace TestPj.Test
{
    [TestFixture(Category = "ADO")]
    public class AdoTset
    {
        public AdoTset()
        {
            dt = new DataTable();
            dt.Columns.Add("Account");
            for (int i = 0; i < count; i++)
            {
                var row = dt.NewRow();
                row[0] = "Name" + i;
                dt.Rows.Add(row);
            }
        }

        private readonly int count = 10;
        private readonly DataTable dt;

        [Test, Category("實體"), Category("数组")]
        public void CastTest()
        {
            var list = dt.GetList<ParsonModle>();
            Assert.AreEqual(count, list.Count);
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual("Name" + i, list[i].Account);
            }
        }

        [Test, Category("實體"), Category("数组")]
        public void CastToList()
        {
            var list = dt.GetList(typeof(ParsonModle));

            Assert.AreEqual(count, list.Count);
        }
    }
}