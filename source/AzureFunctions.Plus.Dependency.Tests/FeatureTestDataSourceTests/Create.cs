using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ninject.AzureFunctions.Contracts;
using Ninject.AzureFunctions.NUnit;
using Ninject.AzureFunctions.Tests.TestNamespace;
using Ninject.AzureFunctions.Tests.TestNamespace.C;
using NUnit.Framework;

namespace Ninject.AzureFunctions.Tests.FeatureTestDataSourceTests
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
