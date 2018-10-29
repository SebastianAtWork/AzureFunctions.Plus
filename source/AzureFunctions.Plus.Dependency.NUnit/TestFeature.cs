using System;

using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.NUnit
{
    public static class TestFeature
    {
        public static void AssertCanBuildFeature(Type featureType, IServiceProvider serviceProvider, string testName)
        {
            try
            {
                serviceProvider.GetService(featureType);
            }
            catch (Exception e)
            {
                throw new AssertionException($"Cannot resolve {testName}", e);
            }
        }
    }
}
