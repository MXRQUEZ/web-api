using System.Collections.Generic;
using Business.DTO;
using Business.Interfaces;
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
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
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
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        public List<ProductDTO> SearchProducts(string term, int limit, int offset)
        {
            return _productService.SearchProducts(term, limit, offset);
        }

    }
}
