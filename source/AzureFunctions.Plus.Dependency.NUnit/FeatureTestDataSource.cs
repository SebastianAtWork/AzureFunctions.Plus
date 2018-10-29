using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject.AzureFunctions.Contracts;
using NUnit.Framework;

namespace Ninject.AzureFunctions.NUnit
{
    public class FeatureTestDataSource<TRootType,TKernelInitializer> where TKernelInitializer : IKernelInitializer
    {
        
        // ReSharper disable once UnusedMember.Global
        public static IEnumerable TestCases
        {
            get { return Create(); }
        }

        public static IEnumerable<TestCaseData> Create()
        {
            var namespaceRootType = typeof(TRootType);
            var rootNamespace = namespaceRootType.Namespace;
            var typesOfNamespace =
                namespaceRootType.Assembly.DefinedTypes.Where(t => t?.Namespace?.StartsWith(rootNamespace)??false);
            var featureTypes = typesOfNamespace.Where(t => t.GetInterface(nameof(IFeature)) != null);
            var kernelInitializer = Activator.CreateInstance<TKernelInitializer>();
            return featureTypes.Select(f => ConvertToTestData(f, rootNamespace, kernelInitializer.CreateKernelConfiguration(new FakeLogger())));
        }

        private static TestCaseData ConvertToTestData(TypeInfo featureType, string rootNamespace,
            IKernelConfiguration kernelConfiguration)
        {
            var testName = "";
            var relativeNamespace =
                featureType.Namespace.Substring(rootNamespace.Length,
                    featureType.Namespace.Length - (rootNamespace.Length));
            relativeNamespace = relativeNamespace.TrimStart('.');
            if (relativeNamespace.Length > 0)
            {
                testName = $"{relativeNamespace}.{featureType.Name}";
            }
            else
            {
                testName = featureType.Name;
            }

            testName = "Features." + testName;
            return new TestCaseData(featureType,kernelConfiguration,testName)
            {
                TestName = testName
            };
        }
    }
}
