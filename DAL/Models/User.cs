using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public sealed class User : IdentityUser<int>
    {
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string AddressDelivery { get; set; }
        public List<ProductRating> Ratings { get; set; } = new();
    }
}
