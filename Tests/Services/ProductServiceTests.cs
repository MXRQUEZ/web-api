using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.DTO;
using Business.Parameters;
using Business.Services;
using DAL.Models.Entities;
using FakeItEasy;
using MockQueryable.FakeItEasy;
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
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);
            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(products.AsQueryable().BuildMock());

            // Act
            var result = await productService.SearchProductsByFiltersAsync(pagination, filters);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTopPlatformsAsync_ReturnsPlatforms()
        {
            // Arrange
            var products = CreateEnumerable<Product>(10);
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);
            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(products.AsQueryable().BuildMock());

            // Act
            var result = await productService.GetTopPlatformsAsync();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SearchProductsByTermAsync_ReturnsProducts()
        {
            // Arrange
            var products = CreateEnumerable<Product>(10);
            var pagination = new PageParameters();
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);
            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(products.AsQueryable().BuildMock());

            // Act
            var result = await productService.SearchProductsAsync(string.Empty, 0, 0, pagination);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsProduct()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);
            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(new List<Product>{product}.AsQueryable().BuildMock());

            // Act
            var result = await productService.FindByIdAsync(ProductId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindByIdAsync_WithInvalidProductId_ReturnsNull()
        {
            // Arrange
            var product = new Product { Id = ProductId };
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);
            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(new List<Product> { product }.AsQueryable().BuildMock());

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
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);
            A.CallTo(() => FakeProductRepository.AddAndSaveAsync(A<Product>.Ignored))
                .Returns(Task.CompletedTask);

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
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);

            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(new List<Product>{product}.AsQueryable().BuildMock());

            A.CallTo(() => FakeProductRepository.UpdateAndSaveAsync(A<Product>.Ignored))
                .Returns(Task.CompletedTask);

            // Act
            var result = await productService.UpdateAsync(productInputDto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidProductName_ReturnNull()
        {
            // Arrange
            var productInputDto = new ProductInputDTO { Name = "OtherName" };
            var product = new Product { Name = ProductName };
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);

            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(new List<Product> { product }.AsQueryable().BuildMock());

            A.CallTo(() => FakeProductRepository.UpdateAndSaveAsync(A<Product>.Ignored))
                .Returns(Task.CompletedTask);

            // Act
            var result = await productService.UpdateAsync(productInputDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProductAsync_WithRatedProduct_ReturnTrue()
        {
            // Arrange
            var product = new Product {Id = ProductId};
            var rating = new ProductRating {UserId = int.Parse(UserId), ProductId = ProductId};
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);

            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(new List<Product> {product}.AsQueryable().BuildMock());

            A.CallTo(() => FakeRatingRepository.GetAll(false))
                .Returns(new List<ProductRating>{rating}.AsQueryable().BuildMock());

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
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);

            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(new List<Product> { product }.AsQueryable().BuildMock());

            A.CallTo(() => FakeRatingRepository.GetAll(false))
                .Returns(new List<ProductRating>().AsQueryable().BuildMock());

            // Act
            var result = await productService.DeleteByIdAsync(ProductId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductAsync_WithInvalidProductId_ReturnFalse()
        {
            // Arrange
            var product = new Product { Id = ProductId }; ;
            var productService = new ProductService(FakeProductRepository, FakeRatingRepository, FakeMapper, null);

            A.CallTo(() => FakeProductRepository.GetAll(false))
                .Returns(new List<Product> { product }.AsQueryable().BuildMock());

            A.CallTo(() => FakeRatingRepository.GetAll(false))
                .Returns(new List<ProductRating>().AsQueryable().BuildMock());

            // Act
            var result = await productService.DeleteByIdAsync(0);

            // Assert
            Assert.False(result);
        }
    }
}
