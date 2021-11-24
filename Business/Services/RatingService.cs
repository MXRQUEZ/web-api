using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Exceptions;
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

        public async Task DeleteRatingAsync(string userIdStr, int productId)
        {
            var product = await GetProductAsync(productId);
            var userId = int.Parse(userIdStr);
            var userRating = await _ratingRepository
                .GetAll(false)
                .FirstOrDefaultAsync(r => r.ProductId.Equals(productId) && r.UserId.Equals(userId));

            if (userRating is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);

            await _ratingRepository.DeleteAndSaveAsync(userRating);

            await RecalculateRatingAsync(product, productId);
        }

        private async Task<Product> GetProductAsync(int productId, int rating = 0)
        {
            if (rating is > 100 or < 0)
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"{ExceptionMessage.BadParameter}." +
                                                                         "Rating can't be more than 100 or less than 0");

            var product = await _productRepository
                .GetAll(false)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);

            return product;
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
