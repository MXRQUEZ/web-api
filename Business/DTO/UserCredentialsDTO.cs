using System.ComponentModel.DataAnnotations;

namespace Business.DTO
{
    public sealed class UserCredentialsDTO
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$",
            ErrorMessage =
                "Passwords must be at least 6 characters and contain the followings: upper case letter, lower case letter, number and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
    }
}