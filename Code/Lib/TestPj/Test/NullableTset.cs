﻿using Library.ComponentModel.Test;
using NUnit.Framework;
using System;

namespace TestPj.Test
{
    [TestFixture]
    public class NullableTset
    {
        private int? _val1;

        [TestFixtureSetUp]
        public void Init()
        {
            _val1 = 1;
        }

        [Test]
        public void IsNull()
        {
            CodeTimer.Time("IsNull", ConstValue.Times99999, () => Todo(_val1 == null ? default(int) : _val1.Value));
        }

        [Test]
        public void Converter()
        {
            CodeTimer.Time("Converter", ConstValue.Times99999, () => Todo(Convert.ToInt32(_val1)));
        }

        private static void Todo(int value)
        {
        }
    }
}