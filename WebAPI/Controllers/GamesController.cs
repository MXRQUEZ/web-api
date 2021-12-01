using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTO;
using Business.Helpers;
using Business.Interfaces;
using Business.Parameters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IEnumerable<Platform>> GetTopPlatforms()
        {
            return await _productService.GetTopPlatformsAsync();
        }

        /// <summary>
        /// Search for product
        /// </summary>
        /// <param name="term" example="product">Name of the product you are looking for</param>
        /// <param name="limit" example="10">Max number of matches</param>
        /// <param name="offset" example="3">Skipped matches</param>
        /// <param name="productParameters">Page</param>
        /// <response code="200">Searching was successful</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("search-product")]
        [ServiceFilter(typeof(PagesValidationFilter))]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductOutputDTO>> SearchProducts(
            string term, int? limit, int? offset, [FromQuery] PageParameters productParameters)
        {
            return await _productService.SearchProductsAsync(term, limit, offset, productParameters);
        }

        /// <summary>
        /// Find product by Id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<ProductOutputDTO> FindProductById(int id)
        {
            return await _productService.FindByIdAsync(id);
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
        public async Task<ProductOutputDTO> AddProduct([FromForm] ProductInputDTO newProduct)
        {
            return await _productService.AddAsync(newProduct);
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
        public async Task<ProductOutputDTO> UpdateProduct([FromForm] ProductInputDTO productDtoUpdate)
        {
            return await _productService.UpdateAsync(productDtoUpdate);
        }

        /// <summary>
        /// Delete product by id
        /// </summary>
        /// <response code="204">Product was deleted</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">You don't have enough rights for this request</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpDelete("id")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            await _productService.DeleteByIdAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Rate product
        /// </summary>
        /// <response code="201">Product was rated</response>
        /// <response code="400">Bad parameter</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPost("rating")]
        [Authorize]
        public async Task<ProductOutputDTO> RateProduct(int rating, int productId)
        {
            return await _ratingService.RateAsync(UserHelper.GetIdByClaims(User.Claims), rating, productId);
        }

        /// <summary>
        /// Delete product rating
        /// </summary>
        /// <response code="204">Rating was deleted</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpDelete("rating")]
        [Authorize]
        public async Task<IActionResult> DeleteProductRating(int productId)
        {
            await _ratingService.DeleteRatingAsync(UserHelper.GetIdByClaims(User.Claims), productId);
            return NoContent();
        }

        /// <summary>
        /// Represents filtered products
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("list")]
        [ServiceFilter(typeof(PagesValidationFilter))]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductOutputDTO>> SearchProductsByFilters([FromQuery] PageParameters pageParameters,
            Genre genre, Rating rating, bool ratingAscending = false, bool priceAscending = true)
        {
            return await _productService.SearchProductsByFiltersAsync(pageParameters, genre, rating, ratingAscending, priceAscending);
        }
    }
}
