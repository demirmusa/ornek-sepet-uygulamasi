using Microsoft.Extensions.DependencyInjection;
using Sepet.Core;

namespace Sepet.InMemory
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddSepetInMemory(this IServiceCollection services)
        {
            services.AddSingleton<ISepetRepository, SepetRepository>();
        }
    }
}