using System;
using Ninject;
using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.NUnit
{
    public static class TestKernelInitializer
    {
        public static void AssertCanBuildFeature(Type featureType, IKernelConfiguration kernelConfiguration, string testName)
        {
            using (var kernel = kernelConfiguration.BuildReadonlyKernel())
            {
                try
                {
                    kernel.Get(featureType);
                }
                catch (ActivationException e)
                {
                    throw new AssertionException($"Cannot resolve {testName}", e);
                }
            }
        }
    }
}
