using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebAPI.HealthCheck;

namespace WebAPI.ServiceExtensions
{
    public static class ServiceHealthCheckExtension
    {
        public static void AddHealthCheckSetup(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecks()
                .AddCheck(
                    "UsersDB-check",
                    new SqlConnectionHealthCheck(connectionString),
                    HealthStatus.Unhealthy,
                    new[] { "usersdb" });
        }
    }
}
