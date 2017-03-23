using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Library.ComponentModel
{
    /// <summary>
    ///
    /// </summary>
    public interface IAlgorithm
    {
    }

    /// <summary>
    ///
    /// </summary>
    public interface IGreedAlgorithm : IAlgorithm
    {
        /// <summary>
        ///
        /// </summary>
        IGreedAlgorithmItem[] Products { get; set; }

        /// <summary>
        ///
        /// </summary>
        GainCollection HaveProducts { get; }
    }

    /// <summary>
    ///
    /// </summary>
    public enum GreedType
    {
        /// <summary>
        ///
        /// </summary>
        Price,

        /// <summary>
        ///
        /// </summary>
        Unit,
    }

    /// <summary>
    /// 獲得產品
    /// </summary>
    public interface IGainItem : IProduct
    {
        /// <summary>
        ///
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        ///
        /// </summary>
        int Quantity { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public struct GainItem : IGainItem
    {
        /// <summary>
        ///
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Quantity { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class GainCollection : Collection<GainItem>
    {
        /// <summary>
        /// 總值
        /// </summary>
        public decimal Total { get { return this.Sum(n => n.Price * n.Quantity); } }

        /// <summary>
        /// 總數量
        /// </summary>
        public int Quantity { get { return this.Sum(n => n.Quantity); } }
    }

    /// <summary>
    /// 產品限制數量
    /// </summary>
    public interface ILimitItem : IProduct
    {
        /// <summary>
        ///
        /// </summary>
        int Limit { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class LimitItem : ILimitItem
    {
        /// <summary>
        ///
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Limit { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IGreedAlgorithmByPrice : IGreedAlgorithm, IAlgorithm
    {
        /// <summary>
        /// 最大量（金額）
        /// </summary>
        decimal MaxAmount { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IGreedAlgorithmByUnits : IGreedAlgorithm, IAlgorithm
    {
        /// <summary>
        ///
        /// </summary>
        decimal MaxUnits { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IGreedMultipleAlgorithm : IGreedAlgorithm
    {
        /// <summary>
        /// 最多只能獲取多少個數量
        /// </summary>
        int? MaxQuantity { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class GreedMultipleAlgorithmByPrice : GreedMultipleAlgorithm, IGreedAlgorithmByPrice
    {
        /// <summary>
        ///
        /// </summary>
        public override void Invoke()
        {
            OnValid();

            var temp = from product in Products
                       join limitItem in Limits on product.Code equals limitItem.Code
                       orderby product.UnitPrice descending
                       select new Tuple<IGreedAlgorithmItem, ILimitItem>(product, limitItem);
            decimal currentAmount = MaxAmount;
            var list = new List<Tuple<IGreedAlgorithmItem, ILimitItem>>(temp);
            int quantity = 0;
            while (currentAmount > 0)
            {
                var product = list.FirstOrDefault(n => n.Item1.Price <= currentAmount);

                if (product == null) break;
                var item = product.Item1;

                var limit = product.Item2;
                var cont = (int)(currentAmount / item.Price);
                if (cont > limit.Limit) cont = limit.Limit;
                currentAmount = currentAmount - cont * item.Price;
                var newquantity = quantity + cont;
                if (MaxQuantity.HasValue && MaxQuantity.Value < newquantity)
                {
                    HaveProducts.Add(new GainItem { Code = item.Code, Quantity = cont + MaxQuantity.Value - newquantity, Price = item.Price });
                    break;
                }
                quantity = newquantity;
                HaveProducts.Add(new GainItem { Code = item.Code, Quantity = cont, Price = item.Price });

                list.Remove(product);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal MaxAmount { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class GreedMultipleAlgorithmByUnits : GreedMultipleAlgorithm, IGreedAlgorithmByUnits
    {
        /// <summary>
        ///
        /// </summary>
        public override void Invoke()
        {
            OnValid();

            var temp = from product in Products
                       join limitItem in Limits on product.Code equals limitItem.Code
                       orderby product.UnitPrice descending
                       select new Tuple<IGreedAlgorithmItem, ILimitItem>(product, limitItem);

            var list = new List<Tuple<IGreedAlgorithmItem, ILimitItem>>(temp);
            int quantity = 0;
            decimal currentUnits = MaxUnits;
            //   decimal currentAmount = 0;
            while (currentUnits > 0)
            {
                var product = list.FirstOrDefault(n => n.Item1.Units <= currentUnits);

                if (product == null) break;
                var willbreak = false;
                var item = product.Item1;

                var limit = product.Item2;
                var cont = (int)(currentUnits / item.Units);
                if (cont > limit.Limit) cont = limit.Limit;
                currentUnits = currentUnits - Math.Ceiling(cont * item.Units);
                var price = item.UnitPrice / item.Units;
                var newquantity = quantity + cont;
                if (MaxQuantity.HasValue && MaxQuantity.Value < newquantity)
                {
                    cont = cont + MaxQuantity.Value - newquantity;
                    willbreak = true;
                }
                //var newAmount = currentAmount + cont * price;
                //if (MaxAmount.HasValue && MaxAmount.Value < newAmount)
                //{
                //    willbreak = true;
                //    cont--;
                //}
                //currentAmount = newAmount;
                quantity = newquantity;
                HaveProducts.Add(new GainItem { Code = item.Code, Quantity = cont, Price = price });
                list.Remove(product);
                if (willbreak) break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal MaxUnits { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class GreedMultipleAlgorithm : GreedAlgorithm, IGreedMultipleAlgorithm, IAlgorithm
    {
        /// <summary>
        ///
        /// </summary>
        public ILimitItem[] Limits { get; set; }

        /// <summary>
        /// 最多只能獲取多少個數量
        /// </summary>
        public int? MaxQuantity { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected override void OnValid()
        {
            base.OnValid();
            if (Limits == null) throw new LogicException();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GreedAlgorithmByPrice : GreedAlgorithm, IGreedAlgorithmByPrice
    {
        /// <summary>
        ///
        /// </summary>
        public decimal MaxAmount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override void Invoke()
        {
            OnValid();

            decimal currentAmount = MaxAmount;
            var list = new List<IGreedAlgorithmItem>(Products.OrderByDescending(n => n.Price).ToArray());
            while (currentAmount > 0)
            {
                var item = list.FirstOrDefault(n => n.Price <= currentAmount);
                if (item == null) break;

                currentAmount = currentAmount - item.Price;
                HaveProducts.Add(new GainItem { Code = item.Code, Quantity = 1, Price = item.Price });

                list.Remove(item);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GreedAlgorithmByUnit : GreedAlgorithm, IGreedAlgorithmByUnits
    {
        /// <summary>
        ///
        /// </summary>
        public decimal MaxUnits { get; set; }

        public override void Invoke()
        {
            OnValid();

            decimal currentAmount = MaxUnits;
            var list = new List<IGreedAlgorithmItem>(Products.OrderByDescending(n => n.UnitPrice).ToArray());
            while (currentAmount > 0)
            {
                var item = list.FirstOrDefault(n => n.Units <= currentAmount);
                if (item == null) break;
                currentAmount = (int)(currentAmount - item.Units);
                HaveProducts.Add(new GainItem { Code = item.Code, Quantity = 1, Price = item.Price });

                list.Remove(item);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class GreedAlgorithm : IAlgorithm
    {
        /// <summary>
        ///
        /// </summary>
        public GreedAlgorithm()
        {
            HaveProducts = new GainCollection();
        }

        /// <summary>
        ///
        /// </summary>
        public IGreedAlgorithmItem[] Products { get; set; }

        /// <summary>
        ///
        /// </summary>
        public GainCollection HaveProducts { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public abstract void Invoke();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnValid()
        {
            if (Products == null || Products.Length == 0) throw new LogicException("");

            HaveProducts.Clear();
            foreach (var item in Products)
            {
                item.UnitPrice = item.Price / item.Units;
            }
        }
    }

    /// <summary>
    /// 產品
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// 唯一編號
        /// </summary>
        string Code { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IGreedAlgorithmItem : IProduct

    {
        /// <summary>
        /// 價值
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// 組成單位
        /// </summary>
        decimal Units { get; set; }

        /// <summary>
        /// 每1單位的價值
        /// </summary>
        decimal UnitPrice { get; set; }
    }
}