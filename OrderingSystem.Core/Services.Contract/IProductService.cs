
using OrderingSystem.Core.Entities.Order;

namespace OrderingSystem.Core.Services.Contract
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task SaveChangesAsync();

    }
}
