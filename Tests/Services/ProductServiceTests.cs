using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.DTO;
using Business.Parameters;
using Business.Services;
using DAL.Models;
using DAL.Models.Entities;
using FakeItEasy;
using Tests.Extensions;
using Xunit;
using static Tests.Extensions.TestData.FakeTestData;
using static Tests.Extensions.TestData.ProductTestData;
using static Tests.Extensions.TestData.UserTestData;

namespace Tests.Services
{
    public sealed class ProductServiceTests : Tester
    {
        [Fact]
        public async Task SearchProductsByFiltersAsync_ReturnProducts()
        {
            // Arrange
            var products = CreateEnumerable<Product>(10);
            var filters = new ProductFilters();
            var pagination = new PageParameters();
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, null);
            A.CallTo(() => FakeProductManager.GetWhereAsync(A<Expression<Func<Product, bool>>>.Ignored))
                .Returns(products);

            // Act
            var result = await productService.SearchByFiltersAsync(pagination, filters);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTopPlatformsAsync_ReturnPlatforms()
        {
            // Arrange
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, null);
            A.CallTo(() => FakeProductManager.CountByPlatformAsync(A<Platform>.Ignored))
                .Returns(0);

            // Act
            var result = await productService.GetTopPlatformsAsync();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SearchProductsByTermAsync_ReturnProducts()
        {
            // Arrange
            var products = CreateEnumerable<Product>(10);
            var pagination = new PageParameters();
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, null);
            A.CallTo(() => FakeProductManager.GetWhereAsync(A<Expression<Func<Product, bool>>>.Ignored))
                .Returns(products);

            // Act
            var result = await productService.SearchByTermAsync(string.Empty, 0, 0, pagination);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnProduct()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, null);
            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            // Act
            var result = await productService.FindByIdAsync(ProductId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindByIdAsync_WithInvalidProductId_ReturnNull()
        {
            // Arrange
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, null);
            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(Task.FromResult<Product>(null));

            // Act
            var result = await productService.FindByIdAsync(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ReturnProduct()
        {
            // Arrange
            var productInputDto = new ProductInputDTO();
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, FakeCloudinary);

            A.CallTo(() => FakeCloudinary.UploadProductLogoAsync(A<ProductInputDTO>.Ignored))
                .Returns(Task.FromResult<string>(null));

            A.CallTo(() => FakeCloudinary.UploadProductBackgroundAsync(A<ProductInputDTO>.Ignored))
                .Returns(Task.FromResult<string>(null));

            // Act
            var result = await productService.AddAsync(productInputDto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnProduct()
        {
            // Arrange
            var productInputDto = new ProductInputDTO {Name = ProductName};
            var product = new Product {Name = ProductName};
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, FakeCloudinary);

            A.CallTo(() => FakeProductManager.FindByNameAsync(A<string>.Ignored))
                .Returns(product);

            A.CallTo(() => FakeCloudinary.UploadProductLogoAsync(A<ProductInputDTO>.Ignored))
                .Returns(Task.FromResult<string>(null));

            A.CallTo(() => FakeCloudinary.UploadProductBackgroundAsync(A<ProductInputDTO>.Ignored))
                .Returns(Task.FromResult<string>(null));

            // Act
            var result = await productService.UpdateAsync(productInputDto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidProductName_ReturnNull()
        {
            // Arrange
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, null);

            A.CallTo(() => FakeProductManager.FindByNameAsync(A<string>.Ignored))
                .Returns(Task.FromResult<Product>(null));

            // Act
            var result = await productService.UpdateAsync(new ProductInputDTO());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProductAsync_WithRatedProduct_ReturnTrue()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var rating = new ProductRating {UserId = int.Parse(UserId), ProductId = ProductId};
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, null);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            A.CallTo(() => FakeRatingManager.GetByProductIdAsync(A<int>.Ignored))
                .Returns(new List<ProductRating> {rating});

            // Act
            var result = await productService.DeleteByIdAsync(ProductId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_WithUnratedProduct_ReturnTrue()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, null);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            A.CallTo(() => FakeRatingManager.GetByProductIdAsync(A<int>.Ignored))
                .Returns(new List<ProductRating>());

            // Act
            var result = await productService.DeleteByIdAsync(ProductId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductAsync_WithInvalidProductId_ReturnFalse()
        {
            // Arrange
            var productService = new ProductService(FakeProductManager, FakeRatingManager, FakeMapper, null);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(Task.FromResult<Product>(null));

            // Act
            var result = await productService.DeleteByIdAsync(0);

            // Assert
            Assert.False(result);
        }
    }
}