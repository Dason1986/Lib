using System;
using System.Globalization;
using System.Threading;
using Library;
using Library.Att;
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
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-cn");
            ex = new CodeException(11001.1);
            Console.WriteLine(ex);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-pt");
            ex = new CodeException(11001.1);
            Console.WriteLine(ex);
        }
         [Test]
        public void TestCultureInfo()
        {

            Console.WriteLine(LanguageResourceManagement.GetString("Global", "Test", new CultureInfo("en-us")));
            Console.WriteLine(LanguageResourceManagement.GetString("Global", "Test", new CultureInfo("zh-cn")));
            Console.WriteLine(LanguageResourceManagement.GetString("Global", "Test", new CultureInfo("pt-pt")));
        }
    }
}
