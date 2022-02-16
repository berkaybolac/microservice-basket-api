using Basket.Domain.Settings;
using Basket.Infrastructure.Interfaces;
using Basket.Infrastructure.Logger;
using Basket.Infrastructure.Repository;
using Basket.Infrastructure.SendEndpointProvider;
using Basket.Infrastructure.Services;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Basket.API
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
            //Added for MongoDb configuration.
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            services.AddSingleton<IMongoDbSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            });
            
            //Added for Successfully Insert Event
            //Maybe we have an another Subscriber who listen this queue.
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                });
            });
            services.AddMassTransitHostedService();
            
            //Added for Anonymous Creation Inside Controller.
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped(typeof(ILogHelper<,>), typeof(LogHelper<,>));
            services.AddScoped(typeof(ISendEndpointProviderHelper<>), typeof(SendEndPointProviderHelper<>));
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddControllers();
            
            //Added For Gateway's, API's, Masstransmit's HealthCheck visualization.
            //If you want check please visit http://localhost:5000/hc-ui/
            services.AddHealthChecks()
                .AddCheck("Basket Api", () => HealthCheckResult.Healthy());
            
            //Added For API's endpoint visualization.
            //If you want check please visit http://localhost:5001/swagger/index.html
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.Api Service", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.Api Service"));
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = r => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
