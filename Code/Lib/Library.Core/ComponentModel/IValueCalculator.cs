using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.ComponentModel
{
    /// <summary>
    /// 计算产品价格
    /// </summary>
    public interface IValueCalculator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        decimal Calculator();
    }
}
