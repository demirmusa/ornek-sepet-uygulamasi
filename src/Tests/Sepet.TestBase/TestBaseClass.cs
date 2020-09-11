using Xunit;

namespace Sepet.TestBase
{
    public abstract class TestBaseClass<TFixture> : IClassFixture<TFixture> where TFixture : FixtureBase
    {
        protected readonly TFixture Fixture;

        protected TService GetService<TService>() => (TService) Fixture.ServiceProvider.GetService(typeof(TService));
        
        protected TestBaseClass(TFixture fixture)
        {
            Fixture = fixture;
        }
    }
}