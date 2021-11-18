using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
        /// Changes user`s password
        /// </summary>
        /// <param name="term" example="product">Name of the product you are looking for</param>
        /// <param name="limit" example="10">Max number of matches</param>
        /// <param name="offset" example="3">Skipped matches</param>
        /// <response code="200">Searching was successful</response>
        /// <response code="500">Can't get this request right now, come back later</response>
        [HttpGet("search-product")]
        [AllowAnonymous]
        public List<ProductOutputDTO> SearchProducts(string term, int limit, int offset)
        {
            return _productService.SearchProducts(term, limit, offset);
        }

        /// <summary>
        /// Find product by Id
        /// </summary>
        /// <response code="200">Searching was successful OK</response>
        /// <response code="400">Bad parameter</response>
        /// <response code="500">Can't get this request right now, come back later</response>
        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<ProductOutputDTO> FindProductById(int id)
        {
            return await _productService.FindByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ProductOutputDTO> AddProduct([FromForm] ProductInputDTO newProduct)
        {
            return await _productService.AddAsync(newProduct);
        }

        [HttpPut]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ProductOutputDTO> UpdateProduct([FromForm] ProductInputDTO productDtoUpdate)
        {
            return await _productService.UpdateAsync(productDtoUpdate);
        }

        [HttpDelete("id")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            await _productService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}
