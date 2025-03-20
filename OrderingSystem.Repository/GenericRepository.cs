using Microsoft.EntityFrameworkCore;
using OrderingSystem.Core.Repositories.Contract;
using OrderingSystem.Repository.Data;
using System.Collections.Generic;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderingSystem.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
           

        }
        public async Task<IReadOnlyList<T>> GetAllAsync(string? includeProperties = null)
        {
            IQueryable<T> query = _dbContext.Set<T>(); 

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

       
        public async Task<T?> GetByIdAsync(int id, string? includeProperties = null)
        {
            IQueryable<T> query;

            query = _dbContext.Set<T>();
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }


        public async Task AddAsync(T entity)
            => await _dbContext.AddAsync(entity);
        
        public void Update(T entity)
            => _dbContext.Update(entity);

        public void Delete(T entity)
             =>  _dbContext.Remove(entity);

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
