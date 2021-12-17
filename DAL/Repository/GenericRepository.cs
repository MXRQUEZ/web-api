using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public abstract class GenericRepository<TEntity> : IDisposable where TEntity : class
    {
        protected readonly ApplicationDbContext DbContext;

        protected GenericRepository(ApplicationDbContext context) =>
            DbContext = context;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual IQueryable<TEntity> GetAll(bool trackChanges) =>
            trackChanges
                ? DbContext.Set<TEntity>()
                : DbContext.Set<TEntity>().AsNoTracking();

        public async Task AddAsync(TEntity entity) =>
            await DbContext.Set<TEntity>().AddAsync(entity);

        public async Task AddAndSaveAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAndSaveAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task SaveAsync() =>
            await DbContext.SaveChangesAsync();

        public void Update(TEntity entity) =>
            DbContext.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) =>
            DbContext.Set<TEntity>().Remove(entity);

        public void DeleteRange(IEnumerable<TEntity> entities) =>
            DbContext.Set<TEntity>().RemoveRange(entities);

        protected virtual void Dispose(bool disposing) =>
            DbContext.Dispose();
    }
}