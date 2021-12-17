using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.DTO
{
    public sealed class ProductInputDTO : ProductDTO
    {
        [Required(ErrorMessage = "Logo image must be loaded")]
        public IFormFile Logo { get; set; }

        [Required(ErrorMessage = "Background image must be specified")]
        public IFormFile Background { get; set; }
    }
}