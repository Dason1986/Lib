using Library.ComponentModel.Model;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAggregateRoot : IEntity
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {

    }
}
