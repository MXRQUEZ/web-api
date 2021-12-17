using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Business.DTO;
using Business.Helpers;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "v5")]
    [Authorize]
    public sealed class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService, ILogger logger) : base(logger) =>
            _orderService = orderService;

        /// <summary>
        ///     Order product
        /// </summary>
        /// <response code="201">Ordered</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPost("orders")]
        public async Task<IActionResult> OrderProduct(int productId, int amount)
        {
            var result = await _orderService.OrderAsync(User.Claims.GetUserId(), productId, amount);
            return result is null ? NotFound() : Created(new Uri(Request.GetDisplayUrl()), result);
        }

        /// <summary>
        ///     Represents your order
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("orders")]
        public async Task<ActionResult<OrderOutputDTO>> RepresentOrder(int? orderId = null)
        {
            var result = await _orderService.RepresentOrderAsync(User.Claims.GetUserId(), orderId);
            return result is null ? NotFound() : result;
        }

        /// <summary>
        ///     Update ordered product
        /// </summary>
        /// <response code="201">Updated</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPut("orders")]
        public async Task<IActionResult> UpdateOrderItem(OrderItemInputDTO orderItemDto)
        {
            var result = await _orderService.UpdateOrderItemAsync(User.Claims.GetUserId(), orderItemDto);
            return result is null ? NotFound() : Created(new Uri(Request.GetDisplayUrl()), result);
        }

        /// <summary>
        ///     Delete product from order by Id
        /// </summary>
        /// <response code="204">Deleted</response>
        /// <response code="404">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpDelete("orders")]
        public async Task<IActionResult> DeleteOrderItem([Required] int productId)
        {
            var result = await _orderService.DeleteOrderItemAsync(User.Claims.GetUserId(), productId);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        ///     Pay for your order
        /// </summary>
        /// <response code="204">Paid</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPost("buy")]
        public async Task<IActionResult> PayOrder()
        {
            var result = await _orderService.PayOrderAsync(User.Claims.GetUserId());
            return result ? NoContent() : BadRequest();
        }
    }
}