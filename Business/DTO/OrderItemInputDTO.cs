using System.ComponentModel.DataAnnotations;

namespace Business.DTO
{
    public sealed class OrderItemInputDTO
    {
        [Required(ErrorMessage = "Product Id must be specified")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product amount must be specified")]
        public int Amount { get; set; }
    }
}
