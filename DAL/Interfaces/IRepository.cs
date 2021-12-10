using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(bool trackChanges);
        Task AddAsync(TEntity newItem);
        Task AddAndSaveAsync(TEntity newItem);
        Task UpdateAndSaveAsync(TEntity itemUpdate);
        Task DeleteAndSaveAsync(TEntity item);
        void Update(TEntity itemUpdate);
        void Delete(TEntity item);
        void DeleteRange(IEnumerable<TEntity> items);
        Task SaveAsync();
    }
}
