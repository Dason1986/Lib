using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Specialized;
using Library;

namespace TestPj.Test
{
    [TestFixture(Category = "參數轉換")]
    public class RequestParamsConvertTest
    {
        NameValueCollection collection = new NameValueCollection() {
                 {"a1","10/2012" },
                 {"a2","02/10/2012" },

                 {"b1","A1" },
                 {"b2","A1,B2" },
                 {"b3","A1|B2" },

                 {"c1","123" },
                 {"c2","123.12" },
                 {"c3","-123.12" },
            };
        [Test(Description = "日期轉換")]
        public void ConverterDateTest()
        {

            RequestParamsConvert converter = new RequestParamsConvert(collection);
            var a1 = converter.GetDateTimeOrNull("a1", "MM/yyyy");
            Assert.IsNotNull(a1);
            Assert.AreEqual("10/2012", a1.Value.ToString("MM/yyyy"));

            var a2 = converter.GetDateTimeOrNull("a2", "dd/MM/yyyy");
            Assert.IsNotNull(a2);
            Assert.AreEqual("02/10/2012", a2.Value.ToString("dd/MM/yyyy"));

        }
        [Test(Description = "數值")]
        public void ConverterNumberTest()
        {

            RequestParamsConvert converter = new RequestParamsConvert(collection);

            var c1 = converter.GetValue<int>("c1");
            Assert.AreEqual(123, c1);
            var c2 = converter.GetValue<double>("c2");
            Assert.AreEqual(123.12d, c2);
            var c3 = converter.GetValue<double>("c3");
            Assert.AreEqual(-123.12d, c3);
        }

        [Test(Description = "枚舉")]
        public void ConverterEnumTest()
        {

            RequestParamsConvert converter = new RequestParamsConvert(collection);


            var b1 = converter.GetEnum<MyEnum>("b1");
            Assert.AreEqual(MyEnum.A1, b1);


            var b2 = converter.GetEnum<MyEnum>("b2");
            Assert.AreEqual(MyEnum.A1 | MyEnum.B2, b2);

            var b22 = converter.GetEnums<MyEnum>("b2");
            Assert.IsNotNull(b22);
            Assert.AreEqual(2, b22.Length);

            Assert.IsFalse(b22.Any(n => n == MyEnum.None));

            var b3 = converter.GetEnums<MyEnum>("b3");
            Assert.IsNotNull(b3);
            Assert.AreEqual(2, b3.Length);
            Assert.IsFalse(b3.Any(n => n == MyEnum.None));
        }
    }
    [Flags]
    internal enum MyEnum
    {
        None = 0,
        A1 = 1,
        B2 = 2,
        C3 = 4,
        D4 = 8
    }
}
