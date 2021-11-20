using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.DTO
{
    public sealed class ProductInputDTO : ProductDTO
    {
        [Required]
        public IFormFile Logo { get; set; }

        [Required]
        public IFormFile Background { get; set; }
    }
}
