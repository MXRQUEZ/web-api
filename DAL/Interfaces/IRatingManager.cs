using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models.Entities;

namespace DAL.Interfaces
{
    public interface IRatingManager
    {
        Task<ProductRating> FindByIdAsync(int productId, int userId);
        Task<IEnumerable<ProductRating>> GetByProductIdAsync(int productId);
        Task<bool> AnyAsync(int productId);
        Task<int> RecalculateRatingAsync(int productId);

        Task AddAndSaveAsync(ProductRating rating);
        Task UpdateAndSaveAsync(ProductRating rating);
        Task DeleteAndSaveAsync(ProductRating rating);
        void DeleteRange(IEnumerable<ProductRating> ratings);
    }
}