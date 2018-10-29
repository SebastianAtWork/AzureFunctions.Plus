using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AzureFunctions.Plus.Dependency.Contracts;

using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.NUnit
{
    public class FeatureTestDataSource<TRootType,TServiceInitializer> where TServiceInitializer : IServiceInitializer
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
            var autoFeatureContainer = new AutoFeatureContainer<TServiceInitializer>(new FakeLogger());
            return featureTypes.Select(f => ConvertToTestData(f, rootNamespace, autoFeatureContainer.Services));
        }

        private static TestCaseData ConvertToTestData(TypeInfo featureType, string rootNamespace,
            IServiceProvider serviceProvider)
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
            return new TestCaseData(featureType,serviceProvider,testName)
            {
                TestName = testName
            };
        }
    }
}
