using OrderingSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IReadOnlyList<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<IReadOnlyList<Order>> GetOrdersByCustomerIdAsync(string customerId);
        Task<bool> DeleteOrderAsync(int orderId, string customerId, bool isAdmin);
    }
}
