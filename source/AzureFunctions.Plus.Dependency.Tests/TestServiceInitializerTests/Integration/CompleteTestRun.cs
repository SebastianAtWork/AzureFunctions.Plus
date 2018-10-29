using System;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.TestNamespace;
using NUnit.Framework;
using TestServiceInitializer = AzureFunctions.Plus.Dependency.NUnit.TestServiceInitializer;

namespace AzureFunctions.Plus.Dependency.Tests.TestServiceInitializerTests.Integration
{
    public class CompleteTestRun
    {
        [TestCaseSource(typeof(FeatureTestDataSource<RootType, Utility.TestServiceInitializer>),nameof(FeatureTestDataSource<RootType,Utility.TestServiceInitializer>.TestCases))]
        public void AbcFeaturesResolveCorrectly(Type featureType, IServiceProvider serviceProvider, string testName)
        {
            TestServiceInitializer.AssertCanBuildFeature(featureType,serviceProvider,testName);
        }
    }
}