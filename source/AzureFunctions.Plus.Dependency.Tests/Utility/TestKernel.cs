using AzureFunctions.Plus.Dependency.Contracts;
using Microsoft.Extensions.Logging;
using Ninject;

namespace AzureFunctions.Plus.Dependency.Tests.Utility
{
    public class TestKernel : IKernelInitializer
    {
        public TestKernel()
        {
            
        }
        public IKernelConfiguration CreateKernelConfiguration(ILogger log)
        {
            var config = new KernelConfiguration();

            config.Bind<IFakeService>().To<FakeService>().InSingletonScope();
            config.Bind<ILogger>().ToConstant(log);

            return config;
        }
    }
}
