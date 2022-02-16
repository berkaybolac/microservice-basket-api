using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((host, config) =>
                    {
                        config.AddJsonFile($"ocelot.{host.HostingEnvironment.EnvironmentName.ToLower()}.json", optional: false, reloadOnChange: true); //Added for docker-compose.
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
