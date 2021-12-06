using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WebAPI.ServiceExtensions
{
    public static class ServiceSwaggerExtension
    {
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo {Title = "Auth", Version = "v1"});
                setup.SwaggerDoc("v2", new OpenApiInfo {Title = "User", Version = "v2"});
                setup.SwaggerDoc("v3", new OpenApiInfo {Title = "Games", Version = "v3"});
                setup.SwaggerDoc("v4", new OpenApiInfo {Title = "Home", Version = "v4"});
                setup.SwaggerDoc("v5", new OpenApiInfo { Title = "Order", Version = "v5" });
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {jwtSecurityScheme, Array.Empty<string>()}
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setup.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerSetup(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "User");
                c.SwaggerEndpoint("/swagger/v3/swagger.json", "Games");
                c.SwaggerEndpoint("/swagger/v4/swagger.json", "Home");
                c.SwaggerEndpoint("/swagger/v5/swagger.json", "Order");
            });

            return app;
        }
    }
}
