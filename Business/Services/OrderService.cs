using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DAL.Interfaces;
using DAL.Models.Entities;

namespace Business.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderManager _orderManager;
        private readonly IProductManager _productManager;

        public OrderService(IOrderManager orderManager, IProductManager productManager, IMapper mapper)
        {
            _orderManager = orderManager;
            _productManager = productManager;
            _mapper = mapper;
        }

        public async Task<OrderOutputDTO> OrderAsync(string userIdStr, int productId, int amount)
        {
            var userId = int.Parse(userIdStr);
            var userOrder = await _orderManager.FindByUserIdAsync(userId);

            if (userOrder is null)
            {
                userOrder = new Order {UserId = userId};
                await _orderManager.AddAndSaveAsync(userOrder);
            }
            else
            {
                var orderExists = userOrder.OrderItems.Any(o => o.ProductId == productId);
                if (orderExists)
                    return await Task.FromResult<OrderOutputDTO>(null);
            }

            var orderedItem = new OrderItem
                {ProductId = productId, OrderId = userOrder.Id, Amount = amount, IsBought = false};
            userOrder.OrderItems.Add(orderedItem);
            userOrder.CreationDate = DateTime.Now;
            await _orderManager.UpdateAndSaveAsync(userOrder);

            return _mapper.Map<OrderOutputDTO>(userOrder);
        }

        public async Task<OrderOutputDTO> RepresentOrderAsync(string userIdStr, int? orderId)
        {
            var userId = int.Parse(userIdStr);

            var userOrder = orderId is not null
                ? await _orderManager.FindByIdAsync(orderId.Value)
                : await _orderManager.FindByUserIdAsync(userId);

            return userOrder is null
                ? await Task.FromResult<OrderOutputDTO>(null)
                : _mapper.Map<OrderOutputDTO>(userOrder);
        }

        public async Task<OrderOutputDTO> UpdateOrderItemAsync(string userIdStr, OrderItemInputDTO orderItemDto)
        {
            var userOrder = await _orderManager.FindByUserIdAsync(int.Parse(userIdStr));

            var orderItem = userOrder?.OrderItems.FirstOrDefault(i => i.ProductId == orderItemDto.ProductId);

            if (orderItem is null || orderItem.IsBought)
                return await Task.FromResult<OrderOutputDTO>(null);

            var itemIndex = userOrder.OrderItems.IndexOf(orderItem);
            userOrder.OrderItems[itemIndex].Amount = orderItemDto.Amount;

            await _orderManager.UpdateAndSaveAsync(userOrder);

            return _mapper.Map<OrderOutputDTO>(userOrder);
        }

        public async Task<bool> DeleteOrderItemAsync(string userIdStr, int productId)
        {
            var userOrder = await _orderManager.FindByUserIdAsync(int.Parse(userIdStr));
            var orderItem = userOrder?.OrderItems.FirstOrDefault(i => i.ProductId == productId);
            if (orderItem is null)
                return false;

            userOrder.OrderItems.Remove(orderItem);

            if (userOrder.OrderItems.Count == 0)
                await _orderManager.DeleteAndSaveAsync(userOrder);
            else
                await _orderManager.UpdateAndSaveAsync(userOrder);

            return true;
        }

        public async Task<bool> PayOrderAsync(string userIdStr)
        {
            var userOrder = await _orderManager.FindByUserIdAsync(int.Parse(userIdStr));

            var notPaidItems = userOrder.OrderItems.Where(i => !i.IsBought).AsQueryable();
            if (!notPaidItems.Any())
                return false;

            foreach (var orderItem in notPaidItems)
            {
                orderItem.IsBought = true;
                var product = await _productManager.FindByIdAsync(orderItem.ProductId);
                if (orderItem.Amount > product.Count)
                    return false;

                product.Count -= orderItem.Amount;
                _productManager.Update(product);
            }

            await _orderManager.UpdateAndSaveAsync(userOrder);
            return true;
        }
    }
}