using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Entities;
using DAL.UserContext;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public sealed class ProductRepository : IProductRepository, IDisposable
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public IEnumerable<string> GetAllByPlatform(Platform platform)
        {
            return _db.Products
                    .Where(p => p.Platform == platform)
                    .Select(p => p.Name)
                    .AsNoTracking();
        }

        public IQueryable<Product> GetAll()
        {
            return _db.Products
                .Include(r => r.Ratings)
                .OrderBy(on => on.Name)
                .AsNoTracking();
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            return await _db.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> AddAsync(Product newProduct)
        {
            await _db.Products.AddAsync(newProduct);
            await _db.SaveChangesAsync();
            return newProduct;
        }

        public async Task<Product> UpdateAsync(Product productUpdate)
        {
            _db.Products.Update(productUpdate);
            await _db.SaveChangesAsync();
            return productUpdate;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return false;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
