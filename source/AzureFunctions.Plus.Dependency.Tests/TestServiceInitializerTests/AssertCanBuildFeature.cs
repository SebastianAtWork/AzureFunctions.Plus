using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using TestServiceInitializer = AzureFunctions.Plus.Dependency.NUnit.TestServiceInitializer;

namespace AzureFunctions.Plus.Dependency.Tests.TestServiceInitializerTests
{
    public class AssertCanBuildFeature
    {
        [Test]
        public void CanBuildFeature()
        {
            TestServiceInitializer.AssertCanBuildFeature(typeof(CanDoFeature), new CanDoServiceInitializer().CreateServiceCollection(new FakeLogger()).BuildServiceProvider(true), nameof(CanDoFeature));
        }

        [Test]
        public void CannotBuildFeature()
        {
            try
            {
                TestServiceInitializer.AssertCanBuildFeature(typeof(NoCanDoFeature), new CanDoServiceInitializer().CreateServiceCollection(new FakeLogger()).BuildServiceProvider(true), nameof(NoCanDoFeature));
            }
            catch (AssertionException)
            {
                
            }
        }


        internal class CanDoFeature : IFeature
        {
            public CanDoFeature(ILogger log)
            {
                
            }
        }

        internal class NoCanDoFeature : IFeature
        {
            public NoCanDoFeature(IFakeService service)
            {

            }
        }
        internal class CanDoServiceInitializer : IServiceInitializer
        {
            public IServiceCollection CreateServiceCollection(ILogger log)
            {
                var serviceCollection = new ServiceCollection();

                serviceCollection.AddSingleton(log);

                return serviceCollection;
            }
        }
    }
}
