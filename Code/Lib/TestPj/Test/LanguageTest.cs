using System;
using System.Globalization;
using System.Threading;
using Library;
using Library.Att;
using Library.Date;
using NUnit.Framework;

namespace TestPj.Test
{
    [TestFixture]
    public class LanguageTest
    {
        [Test]
        public void TestCodeException()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            var ex = new CodeException(11001.1);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("IDCard number format wong", ex.Message);

            ex = new CodeException();
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("Unknown", ex.Message);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-pt");
            ex = new CodeException(11001.1);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("IDcard formato de número wong", ex.Message);


            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-cn");
            ex = new CodeException(11001.1);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("證件號碼格式不符合", ex.Message);

            ex = new CodeException(11002.106);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("不支持類型轉換", ex.Message);

             
            ex = new ChineseDateTimeException(11002.104, 10);
            Console.WriteLine(ex);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase("不合法的農曆日期,[10月]不為闰月。", ex.Message);
        }
        [Test]
        public void TestCultureInfo()
        {
            string msg = LanguageResourceManagement.GetString("Test", "Global", new CultureInfo("en-us"));
            Console.WriteLine(msg);
            StringAssert.AreEqualIgnoringCase("Test", msg, "en語言不正確");


            msg = LanguageResourceManagement.GetString("Test", "Global", new CultureInfo("zh-cn"));
            Console.WriteLine(msg);
            StringAssert.AreEqualIgnoringCase("測試", msg, "zh語言不正確");


            msg = LanguageResourceManagement.GetString("Test", "Global", new CultureInfo("pt-pt"));
            Console.WriteLine(msg);
            StringAssert.AreEqualIgnoringCase("teste", msg, "pt語言不正確");

        }
    }
}
