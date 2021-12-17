using System.Threading.Tasks;
using Business.DTO;
using Business.Services;
using DAL.Models.Entities;
using FakeItEasy;
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
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(Task.FromResult<Order>(null));

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
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            // Act
            var result = await orderService.OrderAsync(UserId, ProductId, ProductCount);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RepresentOrderAsync_ByUserId_ReturnOrder()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);
            var userOrder = new Order {UserId = int.Parse(UserId)};

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            // Act
            var result = await orderService.RepresentOrderAsync(UserId, null);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task RepresentOrderAsync_ByValidOrderId_ReturnOrder()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);
            var userOrder = new Order {Id = OrderId};

            A.CallTo(() => FakeOrderManager.FindByIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            // Act
            var result = await orderService.RepresentOrderAsync(UserId, OrderId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task RepresentOrderAsync_WithNonExistingOrderId_ReturnNull()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);

            A.CallTo(() => FakeOrderManager.FindByIdAsync(A<int>.Ignored))
                .Returns(Task.FromResult<Order>(null));

            // Act
            var result = await orderService.RepresentOrderAsync(UserId, OrderId + 1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateOrderAsync_WithValidOrderItem_ReturnOrder()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId});
            var orderItemInputDto = new OrderItemInputDTO {ProductId = ProductId};

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            // Act
            var result = await orderService.UpdateOrderItemAsync(UserId, orderItemInputDto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateOrderAsync_WithUnorderedOrderItem_ReturnNull()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId + 1});
            var orderItemInputDto = new OrderItemInputDTO {ProductId = ProductId};

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            // Act
            var result = await orderService.UpdateOrderItemAsync(UserId, orderItemInputDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteOrderAsync_WithExistingProductId_ReturnTrue()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId});

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            // Act
            var result = await orderService.DeleteOrderItemAsync(UserId, ProductId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteOrderAsync_WithNonExistingProductId_ReturnFalse()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId});

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            // Act
            var result = await orderService.DeleteOrderItemAsync(UserId, ProductId + 1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task PayOrderAsync_WithUnpaidProduct_ReturnTrue()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            var product = new Product {Id = ProductId, Count = ProductCount};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId, Amount = OrderItemAmount, IsBought = false});

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            // Act
            var result = await orderService.PayOrderAsync(UserId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task PayOrderAsync_WithAlreadyPaidProduct_ReturnFalse()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            var product = new Product {Id = ProductId, Count = 3};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId, Amount = OrderItemAmount, IsBought = true});

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);


            // Act
            var result = await orderService.PayOrderAsync(UserId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task PayOrderAsync_WithNotEnoughProductsInStock_ReturnFalse()
        {
            // Arrange
            var orderService = new OrderService(FakeOrderManager, FakeProductManager, FakeMapper);
            var userOrder = new Order {Id = OrderId, UserId = int.Parse(UserId)};
            var product = new Product {Id = ProductId, Count = ProductCount - 2};
            userOrder.OrderItems.Add(new OrderItem {ProductId = ProductId, Amount = OrderItemAmount, IsBought = false});

            A.CallTo(() => FakeOrderManager.FindByUserIdAsync(A<int>.Ignored))
                .Returns(userOrder);

            A.CallTo(() => FakeProductManager.FindByIdAsync(A<int>.Ignored))
                .Returns(product);

            // Act
            var result = await orderService.PayOrderAsync(UserId);

            // Assert
            Assert.False(result);
        }
    }
}