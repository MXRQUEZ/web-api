using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IProductRepository<T> where T : class
    {
        IQueryable<Product> GetAllProducts();
        IEnumerable<string> GetProductsByPlatform(Platform platform);
        Task<Product> FindByIdAsync(int id);
        Task<Product> AddAsync(Product newProduct);
        Task<Product> UpdateAsync(Product productUpdate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
