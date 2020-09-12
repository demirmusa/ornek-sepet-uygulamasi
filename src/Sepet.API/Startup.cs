using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sepet.Application;
using Sepet.InMemory;
using Sepet.MongoDb;

namespace Sepet.API
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
            //test ederken kolaylık olması için bu şekilde eklendi. Appsettingsde StorageType alanını değiştirebilirsiniz.
            if (SepetMongoDbAktifMi)
            {
                services.AddSepetMongoDb(x =>
                    {
                        x.ConnectionString = Configuration["SepetMongoDb:ConnectionString"];
                        x.Database = Configuration["SepetMongoDb:Database"];
                    }
                );
            }
            else if (SepetInMemoryAktifMi)
            {
                services.AddSepetInMemory();
            }
            
            services.AddSepetApplication();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private bool SepetInMemoryAktifMi => Configuration["StorageType"] == "InMemory";
        private bool SepetMongoDbAktifMi => Configuration["StorageType"] == "MongoDb";
        private bool SepetRedisAktifMi => Configuration["StorageType"] == "Redis";
    }
}