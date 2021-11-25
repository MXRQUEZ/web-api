using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(bool trackChanges);
        Task AddAsync(T newItem);
        Task AddAndSaveAsync(T newItem);
        Task UpdateAndSaveAsync(T itemUpdate);
        Task DeleteAndSaveAsync(T item);
        void Update(T itemUpdate);
        void Delete(T item);
        void DeleteRange(IEnumerable<T> items);
        Task SaveAsync();
    }
}
