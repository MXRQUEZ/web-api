using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Business.Helpers
{
    public static class UserHelper
    {
        public static string GetUserId(this IEnumerable<Claim> claims) =>
            claims.Where(c => c.Type == "nameid").Select(c => c.Value).SingleOrDefault();
    }
}
