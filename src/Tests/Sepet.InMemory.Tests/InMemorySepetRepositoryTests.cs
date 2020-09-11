using Microsoft.Extensions.DependencyInjection;
using Sepet.TestBase;

namespace Sepet.InMemory.Tests
{
    public class InMemorySepetTestFixture : FixtureBase
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSepetInMemory();
        }
    }

    public class InMemorySepetRepositoryTests : SepetRepositoryTestBase<InMemorySepetTestFixture>
    {
        public InMemorySepetRepositoryTests(InMemorySepetTestFixture fixture) : base(fixture)
        {
        }
    }
}