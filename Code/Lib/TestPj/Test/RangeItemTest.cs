using Library;
using Library.Comparable;
using Library.HelperUtility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TestPj.Test
{
    public class RangeItemTest
    {
        private class AppData : IRangeItem<DateTime>
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

            public bool IsInside(DateTime x)
            {
                return true;
            }
        }

        private readonly List<AppData> _list = new List<AppData>();
        private readonly List<AppData> _erlist = new List<AppData>();

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
        public void TestValidateNone()
        {
            TestNone(_list, true);
            TestNone(_erlist, false);
        }

        [Test]
        public void TestMergeRange()
        {
            int count = 0;
            List<AppData> appDatas = new List<AppData>(_list);

            var wacth = Stopwatch.StartNew();
            wacth.Start();

            appDatas.AddRange(_erlist);
            for (int i = 1; i < appDatas.Count; i++)
            {
                var noneArray = PermutationAndCombination<AppData>.GetCombination(appDatas.ToArray(), i + 1);

                foreach (var none in noneArray)
                {
                    count++;
                    var items = RangeItemHelper.Merge<AppData, DateTime>(none);
                    foreach (var mergeRange in items)
                    {
                        Assert.AreEqual(mergeRange.Begin, mergeRange.List.Min(n => n.Begin), "合併失敗，開始值不為數組中的最小值");
                        Assert.AreEqual(mergeRange.End, mergeRange.List.Max(n => n.End), "合併失敗，結束值不為數組中的最大值");
                    }

                    Assert.AreEqual(none.Length, items.Sum(n => n.List.Length), "合併失敗，合併後的數據不齊全");
                }
            }
            wacth.Stop();
            Console.WriteLine(" Use Time:{0}  run count:{1}", wacth.Elapsed, count);
        }

        private void TestNone(List<AppData> list, bool flag)
        {
            var wacth = Stopwatch.StartNew();
            wacth.Start();
            int arraycount = 0;
            int total = 0;
            for (int i = 1; i < list.Count; i++)
            {
                var noneArray = PermutationAndCombination<AppData>.GetCombination(list.ToArray(), i + 1);
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
                    if (itemcount != 0) Assert.AreEqual(noneArray2.Count, itemcount, "一但有錯，一批的排列數據都應該出錯");
                    total = total + itemcount;
                }
            }
            wacth.Stop();
            if (flag) Assert.AreEqual(0, total);
            Console.WriteLine("{3} Use Time:{0},組合+排列數：{1},出現錯誤數：{2}", wacth.Elapsed, arraycount, total, flag ? "無相交" : "有相交");
        }
    }
}