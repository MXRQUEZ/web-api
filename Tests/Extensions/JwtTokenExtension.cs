using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Tests.Extensions
{
    public static class JwtTokenExtension
    {
        public static string Encode(this string token)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            token = WebEncoders.Base64UrlEncode(tokenBytes);

            return token;
        }
    }
}