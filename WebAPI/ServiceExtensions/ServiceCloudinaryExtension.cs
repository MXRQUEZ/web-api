using System;
using System.Linq;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.ServiceExtensions
{
    public static class ServiceCloudinaryExtension
    {
        public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudName = configuration.GetValue<string>("CloudinaryAccount:CloudName");
            var apiKey = configuration.GetValue<string>("CloudinaryAccount:ApiKey");
            var apiSecret = configuration.GetValue<string>("CloudinaryAccount:ApiSecret");

            if (new[] { cloudName, apiKey, apiSecret }.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("Please specify Cloudinary account details!");

            services.AddSingleton(new Cloudinary(new Account(cloudName, apiKey, apiSecret)));

            return services;
        }
    }
}
