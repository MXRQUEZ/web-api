using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.ApplicationContext;
using DAL.Interfaces;
using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public sealed class ProductRepository : IRepository<Product>, IDisposable
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public IQueryable<Product> GetAll()
        {
            return _db.Products
                .OrderBy(on => on.Name)
                .AsNoTracking();
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

        public async Task DeleteAsync(Product product)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
