using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Added For Gateway Configuration.
            services.AddOcelot();
            
            //Added For Gateway, API's, Masstransmit HealthCheck visualization.
            //If you want check please visit http://localhost:5000/hc-ui/
            services.AddHealthChecks()
                .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 200, tags: new[] { "memory" })
                .AddCheck("Gateway Api", () => HealthCheckResult.Healthy());
            services.AddHealthChecksUI().AddInMemoryStorage(); 
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecksUI(options =>
                {
                    options.UIPath = "/hc-ui";
                });
            });
            app.UseOcelot();
        }
    }
}
