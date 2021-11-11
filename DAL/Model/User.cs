using Microsoft.AspNetCore.Identity;

namespace DAL.Model
{
    public sealed class User : IdentityUser<int>
    {
        public int Age { get; set; }
        public string AddressDelivery { get; set; }
    }
}
