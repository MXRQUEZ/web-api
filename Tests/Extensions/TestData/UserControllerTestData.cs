using Business.DTO;

namespace Tests.Extensions.TestData
{
    public static class UserControllerTestData
    {
        public const string UserId = "2";
        public const string UserRole = "user";
        public const string UpdateProfileMethodName = "UpdateProfile";
        public const string ChangePasswordMethodName = "ChangePassword";
        public const string GetUserInfoMethodName = "GetUserInfo";

        public static UserDTO ValidUserDto { get; } = new()
        {
            Email = "user@gmail.com",
            Gender = "Male",
            PhoneNumber = "375292678085",
            DateOfBirth = "28/03/2002",
            AddressDelivery = "Minsk",
            UserName = "Sergey"
        };

        public static UserDTO InvalidUserDto { get; } = new()
        {
            Email = "usermail.com",
            Gender = "Male",
            PhoneNumber = "375fsfsffeg*78085",
            DateOfBirth = "28/03.2002",
            AddressDelivery = "Minsk",
            UserName = "Sergey"
        };
    }
}