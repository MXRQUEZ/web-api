namespace Business.Exceptions
{
    public static class ExceptionMessage
    {
        public const string UserNotFound = "User not found";
        public const string ProductNotFound = "Product not found";
        public const string OrderNotFound = "Order not found";
        public const string ProductIsBought = "This product is bought already";
        public const string WrongEmail = "There is no such mail registered";
        public const string EmailAlreadyRegistered = "This email was already registered";
        public const string WrongPassword = "Password is incorrect";
        public const string WrongCofirmationPassword = "Confirmation password is incorrect";
        public const string Unauthorized = "You are unauthorized";
        public const string BadParameter = "Bad parameter";
        public const string NullValue = "Values can't be empty";
    }
}