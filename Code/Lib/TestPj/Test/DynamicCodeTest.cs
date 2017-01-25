using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DynamicCode;
using NUnit.Framework;

namespace TestPj.Test
{
    [TestFixture]
    public class DynamicCodeTest
    {
        [Test]
        public void CastTestNotifyPropertyEntity()
        {
            GenerateNotifyPropertyAssembly generateNotify = new GenerateNotifyPropertyAssembly("test");
            var dt = new DataTable("mycalss");
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            generateNotify.Add(dt);
            var ass = generateNotify.Generate();
            Assert.IsNotNull(ass);
        }
    }
}