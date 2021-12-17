using AutoMapper;
using Business.Interfaces;
using DAL.Interfaces;
using DAL.Models.Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace Tests.Extensions.TestData
{
    public static class FakeTestData
    {
        public static readonly IJwtGenerator FakeJwtGenerator = A.Fake<IJwtGenerator>();
        public static readonly IMapper FakeMapper = A.Fake<IMapper>();
        public static readonly IEmailSender FakeEmailSender = A.Fake<IEmailSender>();
        public static readonly ICloudinaryManager FakeCloudinary = A.Fake<ICloudinaryManager>();

        public static readonly UserManager<User> FakeUserManager = A.Fake<UserManager<User>>();
        public static readonly IOrderManager FakeOrderManager = A.Fake<IOrderManager>();
        public static readonly IProductManager FakeProductManager = A.Fake<IProductManager>();
        public static readonly IRatingManager FakeRatingManager = A.Fake<IRatingManager>();
    }
}