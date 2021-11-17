using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Business.Interfaces;
using Business.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.ServiceExtensions
{
    public static class ServiceJwtTokenExtension
    {
        public static void AddJwtToken(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var jwtAppSettingOptions = configuration.GetSection("JwtIssuerOptions");
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtAppSettingOptions["JwtIssuer"],
                        ValidAudience = jwtAppSettingOptions["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAppSettingOptions["JwtKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddScoped<IJwtGenerator, JwtGenerator>();
        }
    }
}
