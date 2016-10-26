using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ValueObject<T> :IEqualityComparer<T> 
    {
      

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public abstract bool Equals(T x, T y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract int GetHashCode(T obj);
        /*
public static bool operator !=(T money1, T money2)
{
   if ((money1.Value != money2.Value) &&
       (money1.CurrencyType != money2.CurrencyType))
   {
       return true;
   }
   return false;
}
public static bool operator ==(T money1, T money2)
{
   if ((money1.Value == money2.Value) &&
       (money1.CurrencyType == money1.CurrencyType))
   {
       return true;
   }
   return false;
}
*/
    }
}
