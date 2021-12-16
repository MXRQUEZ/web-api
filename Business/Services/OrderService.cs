using System;
using System.Linq;
using System.Linq.Expressions;
using Business.DTO;
using Business.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Interfaces;
using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<OrderOutputDTO> OrderAsync(string userIdStr, int productId, int amount)
        {
            var userId = int.Parse(userIdStr);
            var userOrder = await _orderRepository
                .GetAll(false)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId);

            if (userOrder is null)
            {
                userOrder = new Order {UserId = userId};
                await _orderRepository.AddAndSaveAsync(userOrder);
            }
            else
            {
                var orderExists = userOrder.OrderItems.Any(o => o.ProductId == productId);
                if (orderExists)
                    return await Task.FromResult<OrderOutputDTO>(null);
            }

            var orderedItem = new OrderItem {ProductId = productId, OrderId = userOrder.Id, Amount = amount, IsBought = false};
            userOrder.OrderItems.Add(orderedItem);
            userOrder.CreationDate = DateTime.Now;
            await _orderRepository.UpdateAndSaveAsync(userOrder);

            return _mapper.Map<OrderOutputDTO>(userOrder);
        }

        public async Task<OrderOutputDTO> RepresentOrderAsync(string userIdStr, int? orderId)
        {
            var userId = int.Parse(userIdStr);

            var userOrder = orderId is not null
                ? await GetUserOrderAsync(o => o.Id == orderId)
                : await GetUserOrderAsync(o => o.UserId == userId);

            return userOrder is null
                ? await Task.FromResult<OrderOutputDTO>(null)
                : _mapper.Map<OrderOutputDTO>(userOrder);
        }

        public async Task<OrderOutputDTO> UpdateOrderItemAsync(string userIdStr, OrderItemInputDTO orderItemDto)
        {
            var userOrder = await GetUserOrderAsync(userIdStr);

            var orderItem = userOrder?.OrderItems.FirstOrDefault(i => i.ProductId == orderItemDto.ProductId);

            if (orderItem is null || orderItem.IsBought)
                return await Task.FromResult<OrderOutputDTO>(null);

            var itemIndex = userOrder.OrderItems.IndexOf(orderItem);
            userOrder.OrderItems[itemIndex].Amount = orderItemDto.Amount;

            await _orderRepository.UpdateAndSaveAsync(userOrder);

            return _mapper.Map<OrderOutputDTO>(userOrder);
        }

        public async Task<bool> DeleteOrderItemAsync(string userIdStr, int productId)
        {
            var userOrder = await GetUserOrderAsync(userIdStr, true);
            var orderItem = userOrder?.OrderItems.FirstOrDefault(i => i.ProductId == productId);
            if (orderItem is null)
                return false;

            userOrder.OrderItems.Remove(orderItem);

            if (userOrder.OrderItems.Count == 0)
                await _orderRepository.DeleteAndSaveAsync(userOrder);
            else
                await _orderRepository.UpdateAndSaveAsync(userOrder);

            return true;
        }

        public async Task<bool> PayOrderAsync(string userIdStr)
        {
            var userOrder = await GetUserOrderAsync(userIdStr);

            var products = _productRepository.GetAll(true);
            var notPaidItems = userOrder.OrderItems.Where(i => !i.IsBought).AsQueryable();
            if (!notPaidItems.Any())
                return false;

            foreach (var orderItem in notPaidItems)
            {
                orderItem.IsBought = true;
                var product = await products.FirstAsync(p => p.Id == orderItem.ProductId);
                if (orderItem.Amount > product.Count)
                    return false;

                product.Count -= orderItem.Amount;
                _productRepository.Update(product);
            }

            await _orderRepository.UpdateAndSaveAsync(userOrder);
            return true;
        }

        private async Task<Order> GetUserOrderAsync(string userIdStr, bool trackChanges = false)
        {
            var userId = int.Parse(userIdStr);
            var userOrder = await _orderRepository
                .GetAll(trackChanges)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId);

            return userOrder ?? await Task.FromResult<Order>(null);
        }

        private async Task<Order> GetUserOrderAsync(Expression<Func<Order, bool>> expression, bool trackChanges = false)
        {
            var userOrder = await _orderRepository
                .GetAll(trackChanges)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(expression);

            return userOrder ?? await Task.FromResult<Order>(null);
        }
    }
}
