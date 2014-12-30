using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPj
{

    class MyClass
    {
        public Status0? Statue { get; set; }
        public DateTime? ApplyDate { get; set; }


    }
    enum Status0 : int
    {

        None = 0,

        Invalid = 1,


        Saved = 2,


        Submited = 4,


        EndWithPass = 8,


        EndWithReject = 16,
    }
    internal class ItemComaper : IComparer<MyClass>
    {
        public static void CompareTset()
        {
            List<MyClass> list = new List<MyClass>
            {
                new MyClass {ApplyDate = DateTime.Parse("2014-07-05 10:20:30"), Statue = Status0.EndWithPass},
                new MyClass {ApplyDate = DateTime.Parse("2014-07-04 10:20:30"), Statue = Status0.Submited},
                new MyClass {ApplyDate = DateTime.Parse("2014-07-05 10:20:30"), Statue = Status0.EndWithReject},
                new MyClass {ApplyDate = DateTime.Parse("2014-08-05 10:20:30"), Statue = Status0.Saved},
                new MyClass {Statue = Status0.Saved},
                null,
                new MyClass {ApplyDate = DateTime.Parse("2014-10-05 10:20:30"), Statue = Status0.Submited},
                new MyClass {ApplyDate = DateTime.Parse("2014-07-06 10:20:30"), Statue = Status0.Saved},
                new MyClass {ApplyDate = DateTime.Parse("2014-09-07 10:20:30"), Statue = Status0.EndWithPass},
                new MyClass {ApplyDate = DateTime.Parse("2014-07-15 10:20:30"), Statue = Status0.EndWithReject},
                new MyClass {ApplyDate = DateTime.Parse("2014-07-25 10:20:30"), Statue = Status0.Saved},
                null
            };
            var array = list.OrderBy(n => n, ItemComaper.Comparer).ToArray();
            foreach (var myClass in array)
            {
                if (myClass != null) Console.WriteLine("Staue:{0}\tdate:{1}", myClass.Statue, myClass.ApplyDate);
                else Console.WriteLine("Staue:null\tdate:null");
            } Console.WriteLine(array);
        }

        public readonly static IComparer<MyClass> Comparer = new ItemComaper();

        private static int SameStateTimeAsc(MyClass x, MyClass y)
        {
            if (x.Statue != y.Statue) return -1;
            return Nullable.Compare(x.ApplyDate, y.ApplyDate);

        }

        public int Compare(MyClass x, MyClass y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            if (x.Statue == Status0.Saved || y.Statue == Status0.Saved)
            {
                if (y.Statue == null) return -1;
                if (x.Statue != y.Statue && y.Statue == Status0.Saved) return 1;
                if (x.Statue == y.Statue && y.Statue == Status0.Saved) return SameStateTimeAsc(x, y);
                return -1;

            }
            int xflag = (int)x.Statue.GetValueOrDefault();
            int yflag = (int)y.Statue.GetValueOrDefault();
            if (xflag > (int)Status0.Submited && yflag > (int)Status0.Submited) return Nullable.Compare(x.ApplyDate, y.ApplyDate);
            if (x.Statue != Status0.Submited && y.Statue != Status0.Submited) return Nullable.Compare(x.ApplyDate, y.ApplyDate);
            if (x.Statue == y.Statue && y.Statue == Status0.Submited) return SameStateTimeAsc(x, y);
            if (y.Statue == null) return -1;

            if (x.Statue == Status0.Submited && yflag > 2) return -1;
            if (y.Statue == Status0.Submited && xflag > 2) return 1;
            return Nullable.Compare(x.ApplyDate, y.ApplyDate);


        }
    }
}