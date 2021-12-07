using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Business.DTO;
using Business.Helpers;
using Business.Interfaces;
using Business.Parameters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "v3")]
    public class GamesController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IRatingService _ratingService;

        public GamesController(IProductService productService, IRatingService ratingService, ILogger logger) : base(logger)
        {
            _productService = productService;
            _ratingService = ratingService;
        }

        /// <summary>
        /// Represents top 3 popular platforms
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("top-platforms")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Platform>>> GetTopPlatforms() => 
            Ok(await _productService.GetTopPlatformsAsync());

        /// <summary>
        /// Search for product
        /// </summary>
        /// <param name="term" example="product">Name of the product you are looking for</param>
        /// <param name="limit" example="10">Max number of matches</param>
        /// <param name="offset" example="3">Skipped matches</param>
        /// <param name="productParameters">Page</param>
        /// <response code="200">Searching was successful</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("search-product")]
        [ServiceFilter(typeof(PagesValidationFilter))]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductOutputDTO>>> SearchProducts(
            [Required] string term, int? limit, int? offset, [FromQuery] PageParameters productParameters)
        {
            var result = await _productService.SearchProductsAsync(term, limit, offset, productParameters);
            if (result is null)
                return BadRequest();

            return !result.Any() ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Find product by Id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Bad parameters</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductOutputDTO>> FindProductById([Required] int id)
        {
            var result = await _productService.FindByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Add product
        /// </summary>
        /// <response code="201">Success</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">You don't have enough rights for this request</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<ProductOutputDTO>> AddProduct([FromForm] ProductInputDTO newProduct)
        {
            var result = await _productService.AddAsync(newProduct);
            return result is null ? BadRequest() : Created(new Uri(Request.GetDisplayUrl()), result);

        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <response code="201">Success</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">You don't have enough rights for this request</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<ProductOutputDTO>> UpdateProduct([FromForm] ProductInputDTO productDtoUpdate)
        {
            var result = await _productService.UpdateAsync(productDtoUpdate);
            return result is null ? BadRequest() : Created(new Uri(Request.GetDisplayUrl()), result);
        }

        /// <summary>
        /// Delete product by id
        /// </summary>
        /// <response code="204">Product was deleted</response>
        /// <response code="404">Not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">You don't have enough rights for this request</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpDelete("id")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteProductById([Required] int id)
        {
            var result = await _productService.DeleteByIdAsync(id);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Rate product
        /// </summary>
        /// <response code="201">Product was rated</response>
        /// <response code="404">Not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPost("rating")]
        [Authorize]
        public async Task<ActionResult<ProductOutputDTO>> RateProduct([Required] int rating, [Required] int productId)
        {
            var result = await _ratingService.RateAsync(User.Claims.GetUserId(), rating, productId);
            return result is null ? NotFound() : Created(new Uri(Request.GetDisplayUrl()), result);
        }

        /// <summary>
        /// Delete product rating
        /// </summary>
        /// <response code="204">Rating was deleted</response>
        /// <response code="404">Not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpDelete("rating")]
        [Authorize]
        public async Task<IActionResult> DeleteProductRating(int productId)
        {
            var result = await _ratingService.DeleteRatingAsync(User.Claims.GetUserId(), productId);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Represents filtered products
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("list")]
        [ServiceFilter(typeof(PagesValidationFilter))]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductOutputDTO>>> SearchProductsByFilters(
            [FromQuery] PageParameters pageParameters,
            Genre genre, Rating rating, bool ratingAscending = false, bool priceAscending = true)
        {
            var result = await _productService.SearchProductsByFiltersAsync(pageParameters, genre, rating, ratingAscending,
                priceAscending);

            if (result is null)
                return BadRequest();

            return !result.Any() ? NotFound() : Ok(result);
        }
    }
}
