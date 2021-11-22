using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> AddAsync(T newProduct);
        Task<T> UpdateAsync(T productUpdate);
        Task DeleteAsync(T product);
    }
}
