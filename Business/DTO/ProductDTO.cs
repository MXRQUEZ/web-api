using System.ComponentModel.DataAnnotations;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.DTO
{
    [Index(nameof(Name), IsUnique = true)]
    public class ProductDTO
    {
        /// <summary>
        /// Product name
        /// </summary>
        /// <example>"My Best Game"</example>
        
        [Required(ErrorMessage = "Name must be specified")]
        public string Name { get; set; }

        /// <summary>
        /// Product platform. Use numbers to chose platform
        /// </summary>

        [Required]
        [EnumDataType(typeof(Platform))]
        public Platform Platform { get; set; }

        /// <summary>
        /// Product date of creation
        /// </summary>
        /// <example>dd/mm/yyyy</example>
        
        [Required(ErrorMessage = "Date of creation must be specified")]
        [RegularExpression(@"^([012]\d|30|31)/(0\d|10|11|12)/\d{4}$", ErrorMessage = "Date must be dd/mm/yyyy")]
        public string DateCreated { get; set; }

        /// <summary>
        /// Product genre
        /// </summary>

        [Required]
        [EnumDataType(typeof(Genre))]
        public Genre Genre { get; set; }

        /// <summary>
        /// Product rating
        /// </summary>

        [Required]
        [EnumDataType(typeof(Rating))]
        public Rating Rating { get; set; }

        /// <summary>
        /// Product price(in dollars equivalent)
        /// </summary>

        [Required(ErrorMessage = "Price must be specified")]
        public int Price { get; set; }

        /// <summary>
        /// Products in storage
        /// </summary>

        [Required(ErrorMessage = "Count must be specified")]
        public int Count { get; set; }
    }
}
