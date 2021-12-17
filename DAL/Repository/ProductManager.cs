using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.ApplicationContext;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public sealed class ProductManager : GenericRepository<Product>, IProductManager
    {
        public ProductManager(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Product> FindByIdAsync(int id) =>
            await DbContext.Set<Product>().FindAsync(id);

        public async Task<Product> FindByNameAsync(string productName) =>
            await DbContext.Set<Product>().AsNoTracking().FirstOrDefaultAsync(p => p.Name == productName);

        public async Task<int> CountByPlatformAsync(Platform platform)
            => await DbContext.Set<Product>().AsNoTracking().CountAsync(p => p.Platform == platform);

        public async Task<IEnumerable<Product>> GetByTermAsync(string term) =>
            await DbContext.Set<Product>()
                .Where(product => product.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetWhereAsync(Expression<Func<Product, bool>> predicate) =>
            await DbContext.Set<Product>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();

        public override IQueryable<Product> GetAll(bool trackChanges) =>
            trackChanges
                ? DbContext.Set<Product>().OrderBy(on => on.Name)
                : DbContext.Set<Product>().OrderBy(on => on.Name).AsNoTracking();
    }
}