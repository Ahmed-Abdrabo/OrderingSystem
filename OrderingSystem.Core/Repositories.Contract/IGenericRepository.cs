using System;
using System.Collections.Generic;

namespace OrderingSystem.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, string? includeProperties = null);
        Task<IReadOnlyList<T>> GetAllAsync(string? includeProperties = null);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}
