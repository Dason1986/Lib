using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using NUnit.Framework;

namespace TestPj.tset
{
    public class RangeItemTest
    {
        class AppData : IRangeItem<DateTime>
        {
            public DateTime? ItemDate { get; set; }
            public DateTime? StartTime { get; set; }
            public DateTime? EndTime { get; set; }

            DateTime IRangeItem<DateTime>.Begin
            {
                get { return this.StartTime.GetValueOrDefault(); }
            }

            DateTime IRangeItem<DateTime>.End
            {
                get { return this.EndTime.GetValueOrDefault(); }
            }
        }
        readonly List<AppData> _list = new List<AppData>();
        readonly List<AppData> _erlist = new List<AppData>();


        [TestFixtureSetUp]
        public void Init()
        {
            //無相交
            _list.Add(new AppData { StartTime = new DateTime(2014, 10, 1, 1, 0, 0), EndTime = new DateTime(2014, 10, 1, 1, 59, 0) });
            _list.Add(new AppData { StartTime = new DateTime(2014, 10, 1, 0, 0, 0), EndTime = new DateTime(2014, 10, 1, 0, 59, 0) });
            _list.Add(new AppData { StartTime = new DateTime(2014, 10, 1, 2, 0, 0), EndTime = new DateTime(2014, 10, 1, 4, 0, 0) });
            _list.Add(new AppData { StartTime = new DateTime(2014, 10, 2, 1, 10, 0), EndTime = new DateTime(2014, 10, 2, 3, 10, 0) });
            _list.Add(new AppData { StartTime = new DateTime(2014, 10, 4, 1, 10, 0), EndTime = new DateTime(2014, 10, 4, 3, 10, 0) });
            _list.Add(new AppData { StartTime = new DateTime(2014, 10, 5, 1, 10, 0), EndTime = new DateTime(2014, 10, 5, 3, 10, 0) });

            //跨日相交
            _erlist.Add(new AppData { StartTime = new DateTime(2014, 9, 30, 22, 0, 0), EndTime = new DateTime(2014, 10, 1, 2, 0, 0) });
            _erlist.Add(new AppData { StartTime = new DateTime(2014, 10, 1, 0, 0, 0), EndTime = new DateTime(2014, 10, 1, 3, 59, 0) });
            //當天相交
            _erlist.Add(new AppData { StartTime = new DateTime(2014, 10, 1, 2, 10, 0), EndTime = new DateTime(2014, 10, 3, 10, 10, 0) });
            _erlist.Add(new AppData { StartTime = new DateTime(2014, 10, 3, 1, 10, 0), EndTime = new DateTime(2014, 10, 3, 3, 10, 0) });
            _erlist.Add(new AppData { StartTime = new DateTime(2014, 10, 4, 1, 10, 0), EndTime = new DateTime(2014, 10, 4, 3, 10, 0) });

        }

        [Test]
        public void Test1()
        {
            TestNone(_list,true);
            TestNone(_erlist,false);
        }
        private void TestNone(List<AppData> list,bool flag)
        {
            var wacth = Stopwatch.StartNew();
            wacth.Start();
            int arraycount = 0;
            int total = 0;
            for (int i = 1; i < list.Count; i++)
            {
                var noneArray = PermutationAndCombination<AppData>.GetCombination(list.ToArray(), i+1);
                arraycount = arraycount + noneArray.Count;
                foreach (var appDatase in noneArray)
                {
                    var noneArray2 = PermutationAndCombination<AppData>.GetPermutation(appDatase);
                    arraycount = arraycount + noneArray2.Count;
                    int itemcount = 0;
                    foreach (var item in noneArray2)
                    {
                        try
                        {
                            RangeItemHelper.ValidateNone<AppData, DateTime>(item);
                        }
                        catch (Exception)
                        {
                            itemcount++;
                        }
                    }
                    if (itemcount != 0) NUnit.Framework.Assert.AreEqual(noneArray2.Count,itemcount, "一但有錯，一批的排列數據都應該出錯");
                    total = total + itemcount;
                }
            }
            wacth.Stop();
            if (flag) NUnit.Framework.Assert.AreEqual(0, total);
            Console.WriteLine("{3} Use Time:{0},組合+排列數：{1},出現錯誤數：{2}", wacth.Elapsed, arraycount, total, flag ? "無相交" : "有相交");
        }


        
    }
}
