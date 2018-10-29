using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.Utility;
using Microsoft.Extensions.Logging;
using Ninject;
using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.Tests.TestKernelInitializerTests
{
    public class AssertCanBuildFeature
    {
        [Test]
        public void CanBuildFeature()
        {
            TestKernelInitializer.AssertCanBuildFeature(typeof(CanDoFeature), new CanDoKernelInitializer().CreateKernelConfiguration(new FakeLogger()), nameof(CanDoFeature));
        }

        [Test]
        public void CannotBuildFeature()
        {
            try
            {
                TestKernelInitializer.AssertCanBuildFeature(typeof(NoCanDoFeature), new CanDoKernelInitializer().CreateKernelConfiguration(new FakeLogger()), nameof(NoCanDoFeature));
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
        internal class CanDoKernelInitializer : IKernelInitializer
        {
            public IKernelConfiguration CreateKernelConfiguration(ILogger log)
            {
                var config = new KernelConfiguration();

                config.Bind<ILogger>().ToConstant(log);

                return config;
            }
        }
    }
}
