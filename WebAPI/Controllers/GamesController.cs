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
    public class GamesController : BaseController
    {
        private readonly IProductService _productService;

        public GamesController(IProductService productService, ILogger logger) : base(logger)
        {
            _productService = productService;
        }

        /// <summary>
        /// Represents top 3 popular platforms
        /// </summary>
        /// <response code="200">Top platforms were successfully changed</response>
        /// <response code="500">Can't represent it right now, come back later</response>
        [HttpGet("top-platforms")]
        [AllowAnonymous]
        public string GetTopPlatforms()
        {
            return _productService.GetTopPlatforms();
        }

        /// <summary>
        /// Search for product
        /// </summary>
        /// <param name="term" example="product">Name of the product you are looking for</param>
        /// <param name="limit" example="10">Max number of matches</param>
        /// <param name="offset" example="3">Skipped matches</param>
        /// <param name="productParameters">Page</param>
        /// <response code="200">Searching was successful</response>
        /// <response code="400">Bad parameter</response>
        /// <response code="500">Can't get this request right now, come back later</response>
        [HttpGet("search-product")]
        [AllowAnonymous]
        [ServiceFilter(typeof(PagesValidationFilter))]
        public List<ProductOutputDTO> SearchProducts([FromQuery] PageParameters productParameters,
            string term, int? limit, int? offset)
        {
            return _productService.SearchProducts(term, limit, offset, productParameters);
        }

        /// <summary>
        /// Find product by Id
        /// </summary>
        /// <response code="200">Searching was successful</response>
        /// <response code="400">Bad parameter</response>
        /// <response code="500">Can't get this request right now, come back later</response>
        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<ProductOutputDTO> FindProductById(int id)
        {
            return await _productService.FindByIdAsync(id);
        }

        /// <summary>
        /// Add product
        /// </summary>
        /// <response code="200">Searching was successful OK</response>
        /// <response code="400">Bad parameter</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Can't get this request right now, come back later</response>
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<ProductOutputDTO> AddProduct([FromForm] ProductInputDTO newProduct)
        {
            return await _productService.AddAsync(newProduct);
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <response code="200">Searching was successful OK</response>
        /// <response code="400">Bad parameter</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Can't get this request right now, come back later</response>
        [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<ProductOutputDTO> UpdateProduct([FromForm] ProductInputDTO productDtoUpdate)
        {
            return await _productService.UpdateAsync(productDtoUpdate);
        }

        /// <summary>
        /// Delete product by id
        /// </summary>
        /// <response code="200">Searching was successful OK</response>
        /// <response code="400">Bad parameter</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Can't get this request right now, come back later</response>
        [HttpDelete("id")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            await _productService.DeleteByIdAsync(id);
            return Ok();
        }

        /// <summary>
        /// Rate product
        /// </summary>
        /// <response code="200">Searching was successful OK</response>
        /// <response code="400">Bad parameter</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Can't get this request right now, come back later</response>
        [HttpPost("rating")]
        [Authorize]
        public async Task<ProductOutputDTO> RateProduct(int rating, string productName)
        {
            return await _productService.RateAsync(UserHelper.GetIdByClaims(User.Claims), rating, productName);
        }

        /// <summary>
        /// Delete product rating
        /// </summary>
        /// <response code="204">Deleted</response>
        /// <response code="400">Bad parameter</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Can't get this request right now, come back later</response>
        [HttpDelete("rating")]
        [Authorize]
        public async Task<IActionResult> DeleteProductRating(string productName)
        {
            await _productService.DeleteRatingAsync(UserHelper.GetIdByClaims(User.Claims), productName);
            return NoContent();
        }

        [HttpGet("list")]
        [AllowAnonymous]
        [ServiceFilter(typeof(PagesValidationFilter))]
        public List<ProductOutputDTO> SearchProductsByFilters([FromQuery] PageParameters pageParameters,
            Genre genre, Rating rating, bool ratingAscending = false, bool priceAscending = true)
        {
            return _productService.SearchProductsByFilters(pageParameters, genre, rating, ratingAscending, priceAscending);
        }
    }
}
