using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WebAPI.ServiceExtensions;

namespace WebAPI
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext();
            services.AddIdentity();
            services.AddCloudinary(Configuration);
            services.AddJwtToken(Configuration);
            services.AddHealthCheckSetup(Configuration.GetConnectionString("DefaultConnection"));
            services.AddSwaggerSetup();
            services.AddRequiredCollection();
            services.AddGzipSetup();
            services.AddServiceFilters();
            services.AddHelpers();
            services.AddServices();
            services.AddRepositories();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error");
            app.UseSwaggerSetup();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            app.UseResponseCompression();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/hc");
            });
        }
    }
}
