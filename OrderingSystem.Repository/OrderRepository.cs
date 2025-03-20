using Microsoft.EntityFrameworkCore;
using OrderingSystem.Core.Entities.Order;
using OrderingSystem.Core.Repositories.Contract;
using OrderingSystem.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Repository
{
    public class OrderRepository :  GenericRepository<Order> , IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<Order>> FindAsync(Expression<Func<Order, bool>> predicate)
        {
            return await _dbContext.Set<Order>().Where(predicate).Include(oi=>oi.OrderItems).ToListAsync();
        }
    }
}
