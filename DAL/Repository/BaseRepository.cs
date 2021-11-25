using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.ApplicationContext;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class BaseRepository<T> : IDisposable, IRepository<T> where T: class
    {
        private readonly ApplicationDbContext _db;

        public BaseRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public IQueryable<T> GetAll(bool trackChanges)
        {
            return trackChanges
                ? _db.Set<T>()
                : _db.Set<T>().AsNoTracking();
        }

        public async Task AddAsync(T newItem)
        {
            await _db.Set<T>().AddAsync(newItem);
        }

        public async Task AddAndSaveAsync(T newItem)
        {
            await _db.Set<T>().AddAsync(newItem);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync(T itemUpdate)
        {
            _db.Set<T>().Update(itemUpdate);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAndSaveAsync(T item)
        {
            _db.Set<T>().Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Update(T itemUpdate)
        {
            _db.Set<T>().Update(itemUpdate);
        }

        public void Delete(T item)
        {
            _db.Set<T>().Remove(item);
        }

        public void DeleteRange(IEnumerable<T> items)
        {
            _db.Set<T>().RemoveRange(items);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
