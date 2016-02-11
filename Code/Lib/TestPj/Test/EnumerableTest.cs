using Library.HelperUtility;
using Library.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPj.Test
{
    [TestFixture]
    public class EnumerableTest
    {
        private readonly List<ParsonModle> _list = new List<ParsonModle>();

        [OneTimeSetUpAttribute]
        public void Init()
        {
            for (int i = 0; i < ConstValue.Times99999; i++)
            {
                _list.Add(new ParsonModle() { Account = "a" + i, Age = 10, UserName = "us" + i, Proxy = "Y" });
            }
        }

        [Test]
        public void IsEmpty()
        {
            CodeTimer.Time("IsEmpty .net linq", ConstValue.Times99999, () =>
            {
                var flg = _list != null && _list.Any();
            });
            CodeTimer.Time("IsEmpty .net count", ConstValue.Times99999, () =>
            {
                var flg = _list.IsEmpty();
            });
            var enumerable = _list.ToArray();
            CodeTimer.Time("IsEmpty .net IEnumerable", ConstValue.Times99999, () =>
            {
                var flg = enumerable.IsEmpty();
            });

            CodeTimer.Time(" HasRecord", ConstValue.Times99999, () =>
            {
                var flg = enumerable.HasRecord(10);
            });

            CodeTimer.Time("linq count ", ConstValue.Times99999, () =>
            {
                var flg = enumerable.Count() > 10;
            });
        }

        [Test]
        public void FilterByGlobal()
        {
            var loginusername = GetLoginUserName();
            CodeTimer.Time("Global Filter", ConstValue.Times99999, () =>
            {
                var data = from ea in _list
                           where string.Equals(ea.UserName, loginusername, StringComparison.OrdinalIgnoreCase)
                                 && ea.Proxy == "Y"
                           select ea;
            });
        }

        [Test]
        public void FilterByLocality()
        {
            CodeTimer.Time("Locality Filter", ConstValue.Times99999, () =>
            {
                var data = from ea in _list
                           where string.Equals(ea.UserName, GetLoginUserName(), StringComparison.OrdinalIgnoreCase)
                                 && ea.Proxy == "Y"
                           select ea;
                //foreach (var modle in data)
                //{
                //    if (modle.Account != null)
                //    {
                //        modle.UserName = modle.Account;
                //    }
                //}
            });
        }

        [Test]
        public void FilterByNotLinq()
        {
            var loginusername = GetLoginUserName();
            CodeTimer.Time("NotLinq Filter", 10, () =>
            {
                foreach (var ea in _list)
                {
                    if (string.Equals(ea.UserName, loginusername, StringComparison.OrdinalIgnoreCase) &&
                        ea.Proxy == "Y")
                    {
                        //if (ea.Account != null)
                        //{
                        //    ea.UserName = ea.Account;
                        //}
                        //    continue;
                    }
                    //todo
                }
            });
        }

        private static string GetLoginUserName()
        {
            //  System.Threading.Thread.Sleep(500);
            return "us599";
        }
    }
}