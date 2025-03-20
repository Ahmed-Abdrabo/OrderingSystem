using OrderingSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Core.Repositories.Contract
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IReadOnlyList<Order>> FindAsync(Expression<Func<Order, bool>> predicate);
    }
}
