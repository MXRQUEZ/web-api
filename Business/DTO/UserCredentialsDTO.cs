using System.ComponentModel.DataAnnotations;

namespace Business.DTO
{
    public sealed class UserCredentialsDTO
    {
        /// <summary>
        ///     Your email
        /// </summary>
        /// <example>example_mail@gmail.com</example>
        [Required(ErrorMessage = "Email must be specified")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Your password. At least upper case letter, lower case letter, number and special character (e.g. !@#$%^&amp;*) must
        ///     be used
        /// </summary>
        /// <example>_SkJwNif2345</example>
        [Required(ErrorMessage = "Password must be specified")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
            ErrorMessage =
                "Passwords must be at least 6 characters and contain the followings: upper case letter, lower case letter, number " +
                "and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
    }
}