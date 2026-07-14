using HRFlow.Common.Interfaces;
using HRFlow.Data.Context;
using HRFlow.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly HRFlowDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(HRFlowDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.CreatedDate = DateTime.UtcNow;
            }
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.UpdatedDate = DateTime.UtcNow;
            }
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = true;
                baseEntity.DeletedDate = DateTime.UtcNow;
            }

            _dbSet.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
