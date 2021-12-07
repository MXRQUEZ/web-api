using System;
using Business.Interfaces;
using Business.Services;
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
        public static void AddRequiredCollection(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddMemoryCache();
            services.AddRazorPages();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton(Log.Logger);
            services.AddScoped<PagesValidationFilter>();

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
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderService, OrderService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<Product>), typeof(BaseRepository<Product>));
            services.AddScoped(typeof(IRepository<ProductRating>), typeof(BaseRepository<ProductRating>));
            services.AddScoped(typeof(IRepository<Order>), typeof(BaseRepository<Order>));
        }
    }
}
