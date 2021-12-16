using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebAPI.HealthCheck;

namespace WebAPI.ServiceExtensions
{
    public static class ServiceHealthCheckExtension
    {
        public static IServiceCollection AddHealthCheckSetup(this IServiceCollection services, string connectionString)
        {
            const string healthCheckName = "UsersDB-check";
            const string dbName = "usersdb";

            services.AddHealthChecks()
                .AddCheck(
                    healthCheckName,
                    new SqlConnectionHealthCheck(connectionString),
                    HealthStatus.Unhealthy,
                    new[] { dbName });

            return services;
        }
    }
}
