using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.ComponentModel;
using NUnit.Framework;

namespace TestPj.Test
{
    [TestFixture(Category = "算法")]
    [Category("貪心算法")]
    public class AlgorithmsTest
    {
        [Test]
        public void GreedAlgorithm()
        {
            var ff = new GreedAlgorithmByUnit();
            ff.Products = new IGreedAlgorithmItem[]
            {
                new GreedAlgorithmItem { Name = "物品1",Price = 60,Units = 10},
                new GreedAlgorithmItem { Name = "物品2",Price = 100,Units = 20},
                new GreedAlgorithmItem { Name = "物品3",Price = 120,Units = 30},
            };

            ff.MaxUnits = 30;
            ff.Invoke();
            Assert.AreEqual(2, ff.HaveProducts.Quantity);
            Assert.AreEqual(2, ff.HaveProducts.Count);
            Assert.AreEqual(160m, ff.HaveProducts.Total);
        }

        [Test]
        public void GreedAlgorithm2()
        {
            var ff = new GreedMultipleAlgorithmByPrice();
            ff.Products = new IGreedAlgorithmItem[]
            {
                new GreedAlgorithmItem { Name = "1角",Price = 0.1m,Units = 1},
                new GreedAlgorithmItem { Name = "5角",Price = 0.5m,Units = 1},
                new GreedAlgorithmItem { Name = "1元",Price = 1,Units = 1},
                new GreedAlgorithmItem { Name = "5元",Price = 5,Units = 1},
                new GreedAlgorithmItem { Name = "10元",Price = 10,Units = 1},
                new GreedAlgorithmItem { Name = "20元",Price = 20,Units = 1},
                new GreedAlgorithmItem { Name = "50元",Price = 50,Units = 1},
                new GreedAlgorithmItem { Name = "100元",Price = 20,Units = 1},
            };
            ff.Limits = new ILimitItem[]
            {
                        new LimitItem {Code = "1角", Limit = 4},
                        new LimitItem {Code = "1元", Limit = 6},
                        new LimitItem {Code = "5元", Limit = 2},
                        new LimitItem {Code = "10元", Limit = 3},
            };
            ff.MaxAmount = 50m;
            ff.Invoke();
            Assert.AreEqual(4, ff.HaveProducts.Count);
            Assert.AreEqual(15, ff.HaveProducts.Quantity);
            Assert.AreEqual(46.4m, ff.HaveProducts.Total);
        }

        [Test]
        public void GreedAlgorithm3()
        {
            var ff = new GreedMultipleAlgorithmByPrice();
            ff.Products = new IGreedAlgorithmItem[]
            {
                new GreedAlgorithmItem { Name = "1角",Price = 0.1m,Units = 1},
                new GreedAlgorithmItem { Name = "5角",Price = 0.5m,Units = 1},
                new GreedAlgorithmItem { Name = "1元",Price = 1,Units = 1},
                new GreedAlgorithmItem { Name = "5元",Price = 5,Units = 1},
                new GreedAlgorithmItem { Name = "10元",Price = 10,Units = 1},
                new GreedAlgorithmItem { Name = "20元",Price = 20,Units = 1},
                new GreedAlgorithmItem { Name = "50元",Price = 50,Units = 1},
                new GreedAlgorithmItem { Name = "100元",Price = 20,Units = 1},
            };
            ff.Limits = new ILimitItem[]
            {
                        new LimitItem {Code = "1角", Limit = 4},
                        new LimitItem {Code = "5角", Limit = 10},
                        new LimitItem {Code = "1元", Limit = 6},
                        new LimitItem {Code = "5元", Limit = 2},
                        new LimitItem {Code = "10元", Limit = 3},
                        new LimitItem {Code = "20元", Limit = 1},
            };
            ff.MaxAmount = 51.2m;
            ff.Invoke();
            Assert.AreEqual(4, ff.HaveProducts.Count);
            Assert.AreEqual(7, ff.HaveProducts.Quantity);
            Assert.AreEqual(51.2m, ff.HaveProducts.Total);
        }

