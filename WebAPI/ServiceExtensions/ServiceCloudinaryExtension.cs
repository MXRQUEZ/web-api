using System;
using System.Linq;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.ServiceExtensions
{
    public static class ServiceCloudinaryExtension
    {
        public static void AddCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudinaryOptions = configuration.GetSection("CloudinaryAccountOptions");
            var cloudName = cloudinaryOptions["CloudinaryName"];
            var apiKey = cloudinaryOptions["CloudinaryApiKey"];
            var apiSecret = cloudinaryOptions["CloudinaryApiSecret"];

            if (new[] { cloudName, apiKey, apiSecret }.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Please specify Cloudinary account details!");
            }

            services.AddSingleton(new Cloudinary(new Account(cloudName, apiKey, apiSecret)));
        }
    }
}
