using Microsoft.AspNetCore.Identity;

namespace DAL.Models.Entities
{
    public sealed class User : IdentityUser<int>
    {
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string AddressDelivery { get; set; }
    }
}
