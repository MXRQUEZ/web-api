using System.ComponentModel.DataAnnotations;

namespace Business.DTO
{
    public sealed class UserCredentialsDTO
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression(
            @"^(?=.*[a - z])(?=.*[A - Z])(?=.*[0 - 9])(?=.*[^a - zA - Z0 - 9])\S{6,}$",
            ErrorMessage =
                "Passwords must be at least 6 characters and contain the followings: upper case letter, lower case letter, number and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
    }
}