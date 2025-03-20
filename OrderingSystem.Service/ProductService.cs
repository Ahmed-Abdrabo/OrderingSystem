using OrderingSystem.Core.Entities;
using OrderingSystem.Core.Entities.Order;
using OrderingSystem.Core.Repositories.Contract;
using OrderingSystem.Core.Services.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystem.Service
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductService(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddAsync(product);
            await SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _productRepository.Update(product);
            await SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                _productRepository.Delete(product);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _productRepository.SaveChangesAsync();
        }
    }
}
