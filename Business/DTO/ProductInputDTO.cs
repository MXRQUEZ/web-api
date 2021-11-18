using Microsoft.AspNetCore.Http;

namespace Business.DTO
{
    public sealed class ProductInputDTO : ProductDTO
    {
        /// <summary>
        /// Link to logo picture
        /// </summary>
       
        public IFormFile Logo { get; set; }

        /// <summary>
        /// Link to background picture
        /// </summary>

        public IFormFile Background { get; set; }
    }
}
