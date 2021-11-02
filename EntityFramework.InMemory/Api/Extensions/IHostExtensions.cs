using Api.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.Extensions
{
    public static class IHostExtensions
    {
        public static IHost Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<DataContext>();
            new Seed(context).Invoke();

            return host;
        }
    }
}
