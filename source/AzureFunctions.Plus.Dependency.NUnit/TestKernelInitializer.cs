using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ninject.AzureFunctions.Contracts;
using NUnit.Framework;

namespace Ninject.AzureFunctions.NUnit
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
