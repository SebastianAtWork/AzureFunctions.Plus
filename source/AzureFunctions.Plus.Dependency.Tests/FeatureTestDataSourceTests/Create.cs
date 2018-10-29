using System;
using System.Linq;
using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.TestNamespace;
using AzureFunctions.Plus.Dependency.Tests.TestNamespace.C;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.Tests.FeatureTestDataSourceTests
{
    public class Create
    {
        [Test]
        public void CreateTestDataForEveryIFeatureInNamespace()
        {
            using (var dataSource = new FeatureTestDataSource<RootType, FakeServiceInitializer>())
            {
                var testData = dataSource.Create();
                Assert.That(testData.Count(), Is.EqualTo(3));
            }
            
        }

        [Test]
        public void CorrectlyCreateTestData()
        {
            using (var dataSource = new FeatureTestDataSource<CFeature, FakeServiceInitializer>())
            {
                var testData = dataSource.Create().Single();

                Assert.That(testData.TestName, Is.EqualTo("Features.CFeature"));
                Assert.That((testData.Arguments[0] as Type)?.Name, Is.EqualTo(nameof(CFeature)));
                Assert.That((testData.Arguments[1] as IServiceProvider), Is.Not.Null);
                Assert.That((testData.Arguments[2] as string), Is.EqualTo("Features.CFeature"));
            }
        }


        internal class FakeServiceInitializer :  IServiceInitializer
        {

            IServiceCollection IServiceInitializer.CreateServiceCollection(ILogger log)
            {
                return new ServiceCollection();
            }
        }
    }
}
