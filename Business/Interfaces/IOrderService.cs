using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IOrderService
    {
        Task<OrderOutputDTO> OrderAsync(string userIdStr, int productId, int amount);
        Task<OrderOutputDTO> RepresentOrderAsync(string userIdStr, int? orderId);
        Task<OrderOutputDTO> UpdateOrderItemAsync(string userIdStr, OrderItemInputDTO orderItemDto);
        Task DeleteOrderItemAsync(string userIdStr, int productId);
        Task PayOrderAsync(string userIdStr);
    }
}
