using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.AzureFunctions.NUnit;
using Ninject.AzureFunctions.Tests.TestNamespace;
using Ninject.AzureFunctions.Tests.Utility;
using NUnit.Framework;

namespace Ninject.AzureFunctions.Tests.TestKernelInitializerTests.Integration
{
    public class CompleteTestRun
    {
        [TestCaseSource(typeof(FeatureTestDataSource<RootType, TestKernel>),nameof(FeatureTestDataSource<RootType,TestKernel>.TestCases))]
        public void AbcFeaturesResolveCorrectly(Type featureType, IKernelConfiguration kernelConfiguration, string testName)
        {
            TestKernelInitializer.AssertCanBuildFeature(featureType,kernelConfiguration,testName);
        }
    }
}