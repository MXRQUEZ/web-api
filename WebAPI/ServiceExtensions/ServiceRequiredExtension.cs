using System;
using Business.Helpers;
using Business.Interfaces;
using Business.Services;
using DAL.Caching;
using DAL.Interfaces;
using DAL.Models.Entities;
using DAL.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WebAPI.Filters;

namespace WebAPI.ServiceExtensions
{
    public static class ServiceRequiredExtension
    {
        public static IServiceCollection AddRequiredCollection(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddMemoryCache();
            services.AddRazorPages();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton(Log.Logger);
            services.AddScoped<PagesFilerAttribute>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }

        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ICloudinaryManager, CloudinaryManager>();

            return services;
        }

        public static IServiceCollection AddGzipSetup(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = new[]
                {
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json"
                };
            });

            return services;
        }

        public static IServiceCollection AddServiceFilters(this IServiceCollection services)
        {
            services.AddScoped<PagesFilerAttribute>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IProductManager), typeof(ProductManager));
            services.AddScoped(typeof(IRatingManager), typeof(RatingManager));
            services.AddScoped(typeof(IOrderManager), typeof(OrderManager));
            services.AddScoped(typeof(ICacheManager<User>), typeof(CacheManager<User>));

            return services;
        }
    }
}