        [Test]
        public void GreedAlgorithm4()
        {
            var ff = new GreedMultipleAlgorithmByPrice();
            ff.Products = new IGreedAlgorithmItem[]
            {
                new GreedAlgorithmItem { Name = "1角",Price = 0.1m,Units = 1},
                new GreedAlgorithmItem { Name = "5角",Price = 0.5m,Units = 1},
                new GreedAlgorithmItem { Name = "1元",Price = 1,Units = 1},
                new GreedAlgorithmItem { Name = "5元",Price = 5,Units = 1},
                new GreedAlgorithmItem { Name = "10元",Price = 10,Units = 1},
                new GreedAlgorithmItem { Name = "20元",Price = 20,Units = 1},
                new GreedAlgorithmItem { Name = "50元",Price = 50,Units = 1},
                new GreedAlgorithmItem { Name = "100元",Price = 20,Units = 1},
            };
            ff.Limits = new ILimitItem[]
            {
                        new LimitItem {Code = "1角", Limit = 4},
                        new LimitItem {Code = "5角", Limit = 10},
                        new LimitItem {Code = "1元", Limit = 6},
                        new LimitItem {Code = "5元", Limit = 2},
                        new LimitItem {Code = "10元", Limit = 3},
            };
            ff.MaxAmount = 51.2m;
            ff.MaxQuantity = 10;
            ff.Invoke();
            Assert.AreEqual(3, ff.HaveProducts.Count);
            Assert.AreEqual(10, ff.HaveProducts.Quantity);
            Assert.AreEqual(45m, ff.HaveProducts.Total);
        }

        [Test]
        public void GreedAlgorithm5()
        {
            var ff = new GreedMultipleAlgorithmByUnits();
            ff.Products = new IGreedAlgorithmItem[]
            {
                new GreedAlgorithmItem { Name = "1角",Price = 0.1m,Units = 1},
                new GreedAlgorithmItem { Name = "5角",Price = 0.5m,Units = 1},
                new GreedAlgorithmItem { Name = "1元",Price = 1,Units = 1},
                new GreedAlgorithmItem { Name = "5元",Price = 5,Units = 1},
                new GreedAlgorithmItem { Name = "10元",Price = 10,Units = 1},
                new GreedAlgorithmItem { Name = "20元",Price = 20,Units = 1},
                new GreedAlgorithmItem { Name = "50元",Price = 50,Units = 1},
                new GreedAlgorithmItem { Name = "100元",Price = 20,Units = 1},
            };
            ff.Limits = new ILimitItem[]
            {
                        new LimitItem {Code = "1角", Limit = 4},
                        new LimitItem {Code = "5角", Limit = 10},
                        new LimitItem {Code = "1元", Limit = 6},
                        new LimitItem {Code = "5元", Limit = 2},
                        new LimitItem {Code = "10元", Limit = 3},
                        new LimitItem {Code = "20元", Limit = 1},
            };

            ff.MaxUnits = 7m;
            ff.Invoke();
            Assert.AreEqual(4, ff.HaveProducts.Count);
            Assert.AreEqual(7, ff.HaveProducts.Quantity);
            Assert.AreEqual(61m, ff.HaveProducts.Total);
        }

        [Test]
        public void GreedAlgorithm6()
        {
            var ff = new GreedMultipleAlgorithmByUnits();
            ff.Products = new IGreedAlgorithmItem[]
            {
                new GreedAlgorithmItem { Name = "1角",Price = 0.1m,Units = 1},
                new GreedAlgorithmItem { Name = "5角",Price = 0.5m,Units = 1},
                new GreedAlgorithmItem { Name = "1元",Price = 1,Units = 1},
                new GreedAlgorithmItem { Name = "5元",Price = 5,Units = 1},
                new GreedAlgorithmItem { Name = "5美元",Price = 5,Units = 0.68m},
                new GreedAlgorithmItem { Name = "10元",Price = 10,Units = 1},
                new GreedAlgorithmItem { Name = "20美元",Price = 20,Units = 0.68m},
                new GreedAlgorithmItem { Name = "20元",Price = 20,Units = 1},
                new GreedAlgorithmItem { Name = "50元",Price = 50,Units = 1},
                new GreedAlgorithmItem { Name = "100元",Price = 20,Units = 1},
            };
            ff.Limits = new ILimitItem[]
            {
                        new LimitItem {Code = "1角", Limit = 4},
                        new LimitItem {Code = "5角", Limit = 10},
                        new LimitItem {Code = "1元", Limit = 6},
                        new LimitItem {Code = "5元", Limit = 2},
                        new LimitItem {Code = "5美元", Limit = 2},
                        new LimitItem {Code = "10元", Limit = 3},
                        new LimitItem {Code = "20美元", Limit =2},
            };

            //   ff.MaxAmount = 100m;
            ff.MaxUnits = 10;
            ff.Invoke();
            Assert.AreEqual(5, ff.HaveProducts.Count);
            Assert.AreEqual(10, ff.HaveProducts.Quantity);
            Assert.AreEqual(149.13m, Math.Round(ff.HaveProducts.Total, 2));
        }

        private class GreedAlgorithmItem : IGreedAlgorithmItem
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal Units { get; set; }
            public decimal UnitPrice { get; set; }

            string IProduct.Code
            {
                get { return Name; }
                set { Name = value; }
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}