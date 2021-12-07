using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IRatingService
    {
        Task<ProductOutputDTO> RateAsync(string userIdStr, int rating, int productId);
        Task<bool> DeleteRatingAsync(string userIdStr, int productId);
    }
}
