﻿using Microsoft.Extensions.DependencyInjection;

namespace Sepet.TestBase
{
    public abstract class FixtureBase
    {
        public ServiceProvider ServiceProvider { get; private set; }
        
        protected abstract void ConfigureServices(IServiceCollection services);
        
        protected FixtureBase()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}