using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DAL.Interfaces;
using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public sealed class RatingService : IRatingService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ProductRating> _ratingRepository;
        private readonly IRepository<Product> _productRepository;
        public RatingService(IRepository<ProductRating> ratingRepository, IRepository<Product> productRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductOutputDTO> RateAsync(string userIdStr, int rating, int productId)
        {
            var product = await GetProductAsync(productId, rating);
            if (product is null)
                return null;

            var userId = int.Parse(userIdStr);
            var userRating = await _ratingRepository
                .GetAll(false)
                .FirstOrDefaultAsync(r => r.ProductId.Equals(productId) && r.UserId.Equals(userId));

            if (userRating is null)
                await _ratingRepository.AddAndSaveAsync(new ProductRating { ProductId = productId, UserId = userId, Rating = rating });
            else
            {
                userRating.Rating = rating;
                await _ratingRepository.UpdateAndSaveAsync(userRating);
            }

            product = await RecalculateRatingAsync(product, productId);

            return _mapper.Map<ProductOutputDTO>(product);
        }

        public async Task<bool> DeleteRatingAsync(string userIdStr, int productId)
        {
            var product = await GetProductAsync(productId);
            if (product is null)
                return false;

            var userId = int.Parse(userIdStr);
            var userRating = await _ratingRepository
                .GetAll(false)
                .FirstOrDefaultAsync(r => r.ProductId.Equals(productId) && r.UserId.Equals(userId));

            if (userRating is null)
                return false;

            await _ratingRepository.DeleteAndSaveAsync(userRating);

            await RecalculateRatingAsync(product, productId);
            return true;
        }

        private async Task<Product> GetProductAsync(int productId, int rating = 0)
        {
            if (rating is > 100 or < 0)
                return null;

            var product = await _productRepository
                .GetAll(false)
                .FirstOrDefaultAsync(p => p.Id == productId);

            return product ?? await Task.FromResult<Product>(null);
        }

        private async Task<Product> RecalculateRatingAsync(Product product, int productId)
        {
            var allProductRatings = _ratingRepository
                .GetAll(false)
                .Where(r => r.ProductId.Equals(productId));

            var ratingsSum = await allProductRatings.SumAsync(r => r.Rating);
            var ratingsCount = await allProductRatings.CountAsync();

            product.TotalRating = ratingsCount != 0
                ? ratingsSum / ratingsCount 
                : 0;

            await _productRepository.UpdateAndSaveAsync(product);
            return product;
        }
    }
}
