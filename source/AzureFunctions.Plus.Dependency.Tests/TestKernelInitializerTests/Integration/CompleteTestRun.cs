using System;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.TestNamespace;
using AzureFunctions.Plus.Dependency.Tests.Utility;
using Ninject;
using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.Tests.TestKernelInitializerTests.Integration
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