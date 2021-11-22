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

        public async Task<ProductOutputDTO> RateAsync(string userId, int rating, int productId)
        {
            var product = await GetProductAsync(productId, rating);
            var userIntId = int.Parse(userId);
            var userRating = await _ratingRepository
                .GetAll()
                .FirstOrDefaultAsync(r => r.ProductId.Equals(productId) && r.UserId.Equals(userIntId));

            if (userRating is null)
                await _ratingRepository.AddAsync(new ProductRating { ProductId = productId, UserId = userIntId, Rating = rating });
            else
            {
                userRating.Rating = rating;
                await _ratingRepository.UpdateAsync(userRating);
            }

            var allProductRatings = _ratingRepository
                .GetAll()
                .Where(r => r.ProductId.Equals(productId));

            var ratingsSum = await allProductRatings.SumAsync(r => r.Rating);
            var ratingsCount = await allProductRatings.CountAsync();

            product.TotalRating = ratingsSum / ratingsCount;
            var result = await _productRepository.UpdateAsync(product);

            return _mapper.Map<ProductOutputDTO>(result);
        }

        public async Task DeleteRatingAsync(string userId, int productId)
        {
            var product = await GetProductAsync(productId);
            var userIntId = int.Parse(userId);
            var userRating = await _ratingRepository
                .GetAll()
                .FirstOrDefaultAsync(r => r.ProductId.Equals(productId) && r.UserId.Equals(userIntId));

            if (userRating is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);

            await _ratingRepository.DeleteAsync(userRating);

            var allProductRatings = _ratingRepository
                .GetAll()
                .Where(r => r.ProductId.Equals(productId));

            var ratingsCount = await allProductRatings.CountAsync();

            if (ratingsCount == 0)
                product.TotalRating = 0;
            else
            {
                var ratingsSum = await allProductRatings.SumAsync(r => r.Rating);
                product.TotalRating = ratingsSum / ratingsCount;
            }

            await _productRepository.UpdateAsync(product);
        }

        private async Task<Product> GetProductAsync(int productId, int rating = 0)
        {
            if (rating is > 100 or < 0)
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"{ExceptionMessage.BadParameter}." +
                                                                         "Rating can't be more than 100 or less than 0");

            var product = await _productRepository
                .GetAll()
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.ProductNotFound);

            return product;
        }
    }
}
