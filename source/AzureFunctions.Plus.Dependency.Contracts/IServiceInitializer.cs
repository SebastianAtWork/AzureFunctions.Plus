using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace AzureFunctions.Plus.Dependency.Contracts
{
    public interface IServiceInitializer
    {
        IServiceCollection CreateServiceCollection(ILogger log);
    }
}
