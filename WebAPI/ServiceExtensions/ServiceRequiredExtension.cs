﻿using Business.Interfaces;
using Business.Services;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WebAPI.ServiceExtensions
{
    public static class ServiceRequiredExtension
    {
        public static void AddRequired(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton(Log.Logger);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped(typeof(IRepository<Product>), typeof(ProductRepository));
        }
    }
}