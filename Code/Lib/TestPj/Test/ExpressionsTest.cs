using Library.HelperUtility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPj.Test
{
    [TestFixture(Category = "扩展")]
    [Category("扩展Expressions")]
    public class ExpressionsTest
    {
        [Test]
        public void GetAttribute()
        {
            var tt = new tt1();
            var atr = ExpressionHelper.GetAttribute<tt1, string, System.ComponentModel.DisplayNameAttribute>(t => t.Name);
            Assert.IsNotNull(atr);
            Assert.Greater(atr.Length, 0);

            atr = ExpressionHelper.GetAttribute<string, System.ComponentModel.DisplayNameAttribute>(() => tt.Name);

            Assert.IsNotNull(atr);
            Assert.Greater(atr.Length, 0);
        }
    
        class tt1
        {
            [System.ComponentModel.DisplayName("111")]
            public string Name { get; set; }
        }
    }
}
