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
            using (var collectionContainer = new AutoFeatureContainer<CanDoServiceInitializer>(new FakeLogger()))
            {
                TestServiceInitializer.AssertCanBuildFeature(typeof(CanDoFeature), collectionContainer.Services, nameof(CanDoFeature));
            }
        }

        [Test]
        public void CannotBuildFeature()
        {
            try
            {
                using (var collectionContainer = new AutoFeatureContainer<CanDoServiceInitializer>(new FakeLogger()))
                {
                    TestServiceInitializer.AssertCanBuildFeature(typeof(NoCanDoFeature), collectionContainer.Services, nameof(NoCanDoFeature));
                }
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
