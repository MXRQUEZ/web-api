using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.DTO;
using Business.Services;
using DAL.Models.Entities;
using FakeItEasy;
using MockQueryable.FakeItEasy;
using Xunit;
using static Tests.Extensions.TestData.FakeTestData;
using static Tests.Extensions.TestData.UserTestData;
using static Tests.Extensions.TestData.ProductTestData;
using static Tests.Extensions.TestData.OrderTestData;

namespace Tests.Services
{
    public sealed class OrderServiceTests
    {
        [Fact]
        public async Task OrderAsync_WithUnorderedItem_ReturnOrder()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order>().AsQueryable().BuildMock());

            A.CallTo(() => FakeOrderRepository.AddAsync(A<Order>.Ignored))
                .Returns(Task.CompletedTask);

            A.CallTo(() => FakeOrderRepository.UpdateAndSaveAsync(A<Order>.Ignored))
                .Returns(Task.CompletedTask);

            // Act
            var result = await orderService.OrderAsync(UserId, ProductId, ProductCount);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task OrderAsync_WithAlreadyOrderedItem_ReturnNull()
        {
            // Arrange
            var userOrder = new Order {UserId = int.Parse(UserId)};
            var orderItem = new OrderItem {ProductId = ProductId};
            userOrder.OrderItems.Add(orderItem);
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            // Act
            var result = await orderService.OrderAsync(UserId, ProductId, ProductCount);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RepresentOrderAsync_ByUserId_ReturnOrder()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {UserId = int.Parse(UserId)};

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            // Act
            var result = await orderService.RepresentOrderAsync(UserId, null);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task RepresentOrderAsync_ByValidOrderId_ReturnOrder()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {Id = OrderId};

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            // Act
            var result = await orderService.RepresentOrderAsync(UserId, OrderId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task RepresentOrderAsync_WithNonExistingOrderId_ReturnNull()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {Id = OrderId};

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            // Act
            var result = await orderService.RepresentOrderAsync(UserId, OrderId + 1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateOrderAsync_WithValidOrderItem_ReturnOrder()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId});
            var orderItemInputDto = new OrderItemInputDTO {ProductId = ProductId};

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            // Act
            var result = await orderService.UpdateOrderItemAsync(UserId, orderItemInputDto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateOrderAsync_WithUnorderedOrderItem_ReturnNull()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId + 1});
            var orderItemInputDto = new OrderItemInputDTO {ProductId = ProductId};

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            // Act
            var result = await orderService.UpdateOrderItemAsync(UserId, orderItemInputDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteOrderAsync_WithExistingProductId_ReturnTrue()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId});

            A.CallTo(() => FakeOrderRepository.GetAll(true))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            // Act
            var result = await orderService.DeleteOrderItemAsync(UserId, ProductId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteOrderAsync_WithNonExistingProductId_ReturnFalse()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId + 1});

            A.CallTo(() => FakeOrderRepository.GetAll(true))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            // Act
            var result = await orderService.DeleteOrderItemAsync(UserId, ProductId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task PayOrderAsync_WithUnpaidProduct_ReturnTrue()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            var product = new Product {Id = ProductId, Count = ProductCount};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId, Amount = OrderItemAmount, IsBought = false});

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            A.CallTo(() => FakeProductRepository.GetAll(true))
                .Returns(new List<Product> {product}.AsQueryable().BuildMock());

            A.CallTo(() => FakeOrderRepository.UpdateAndSaveAsync(userOrder))
                .Returns(Task.CompletedTask);

            // Act
            var result = await orderService.PayOrderAsync(UserId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task PayOrderAsync_WithAlreadyPaidProduct_ReturnFalse()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            var product = new Product {Id = ProductId, Count = 3};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId, Amount = OrderItemAmount, IsBought = true});

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            A.CallTo(() => FakeProductRepository.GetAll(true))
                .Returns(new List<Product> {product}.AsQueryable().BuildMock());

            A.CallTo(() => FakeOrderRepository.UpdateAndSaveAsync(userOrder))
                .Returns(Task.CompletedTask);

            // Act
            var result = await orderService.PayOrderAsync(UserId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task PayOrderAsync_WithNotEnoughProductsInStock_ReturnFalse()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            var product = new Product {Id = ProductId, Count = ProductCount - 2};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId, Amount = OrderItemAmount, IsBought = false});

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order> {userOrder}.AsQueryable().BuildMock());

            A.CallTo(() => FakeProductRepository.GetAll(true))
                .Returns(new List<Product> {product}.AsQueryable().BuildMock());

            A.CallTo(() => FakeOrderRepository.UpdateAndSaveAsync(userOrder))
                .Returns(Task.CompletedTask);

            // Act
            var result = await orderService.PayOrderAsync(UserId);

            // Assert
            Assert.False(result);
        }
    }
}