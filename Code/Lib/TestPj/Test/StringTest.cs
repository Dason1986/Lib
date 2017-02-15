using Library.Date;
using Library.HelperUtility;
using Library.ComponentModel.Test;
using NUnit.Framework;
using System;

namespace TestPj.Test
{
    [TestFixture(Category = "字符")]
    public class StringTest
    {
        private const string Sourcestr = "String String";
        private const string Tragestr = "STRING string";

        [TestFixtureSetUp]
        public void Init()
        {
            Console.WriteLine("Char count:{0}", Sourcestr.Length);
        }

        [Test, Category("查詢")]
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

        [Test, Category("轉換")]
        public void TsetCharToInt()
        {
            foreach (var i in "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^")
            {
                Console.WriteLine("{0} {1}", i, StringUtility.A_ZToNumber(i.ToString()));
                ;
            }
        }

        [Test, Category("拆分")]
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
        [Test, Category("比較")]
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
        [Test, Category("比較")]
        public void CompareByEquals()
        {
            Console.Write("string.Equals(Sourcestr, Tragestr, StringComparison.OrdinalIgnoreCase))  ");
            Console.WriteLine(string.Equals(Sourcestr, Tragestr, StringComparison.OrdinalIgnoreCase));
            CodeTimer.Time("Equals Compare", ConstValue.Times99999, () =>
                  {
                      var flag = string.Equals(Sourcestr, Tragestr, StringComparison.OrdinalIgnoreCase);
                  });
        }

        [Test, Category("格式化")]
        public void DateFormatText()
        {
            var now = DateTime.Now;
            var few = now.AddMinutes(-5);
            Console.WriteLine("{0} {1} {2}", few, few.GetPeriod(), few.FormatPeriodText());
            for (int i = 1; i <= 365; i++)
            {
                var time = now.AddDays(-i);
                Console.WriteLine("{0} {1} {2}", time, time.GetPeriod(), time.FormatPeriodText());
                time = now.AddDays(+i);
                Console.WriteLine("{0} {1} {2}", time, time.GetPeriod(), time.FormatPeriodText());
            }
        }

        [Test, Category("格式化")]
        public void GetDateFormatText()
        {
            var periods = Enum.GetValues(typeof(DateTimePeriod));
            foreach (DateTimePeriod period in periods)
            {
                Console.WriteLine("{0}:{1}", period, period.GetDateTimeRange());
            }
        }
    }
}