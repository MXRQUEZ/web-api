using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.Entities;

namespace DAL.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();
        IEnumerable<string> GetAllByPlatform(Platform platform);
        Task<Product> FindByIdAsync(int id);
        Task<Product> AddAsync(Product newProduct);
        Task<Product> UpdateAsync(Product productUpdate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
