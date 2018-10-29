using Microsoft.Extensions.Logging;
using Ninject;

namespace AzureFunctions.Plus.Dependency.Contracts
{
    public interface IKernelInitializer
    {
        IKernelConfiguration CreateKernelConfiguration(ILogger log);
    }
}
