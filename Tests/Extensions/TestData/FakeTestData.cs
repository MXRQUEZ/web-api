﻿using AutoMapper;
using Business.Interfaces;
using DAL.Interfaces;
using DAL.Models.Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace Tests.Extensions.TestData
{
    public static class FakeTestData
    {
        public static readonly UserManager<User> FakeUserManager = A.Fake<UserManager<User>>();

        public static readonly IJwtGenerator FakeJwtGenerator = A.Fake<IJwtGenerator>();
        public static readonly IMapper FakeMapper = A.Fake<IMapper>();
        public static readonly IEmailSender FakeEmailSender = A.Fake<IEmailSender>();

        public static readonly IGenericRepository<Order> FakeOrderRepository = A.Fake<IGenericRepository<Order>>();
        public static readonly IGenericRepository<Product> FakeProductRepository = A.Fake<IGenericRepository<Product>>();
        public static readonly IGenericRepository<ProductRating> FakeRatingRepository = A.Fake<IGenericRepository<ProductRating>>();
    }
}