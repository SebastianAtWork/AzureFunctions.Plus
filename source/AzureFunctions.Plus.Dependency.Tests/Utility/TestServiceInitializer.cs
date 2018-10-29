using AzureFunctions.Plus.Dependency.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace AzureFunctions.Plus.Dependency.Tests.Utility
{
    public class TestServiceInitializer : IServiceInitializer
    {
        public IServiceCollection CreateServiceCollection(ILogger log)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(log);
            serviceCollection.AddSingleton<IFakeService, FakeService>();

            return serviceCollection;
        }
    }
}
