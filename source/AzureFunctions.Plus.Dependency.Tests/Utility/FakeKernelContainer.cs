using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.NUnit;
using Ninject;

namespace AzureFunctions.Plus.Dependency.Tests.Utility
{
        public class FakeKernelContainer : IAutoFeatureContainer
        {
            public IReadOnlyKernel Kernel { get; }

            public FakeKernelContainer()
            {
                var kernelConfig = new TestKernel()
                    .CreateKernelConfiguration(new FakeLogger());
                Kernel = kernelConfig.BuildReadonlyKernel();
            }
        }
}
