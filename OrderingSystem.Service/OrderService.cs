using OrderingSystem.Core.Entities.Order;
using OrderingSystem.Core.Repositories.Contract;
using OrderingSystem.Core.Services.Contract;

namespace OrderingSystem.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public OrderService(IOrderRepository orderRepository, IGenericRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                    throw new Exception($"Product {item.ProductId} is unavailable or out of stock");

                product.Stock -= item.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = product.Name,  
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                orderItems.Add(orderItem);
                totalAmount += item.Quantity * product.Price;
            }

            order.OrderItems = orderItems;
            order.TotalAmount = totalAmount;
            order.OrderDate = DateTime.UtcNow;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return order;
        }

        public async Task<bool> DeleteOrderAsync(int orderId, string customerId, bool isAdmin)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, "OrderItems");
            if (order == null)
                return false;

            // Allow deletion only if the user is an admin or owns the order
            if (!isAdmin && order.CustomerId != customerId)
                return false;

            // Restore product stock
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                }
            }

            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync("OrderItems,Customer");
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId, "OrderItems,Customer");
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await _orderRepository.FindAsync(o => o.CustomerId == customerId);
        }
    }
}
