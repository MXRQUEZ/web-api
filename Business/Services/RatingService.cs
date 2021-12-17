using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DAL.Interfaces;
using DAL.Models.Entities;

namespace Business.Services
{
    public sealed class RatingService : IRatingService
    {
        private readonly IMapper _mapper;
        private readonly IProductManager _productManager;
        private readonly IRatingManager _ratingManager;

        public RatingService(IRatingManager ratingManager, IProductManager productManager, IMapper mapper)
        {
            _ratingManager = ratingManager;
            _productManager = productManager;
            _mapper = mapper;
        }

        public async Task<ProductOutputDTO> RateAsync(string userIdStr, int rating, int productId)
        {
            var product = await _productManager.FindByIdAsync(productId);
            if (product is null || rating is > 100 or < 0)
                return await Task.FromResult<ProductOutputDTO>(null);

            var userId = int.Parse(userIdStr);
            var userRating = await _ratingManager.FindByIdAsync(productId, userId);

            if (userRating is null)
            {
                await _ratingManager.AddAndSaveAsync(new ProductRating
                    {ProductId = productId, UserId = userId, Rating = rating});
            }
            else
            {
                userRating.Rating = rating;
                await _ratingManager.UpdateAndSaveAsync(userRating);
            }

            product.TotalRating = await _ratingManager.RecalculateRatingAsync(productId);
            await _productManager.UpdateAndSaveAsync(product);

            return _mapper.Map<ProductOutputDTO>(product);
        }

        public async Task<bool> DeleteRatingAsync(string userIdStr, int productId)
        {
            var product = await _productManager.FindByIdAsync(productId);
            if (product is null)
                return false;

            var userId = int.Parse(userIdStr);
            var userRating = await _ratingManager.FindByIdAsync(productId, userId);

            if (userRating is null)
                return false;

            await _ratingManager.DeleteAndSaveAsync(userRating);

            product.TotalRating = await _ratingManager.RecalculateRatingAsync(productId);
            await _productManager.UpdateAndSaveAsync(product);

            return true;
        }
    }
}