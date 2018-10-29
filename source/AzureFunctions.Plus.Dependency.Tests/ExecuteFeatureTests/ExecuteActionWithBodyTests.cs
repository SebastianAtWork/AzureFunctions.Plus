using System;
using System.Threading.Tasks;
using System.Web.Http;
using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.Features;
using AzureFunctions.Plus.Dependency.NUnit;
using AzureFunctions.Plus.Dependency.Tests.FeatureTestDataSourceTests;
using AzureFunctions.Plus.Dependency.Tests.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TestServiceInitializer = AzureFunctions.Plus.Dependency.Tests.Utility.TestServiceInitializer;

namespace AzureFunctions.Plus.Dependency.Tests.ExecuteFeatureTests
{
    public class ExecuteActionWithBodyTests
    {
        [Test]
        public async Task ExecutesActionWithBodyFeature()
        {
            using (var collectionContainer = new AutoFeatureContainer<TestServiceInitializer>(new FakeLogger()))
            using (var request = new FakeHttpRequest<string>("Test"))
            {
                var fakeService = collectionContainer.Services.GetService<IFakeService>();

                var result =
                    await ExecuteFeature.ExecuteActionWithBody<ActionWithBodyFeature, string>(collectionContainer, request, (f, b) => f.Execute(b)) as
                        OkObjectResult;

                Assert.That(fakeService.Value, Is.EqualTo("Test"));
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Value, Is.EqualTo("Bla"));
            }
        }

        [Test]
        public async Task ExecutesActionWithBodyFeatureThrowsException()
        {
            using (var collectionContainer = new AutoFeatureContainer<TestServiceInitializer>(new FakeLogger()))
            using (var request = new FakeHttpRequest<string>("Test"))
            {
                var result =
                    await ExecuteFeature.ExecuteActionWithBody<ActionWithBodyFeatureWithException, string>(collectionContainer, request, (f, b) => f.Execute(b));

                Assert.That(result.GetType(), Is.EqualTo(typeof(InternalServerErrorResult)));
            }
        }



        internal class ActionWithBodyFeature : IFeature
        {
            private readonly IFakeService _fakeService;

            public ActionWithBodyFeature(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task<IActionResult> Execute(string value)
            {
                _fakeService.SetValue(value);
                return await Task.FromResult(new OkObjectResult("Bla"));
            }
        }

        internal class ActionWithBodyFeatureWithException : IFeature
        {
            private readonly IFakeService _fakeService;

            public ActionWithBodyFeatureWithException(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task<IActionResult> Execute(string value)
            {
                await Task.Run(() => throw new ArgumentException());
                return new OkObjectResult("Bla");
            }
        }
    }
}
