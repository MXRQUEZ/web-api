using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IRatingService
    {
        Task<ProductOutputDTO> RateAsync(string userId, int rating, int productId);
        Task DeleteRatingAsync(string userId, int productId);
    }
}
