using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.DTO;
using Business.Services;
using DAL.Models.Entities;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using MockQueryable.FakeItEasy;
using Xunit;
using static Tests.Extensions.TestData.FakeTestData;
using static Tests.Extensions.TestData.UserTestData;
using static Tests.Extensions.TestData.ProductTestData;

namespace Tests.Services
{
    public sealed class OrderServiceTests
    {
        [Fact]
        public async Task OrderAsync_WithUnorderedItem_ReturnOrderOutputDto()
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
            var result = await orderService.OrderAsync(UserId, ProductId, ProductAmount);

            // Assert
            Assert.IsType<OrderOutputDTO>(result);
        }

        [Fact]
        public async Task OrderAsync_WithAlreadyOrderedItem_ReturnNull()
        {
            // Arrange
            var userOrder = new Order { UserId = int.Parse(UserId) };
            var orderItem = new OrderItem { ProductId = ProductId };
            userOrder.OrderItems.Add(orderItem);
            var orderService = new OrderService(FakeOrderRepository, FakeProductRepository, FakeMapper);

            A.CallTo(() => FakeOrderRepository.GetAll(false))
                .Returns(new List<Order>{ userOrder }.AsQueryable().BuildMock());

            // Act
            var result = await orderService.OrderAsync(UserId, ProductId, ProductAmount);

            // Assert
            Assert.Null(result);
        }
    }
}
