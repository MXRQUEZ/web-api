using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.ApplicationContext;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class BaseRepository<TEntity> : IDisposable, IRepository<TEntity> where TEntity: class
    {
        private readonly ApplicationDbContext _db;

        public BaseRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public IQueryable<TEntity> GetAll(bool trackChanges)
        {
            return trackChanges
                ? _db.Set<TEntity>()
                : _db.Set<TEntity>().AsNoTracking();
        }

        public async Task AddAsync(TEntity newItem)
        {
            await _db.Set<TEntity>().AddAsync(newItem);
        }

        public async Task AddAndSaveAsync(TEntity newItem)
        {
            await _db.Set<TEntity>().AddAsync(newItem);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync(TEntity itemUpdate)
        {
            _db.Set<TEntity>().Update(itemUpdate);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAndSaveAsync(TEntity item)
        {
            _db.Set<TEntity>().Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Update(TEntity itemUpdate)
        {
            _db.Set<TEntity>().Update(itemUpdate);
        }

        public void Delete(TEntity item)
        {
            _db.Set<TEntity>().Remove(item);
        }

        public void DeleteRange(IEnumerable<TEntity> items)
        {
            _db.Set<TEntity>().RemoveRange(items);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
