using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Ninject.AzureFunctions.Contracts;
using Ninject.AzureFunctions.Tests.ExecuteFeatureTests;

namespace Ninject.AzureFunctions.Tests.Utility
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
