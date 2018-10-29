using System;
using System.Threading.Tasks;
using System.Web.Http;
using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.Features;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TestServiceInitializer = AzureFunctions.Plus.Dependency.Tests.Utility.TestServiceInitializer;

namespace AzureFunctions.Plus.Dependency.Tests.ExecuteFeatureTests
{
    public class ExecuteOkTests
    {
        [Test]
        public async Task ExecutesOkFeature()
        {
            using (var collectionContainer = new AutoFeatureContainer<TestServiceInitializer>(new FakeLogger()))
            {
                var fakeService = collectionContainer.Services.GetService<IFakeService>();

                var result = await ExecuteFeature.ExecuteOk<OkFeature, string>(collectionContainer, f => f.Execute("Test")) as OkObjectResult;

                Assert.That(fakeService.Value, Is.EqualTo("Test"));
                Assert.That(result, Is.Not.Null);
                var resultContent = result.Value;
                Assert.That(resultContent, Is.EqualTo("Bla"));
            }
                
        }

        [Test]
        public async Task ExecutesOkFeatureThrowsException()
        {
            using (var collectionContainer = new AutoFeatureContainer<TestServiceInitializer>(new FakeLogger()))
            {
                var result = await ExecuteFeature.ExecuteOk<OkFeatureWithException, string>(collectionContainer, f => f.Execute("Test"));

                Assert.That(result.GetType(), Is.EqualTo(typeof(InternalServerErrorResult)));
            }
        }

       

        internal class OkFeature : IFeature
        {
            private readonly IFakeService _fakeService;

            public OkFeature(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task<string> Execute(string value)
            {
                _fakeService.SetValue(value);
                return await Task.FromResult("Bla");
            }
        }
        internal class OkFeatureWithException : IFeature
        {
            private readonly IFakeService _fakeService;

            public OkFeatureWithException(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task<string> Execute(string value)
            {
                await Task.Run(()=>throw new ArgumentException());
                return value;
            }
        }

      
    }
}
