using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiWithRefreshToken.Extensions
{
    public static class IHostExtensions
    {
        public static IHost Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            
            new Seed(roleManager).InvokeAsync().Wait();
            
            return host;
        }
    }
}