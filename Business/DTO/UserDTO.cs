using System.ComponentModel.DataAnnotations;

namespace Business.DTO
{
    public sealed class UserDTO
    {
        [RegularExpression(@"^([012]\d|30|31)/(0\d|10|11|12)/\d{4}$", ErrorMessage = "Date must be dd/mm/yyyy")]
        public string DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Number must contain numbers only")]
        public string PhoneNumber { get; set; }

        public string AddressDelivery { get; set; }

        public string UserName { get; set; }
    }
}
