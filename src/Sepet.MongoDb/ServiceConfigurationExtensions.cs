using System;
using Microsoft.Extensions.DependencyInjection;
using Sepet.Core;

namespace Sepet.MongoDb
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddSepetMongoDb(this IServiceCollection services, Action<SepetMongoDbSettings> options)
        {
            services.Configure<SepetMongoDbSettings>(options);
            
            services.AddScoped<ISepetMongoDbContext, SepetMongoDbContext>();
            services.AddTransient<ISepetRepository, SepetRepository>();
        }
    }
}