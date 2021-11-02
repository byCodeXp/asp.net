using Api.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Seed().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
            return hostBuilder;
        }
    }
}
