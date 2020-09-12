using Microsoft.Extensions.DependencyInjection;

namespace Sepet.Application
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddSepetApplication(this IServiceCollection services)
        {
            services.AddTransient<IUrunApplicationService, UrunApplicationService>();
            services.AddTransient<IStokKontrolApplicationService, StokKontrolApplicationService>();
            services.AddTransient<ISepetApplicationService, SepetApplicationService>();
        }
    }
}
