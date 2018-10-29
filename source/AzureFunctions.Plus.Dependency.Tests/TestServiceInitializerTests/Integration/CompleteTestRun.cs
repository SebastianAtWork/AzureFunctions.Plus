using System;
using System.Collections.Generic;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.TestNamespace;
using AzureFunctions.Plus.Dependency.Tests.Utility;
using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.Tests.TestServiceInitializerTests.Integration
{
    public class CompleteTestRun : AllFeaturesCanBeResolvedBaseTest<RootType, TestServiceInitializer,FakeEnvironmentInitializer>
    {
    }
}