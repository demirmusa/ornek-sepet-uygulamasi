using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Sepet.TestBase;

namespace Sepet.MongoDb.Tests
{
    public class MongoDbSepetRepositoryTestFixture : FixtureBase, IDisposable
    {
        private string _suankiVeritabani;

        private string _connectionString = "mongodb://localhost:27017";

        protected override void ConfigureServices(IServiceCollection services)
        {
            _suankiVeritabani = $"TestDatabase_{Guid.NewGuid()}";

            //mocklamak yerine daha kolay olacağı için bir test veritabanına bağladım. Her test için veritabanı oluşturup siliyor.
            services.AddSepetMongoDb(option =>
            {
                option.ConnectionString = _connectionString;
                option.Database = _suankiVeritabani;
            });
        }

        protected override void OnBeforePrepareServices()
        {
            TestVeritabaniniSil();
        }

        private void TestVeritabaniniSil()
        {
            if (string.IsNullOrWhiteSpace(_suankiVeritabani)) return;

            var client = new MongoClient(_connectionString);
            client.DropDatabase(_suankiVeritabani);
        }

        public void Dispose()
        {
            TestVeritabaniniSil();
        }
    }

    public class MongoDbSepetRepositoryTests : SepetRepositoryTestBase<MongoDbSepetRepositoryTestFixture>
    {
        public MongoDbSepetRepositoryTests(MongoDbSepetRepositoryTestFixture fixture) : base(fixture)
        {
        }
    }
}