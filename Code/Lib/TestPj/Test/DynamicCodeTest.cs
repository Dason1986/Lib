using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DynamicCode;
using NUnit.Framework;
using Library.HelperUtility;

namespace TestPj.Test
{
    [TestFixture(Category = "Dynamic")]
    public class DynamicCodeTest
    {
        public DynamicCodeTest()
        {
            dt = new DataTable("mycalss");
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            dt.Rows.Add("123", 2);
            dt.Rows.Add("456", 7);
        }

        private readonly DataTable dt;

        [Test, Category("實體")]
        public void CastTestNotifyPropertyEntity()
        {
            GenerateNotifyPropertyAssembly generateNotify = new GenerateNotifyPropertyAssembly("test");

            generateNotify.Add(dt);
            var ass = generateNotify.Generate();
            Assert.IsNotNull(ass);
            var list = dt.GetList(ass.ExportedTypes.FirstOrDefault());
            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count);
        }
    }
}