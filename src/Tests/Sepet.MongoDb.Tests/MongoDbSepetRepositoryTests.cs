using System;
using Microsoft.Extensions.DependencyInjection;
using Sepet.TestBase;

namespace Sepet.MongoDb.Tests
{
    
    public class MongoDbSepetRepositoryTestFixture : FixtureBase
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            //TODO: mongodb katmanını test veritabanı ile ekle
            throw new NotImplementedException();
        }
    }

    public class MongoDbSepetRepositoryTests : SepetRepositoryTestBase<MongoDbSepetRepositoryTestFixture>
    {
        public MongoDbSepetRepositoryTests(MongoDbSepetRepositoryTestFixture fixture) : base(fixture)
        {
        }
    }
}