using AutoFixture;
using Business.DTO;

namespace Tests.Extensions.TestData
{
    public static class UserTestData
    {
        public const string UserId = "2";
        public const string UserRole = "user";

        public static readonly string JwtTokenEncoded = new Fixture().Create<string>().Encode();
        
        public static readonly UserCredentialsDTO UserCredentialsDto = new()
        {
            Email = "user@gmail.com",
            Password = "_Aa123456"
        };
    }
}