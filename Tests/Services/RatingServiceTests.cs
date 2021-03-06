using System.Threading.Tasks;
using Business.Services;
using DAL.Models.Entities;
using FakeItEasy;
using Xunit;
using static Tests.Extensions.TestData.FakeTestData;
using static Tests.Extensions.TestData.UserTestData;
using static Tests.Extensions.TestData.ProductTestData;

namespace Tests.Services
{
    public sealed class RatingServiceTests
    {
        [Fact]
        public async Task RateAsync_ReturnProduct()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var rating = new ProductRating {ProductId = ProductId};
            var ratingService = new RatingService(FakeRatingManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            A.CallTo(() => FakeRatingManager.FindByIdAsync(A<int>.Ignored, A<int>.Ignored))
                .Returns(rating);

            A.CallTo(() => FakeRatingManager.RecalculateRatingAsync(A<int>.Ignored))
                .Returns(0);

            // Act
            var result = await ratingService.RateAsync(UserId, 100, ProductId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task RateAsync_WithAlreadyRatedProduct_ReturnProduct()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var rating = new ProductRating {ProductId = ProductId, UserId = int.Parse(UserId)};
            var ratingService = new RatingService(FakeRatingManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            A.CallTo(() => FakeRatingManager.FindByIdAsync(A<int>.Ignored, A<int>.Ignored))
                .Returns(rating);

            A.CallTo(() => FakeRatingManager.RecalculateRatingAsync(A<int>.Ignored))
                .Returns(0);

            // Act
            var result = await ratingService.RateAsync(UserId, 100, ProductId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task RateAsync_WithInvalidRating_ReturnNull()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var rating = new ProductRating {ProductId = ProductId, UserId = int.Parse(UserId)};
            var ratingService = new RatingService(FakeRatingManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            A.CallTo(() => FakeRatingManager.FindByIdAsync(A<int>.Ignored, A<int>.Ignored))
                .Returns(rating);

            A.CallTo(() => FakeRatingManager.RecalculateRatingAsync(A<int>.Ignored))
                .Returns(0);

            // Act
            var result = await ratingService.RateAsync(UserId, 101, ProductId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RateAsync_WithInvalidProductId_ReturnNull()
        {
            // Arrange
            var ratingService = new RatingService(FakeRatingManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(Task.FromResult<Product>(null));

            // Act
            var result = await ratingService.RateAsync(UserId, 100, ProductId + 1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteRatingAsync_ReturnTrue()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var rating = new ProductRating {ProductId = ProductId, UserId = int.Parse(UserId)};
            var ratingService = new RatingService(FakeRatingManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            A.CallTo(() => FakeRatingManager.FindByIdAsync(A<int>.Ignored, A<int>.Ignored))
                .Returns(rating);

            A.CallTo(() => FakeRatingManager.RecalculateRatingAsync(A<int>.Ignored))
                .Returns(0);

            // Act
            var result = await ratingService.DeleteRatingAsync(UserId, ProductId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteRatingAsync_WithNonExistingRating_ReturnFalse()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var ratingService = new RatingService(FakeRatingManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            A.CallTo(() => FakeRatingManager.FindByIdAsync(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult<ProductRating>(null));

            // Act
            var result = await ratingService.DeleteRatingAsync(UserId, ProductId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteRatingAsync_WithNonExistingProductId_ReturnFalse()
        {
            // Arrange
            var ratingService = new RatingService(FakeRatingManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(Task.FromResult<Product>(null));

            // Act
            var result = await ratingService.DeleteRatingAsync(UserId, ProductId);

            // Assert
            Assert.False(result);
        }
    }
}