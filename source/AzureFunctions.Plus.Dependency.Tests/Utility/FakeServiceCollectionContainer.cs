using System;
using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.NUnit;
using Microsoft.Extensions.DependencyInjection;


namespace AzureFunctions.Plus.Dependency.Tests.Utility
{
        public class FakeServiceCollectionContainer : IAutoFeatureContainer
        {

            public FakeServiceCollectionContainer()
            {
                var serviceCollection = new TestServiceInitializer()
                    .CreateServiceCollection(new FakeLogger());
                Services = serviceCollection.BuildServiceProvider();
            }

            public IServiceProvider Services { get; }
        }
}
