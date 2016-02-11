using Library;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading;

namespace TestPj.Test
{
    [TestFixture]
    public class LanguageTest
    {
        [Test]
        public void TestCodeException()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            var ex = new CodeException(GlobalResource._Code11001_1, 11001.1);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("IDCard number format wong", ex.Message);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-pt");
            ex = new CodeException(GlobalResource._Code11001_1, 11001.1);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("IDcard formato de número wong", ex.Message);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-cn");
            ex = new CodeException(GlobalResource._Code11001_1, 11001.1);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("證件號碼格式不符合", ex.Message);

            ex = new CodeException(GlobalResource._Code11002_106, 11002.106);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("不支持類型轉換", ex.Message);

            ex = new CodeException(GlobalResource._Code11002_104, 11002.104, 10);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("不合法的農曆日期,[10月]不為闰月。", ex.Message);
        }

        [Test]
        public void TestCultureInfo()
        {
            string msg = ResourceManagement.GetString(typeof(GlobalResource), "Test", new CultureInfo("en-us"));
            Console.WriteLine(msg);
            StringAssert.AreEqualIgnoringCase("Test", msg, "en語言不正確");

            msg = ResourceManagement.GetString(typeof(GlobalResource), "Test", new CultureInfo("zh-cn"));
            Console.WriteLine(msg);
            StringAssert.AreEqualIgnoringCase("測試", msg, "zh語言不正確");

            msg = ResourceManagement.GetString(typeof(GlobalResource), "Test", new CultureInfo("pt-pt"));
            Console.WriteLine(msg);
            StringAssert.AreEqualIgnoringCase("teste", msg, "pt語言不正確");
        }
    }
}