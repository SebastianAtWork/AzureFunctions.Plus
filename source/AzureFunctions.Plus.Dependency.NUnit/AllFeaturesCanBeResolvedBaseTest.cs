using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureFunctions.Plus.Dependency.Contracts;
using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.NUnit
{
    public class AllFeaturesCanBeResolvedBaseTest<TRootType,TServiceInitializer> : IDisposable where TRootType: class where TServiceInitializer : IServiceInitializer
    {
        private static FeatureTestDataSource<TRootType, TServiceInitializer> _dataSource;

        [TestCaseSource(nameof(GenerateCases))]
        public void AbcFeaturesResolveCorrectly(Type featureType, IServiceProvider serviceProvider, string testName)
        {
            TestFeature.AssertCanBuildFeature(featureType, serviceProvider, testName);
        }

        public static IEnumerable<TestCaseData> GenerateCases()
        {
            if (_dataSource == null)
            {
                _dataSource = new FeatureTestDataSource<TRootType, TServiceInitializer>();
            }
            return _dataSource.Create();
        }
        public void Dispose()
        {
            _dataSource?.Dispose();
        }
    }
}
