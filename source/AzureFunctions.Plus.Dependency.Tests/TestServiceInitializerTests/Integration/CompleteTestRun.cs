using System;
using System.Collections.Generic;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.TestNamespace;
using AzureFunctions.Plus.Dependency.Tests.Utility;
using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.Tests.TestServiceInitializerTests.Integration
{
    public class CompleteTestRun : IDisposable
    {
        private static FeatureTestDataSource<RootType, TestServiceInitializer> _dataSource;

        [TestCaseSource(nameof(GenerateCases))]
        public void AbcFeaturesResolveCorrectly(Type featureType, IServiceProvider serviceProvider, string testName)
        {
            TestFeature.AssertCanBuildFeature(featureType, serviceProvider, testName);
        }

        public static IEnumerable<TestCaseData> GenerateCases()
        {
            if (_dataSource == null)
            {
                _dataSource = new FeatureTestDataSource<RootType, TestServiceInitializer>();
            }
            return _dataSource.Create();
        }
        public void Dispose()
        {
            _dataSource?.Dispose();
        }
    }
}