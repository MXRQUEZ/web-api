using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.Entities;

namespace DAL.Interfaces
{
    public interface IProductManager
    {
        Task<Product> FindByIdAsync(int id);
        Task<Product> FindByNameAsync(string productName);
        Task<IEnumerable<Product>> GetByTermAsync(string term);
        Task<IEnumerable<Product>> GetWhereAsync(Expression<Func<Product, bool>> predicate);
        Task<int> CountByPlatformAsync(Platform platform);

        Task AddAndSaveAsync(Product product);
        Task UpdateAndSaveAsync(Product product);
        void Update(Product product);
        Task DeleteAndSaveAsync(Product product);
    }
}