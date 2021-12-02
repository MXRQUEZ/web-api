using System.Threading.Tasks;
using Business.DTO;
using Business.Helpers;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "v5")]
    public sealed class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService, ILogger logger) : base(logger)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Order product
        /// </summary>
        /// <response code="201">Ordered</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPost("orders")]
        [Authorize]
        public async Task<OrderOutputDTO> OrderProduct(int productId, int amount)
        {
            return await _orderService.OrderAsync(User.Claims.GetUserId(), productId, amount);
        }

        /// <summary>
        /// Represents your order
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("orders")]
        [Authorize]
        public async Task<OrderOutputDTO> RepresentOrder(int? orderId = null)
        {
            return await _orderService.RepresentOrderAsync(User.Claims.GetUserId(), orderId);
        }

        /// <summary>
        /// Update ordered product
        /// </summary>
        /// <response code="201">Updated</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPut("orders")]
        [Authorize]
        public async Task<OrderOutputDTO> UpdateOrderItem(OrderItemInputDTO orderItemDto)
        {
            return await _orderService.UpdateOrderItemAsync(User.Claims.GetUserId(), orderItemDto);
        }

        /// <summary>
        /// Delete product from order by Id
        /// </summary>
        /// <response code="204">Deleted</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpDelete("orders")]
        [Authorize]
        public async Task<IActionResult> DeleteOrderItem(int productId)
        {
            await _orderService.DeleteOrderItemAsync(User.Claims.GetUserId(), productId);
            return NoContent();
        }

        /// <summary>
        /// Pay for your order
        /// </summary>
        /// <response code="201">Paid</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPost("buy")]
        [Authorize]
        public async Task<IActionResult> PayOrder()
        {
            await _orderService.PayOrderAsync(User.Claims.GetUserId());
            return NoContent();
        }
    }
}
