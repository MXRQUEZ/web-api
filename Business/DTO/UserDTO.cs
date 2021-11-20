using System.ComponentModel.DataAnnotations;

namespace Business.DTO
{
    public sealed class UserDTO
    {
        /// <summary>
        /// Your birthday date
        /// </summary>
        /// <example>dd/mm/yyyy</example>
        [Required]
        [RegularExpression(@"^([012]\d|30|31)/(0\d|10|11|12)/\d{4}$", ErrorMessage = "Date must be dd/mm/yyyy")]
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Your gender
        /// </summary>
        /// <example>["Male", "Female"] or else</example>
        [Required]
        public string Gender { get; set; }

        /// <summary>
        /// Your email
        /// </summary>
        /// <example>example_mail@gmail.com</example>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Your phone number using numbers only
        /// </summary>
        /// <example>37529XXXXXXX</example>
        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Phone number must contain numbers only")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Your delivery address
        /// </summary>
        /// <example>Minsk, Vyazenceva 43/12</example>
        [Required]
        public string AddressDelivery { get; set; }

        /// <summary>
        /// Your nickname without whitespaces
        /// </summary>
        /// <example>"ThisIsMyNickName"</example>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.-]*$", ErrorMessage = "No whitespaces!")]
        public string UserName { get; set; }
    }
}
