using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Exceptions
{
    public static class ExceptionMessage
    {
        public const string NotFound = "User not found";
        public const string WrongEmail = "There is no such mail registered";
        public const string ConfirmationFailed = "Confirmation failed";
        public const string WrongPassword = "Password is incorrect";
        public const string WrongCofirmationPassword = "Confirmation password is incorrect";
        public const string Forbidden = "You don't have a rights for this request";
        public const string Failed = "Something went wrong...";
        public const string Unauthorized = "You are unauthorized";
        public const string BadParameter = "Bad parameter";
    }
}
