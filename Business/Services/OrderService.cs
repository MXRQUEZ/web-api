using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Business.DTO;
using Business.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using DAL.Interfaces;
using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;

        public OrderService(IRepository<Order> orderRepository, IRepository<Product> productRepository, IMapper mapper)
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
                .FirstOrDefaultAsync(o => o.UserId.Equals(userId));

            if (userOrder is null)
            {
                userOrder = new Order {UserId = userId};
                await _orderRepository.AddAndSaveAsync(userOrder);
            }
            else
            {
                var orderExists = userOrder.OrderItems.Any(o => o.ProductId.Equals(productId));
                if (orderExists)
                    throw new HttpStatusException(HttpStatusCode.BadRequest,
                        $"{ExceptionMessage.BadParameter}! Product is in order already");
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
            var userOrder = orderId is null
                ? await GetUserOrderAsync(o => o.UserId.Equals(userId))
                : await GetUserOrderAsync(o => o.Id.Equals(orderId));

            return _mapper.Map<OrderOutputDTO>(userOrder);
        }

        public async Task<OrderOutputDTO> UpdateOrderItemAsync(string userIdStr, OrderItemInputDTO orderItemDto)
        {
            var userOrder = await GetUserOrderAsync(userIdStr);
            var orderItem = GetOrderItem(userOrder, orderItemDto.ProductId);

            if (orderItem.IsBought)
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.ProductIsBought);

            var itemIndex = userOrder.OrderItems.IndexOf(orderItem);
            userOrder.OrderItems[itemIndex].Amount = orderItemDto.Amount;

            await _orderRepository.UpdateAndSaveAsync(userOrder);

            return _mapper.Map<OrderOutputDTO>(userOrder);
        }

        public async Task DeleteOrderItemAsync(string userIdStr, int productId)
        {
            var userOrder = await GetUserOrderAsync(userIdStr, true);

            var orderItem = userOrder.OrderItems.FirstOrDefault(i => i.ProductId.Equals(productId));

            userOrder.OrderItems.Remove(orderItem);

            if (userOrder.OrderItems.Count == 0)
                await _orderRepository.DeleteAndSaveAsync(userOrder);
            else
                await _orderRepository.UpdateAndSaveAsync(userOrder);
        }

        public async Task PayOrderAsync(string userIdStr)
        {
            var userOrder = await GetUserOrderAsync(userIdStr);

            var products = _productRepository.GetAll(true);
            foreach (var orderItem in userOrder.OrderItems.Where(i => !i.IsBought))
            {
                orderItem.IsBought = true;
                var product = await products.FirstAsync(p => p.Id.Equals(orderItem.ProductId));
                if (orderItem.Amount > product.Count)
                    throw new HttpStatusException(HttpStatusCode.BadRequest,
                        $"{ExceptionMessage.BadParameter} {nameof(orderItem.Amount)}. Not enough items in storage. Please, check out later");

                product.Count -= orderItem.Amount;
                _productRepository.Update(product);
            }

            await _orderRepository.UpdateAndSaveAsync(userOrder);
        }

        private async Task<Order> GetUserOrderAsync(string userIdStr, bool trackChanges = false)
        {
            var userId = int.Parse(userIdStr);
            var userOrder = await _orderRepository
                .GetAll(trackChanges)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId.Equals(userId));

            if (userOrder is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.OrderNotFound);

            return userOrder;
        }

        private async Task<Order> GetUserOrderAsync(Expression<Func<Order, bool>> expression, bool trackChanges = false)
        {
            var userOrder = await _orderRepository
                .GetAll(trackChanges)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(expression);

            if (userOrder is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.OrderNotFound);

            return userOrder;
        }

        private static OrderItem GetOrderItem(Order userOrder, int productId)
        {
            var orderItem = userOrder.OrderItems.FirstOrDefault(i => i.ProductId.Equals(productId));
            if (orderItem is null)
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.OrderNotFound);

            return orderItem;
        }
    }
}
