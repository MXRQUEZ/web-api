using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Models;
using DAL.UserContext;
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

        public Dictionary<Platforms, string[]> GetTopPlatforms()
        {
            var pc = GetPlatformProducts(Platforms.PersonalComputer);
            var mobile = GetPlatformProducts(Platforms.Mobile);
            var ps = GetPlatformProducts(Platforms.PlayStation);
            var xbox = GetPlatformProducts(Platforms.Xbox);
            var nintendo = GetPlatformProducts(Platforms.Nintendo);

            var platforms = new Dictionary<Platforms, string[]>
            {
                {Platforms.PersonalComputer, pc},
                {Platforms.Mobile, mobile},
                {Platforms.PlayStation, ps},
                {Platforms.Xbox, xbox},
                {Platforms.Nintendo, nintendo}
            };

            return platforms
                .OrderByDescending(p => p.Value.Length)
                .Take(3)
                .ToDictionary(p => p.Key, p => p.Value);

            string[] GetPlatformProducts(Platforms platform)
            {
                return _db.Products
                    .Where(p => p.Platform == (int)platform)
                    .Select(p => p.Name)
                    .ToArray();
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            return _db.Products;
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            return await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
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
