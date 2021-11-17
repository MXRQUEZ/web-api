using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Dictionary<Platforms, string[]> GetTopPlatforms();
        IEnumerable<Product> GetProducts();
        Task<Product> FindByIdAsync(int id);
        Task<Product> AddAsync(Product newProduct);
        Task<Product> UpdateAsync(Product productUpdate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
