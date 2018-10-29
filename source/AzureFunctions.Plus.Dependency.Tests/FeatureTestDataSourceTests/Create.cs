using System;
using System.Linq;
using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.TestNamespace;
using AzureFunctions.Plus.Dependency.Tests.TestNamespace.C;
using Microsoft.Extensions.Logging;
using Ninject;
using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.Tests.FeatureTestDataSourceTests
{
    public class Create
    {
        [Test]
        public void CreateTestDataForEveryIFeatureInNamespace()
        {
            var testData = FeatureTestDataSource<RootType, FakeKernelInitializer>.Create();
            Assert.That(testData.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CorrectlyCreateTestData()
        {
            var testData = FeatureTestDataSource<CFeature, FakeKernelInitializer>.Create().Single();

            Assert.That(testData.TestName, Is.EqualTo("Features.CFeature"));
            Assert.That((testData.Arguments[0] as Type)?.Name, Is.EqualTo(nameof(CFeature)));
            Assert.That((testData.Arguments[1] as IKernelConfiguration), Is.Not.Null);
            Assert.That((testData.Arguments[2] as string), Is.EqualTo("Features.CFeature"));
        }


        internal class FakeKernelInitializer :  IKernelInitializer
        {
            public IKernelConfiguration CreateKernelConfiguration(ILogger log)
            {
                return new KernelConfiguration();
            }
        }
    }
}
