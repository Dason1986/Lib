using System;
using Library.Test;
using NUnit.Framework;

namespace TestPj.Test
{
    [TestFixture]
    public class StringTest
    {
        const string Sourcestr = "String String";
        const string Tragestr = "STRING string";

        [TestFixtureSetUp]
        public void Init()
        {
            Console.WriteLine("Char count:{0}", Sourcestr.Length);
        }
        [Test]
        public void FindLasdItemByIndex()
        {
            string uName = string.Empty;
            CodeTimer.Time("Index Find", ConstValue.Times99999, () =>
            {

                var index = Sourcestr.LastIndexOf(' ');
                if (index != -1)
                {
                    uName = Sourcestr.Substring(index + 1);
                }
            });
            Console.WriteLine(uName);
        }
        [Test]
        public void FindLasdItemBySpilt()
        {
            string uName = string.Empty;
            CodeTimer.Time("Spilt Find", ConstValue.Times99999, () =>
            {
                string[] data = Sourcestr.Split(' ');

                if (data.Length <= 1) return;
                uName = data[data.Length - 1];
            });
            Console.WriteLine(uName);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CompareByUpper()
        {

            Console.Write("Sourcestr.ToUpper() == Tragestr.ToUpper()  ");
            Console.WriteLine(Sourcestr.ToUpper() == Tragestr.ToUpper());

            CodeTimer.Time("ToUpper Compare", ConstValue.Times99999, () =>
            {
                var flag = Sourcestr.ToUpper() == Tragestr.ToUpper();
            });

        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CompareByEquals()
        {

            Console.Write("string.Equals(Sourcestr, Tragestr, StringComparison.OrdinalIgnoreCase))  ");
            Console.WriteLine(string.Equals(Sourcestr, Tragestr, StringComparison.OrdinalIgnoreCase));
            CodeTimer.Time("Equals Compare", ConstValue.Times99999, () =>
                  {
                      var flag = string.Equals(Sourcestr, Tragestr, StringComparison.OrdinalIgnoreCase);

                  });

        }
    }
}