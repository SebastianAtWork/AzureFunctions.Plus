using System;
using System.Collections.Generic;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.TestNamespace;
using NUnit.Framework;
using TestServiceInitializer = AzureFunctions.Plus.Dependency.NUnit.TestServiceInitializer;

namespace AzureFunctions.Plus.Dependency.Tests.TestServiceInitializerTests.Integration
{
    public class CompleteTestRun
    {
        [TestCaseSource(nameof(GenerateCases))]
        public void AbcFeaturesResolveCorrectly(Type featureType, IServiceProvider serviceProvider, string testName)
        {
            TestServiceInitializer.AssertCanBuildFeature(featureType,serviceProvider,testName);
        }

        public static IEnumerable<TestCaseData> GenerateCases()
        {
            using (var dataSource = new FeatureTestDataSource<RootType, Utility.TestServiceInitializer>())
            {
                return dataSource.Create();
            }
        }
    }
}