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
    public class ExecuteOkWithBodyTests
    {
        [Test]
        public async Task ExecutesOkWithBodyFeature()
        {
            using (var collectionContainer = new AutoFeatureContainer<TestServiceInitializer>(new FakeLogger()))
            using (var request = new FakeHttpRequest<string>("Test"))
            {
                var fakeService = collectionContainer.Services.GetService<IFakeService>();

                var result =
                    await ExecuteFeature.ExecuteOkWithBody<OkWithBodyFeature, string, string>(collectionContainer, request, (f, b) => f.Execute(b)) as
                        OkObjectResult;

                Assert.That(fakeService.Value, Is.EqualTo("Test"));
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Value, Is.EqualTo("Bla"));

            }
        }

        [Test]
        public async Task ExecutesOkWithBodyFeatureThrowsException()
        {
            using (var collectionContainer = new AutoFeatureContainer<TestServiceInitializer>(new FakeLogger()))
            using (var request = new FakeHttpRequest<string>("Test"))
            {
                var result =
                    await ExecuteFeature.ExecuteOkWithBody<OkWithBodyFeatureWithException, string, string>(collectionContainer, request, (f, b) => f.Execute(b));

                Assert.That(result.GetType(), Is.EqualTo(typeof(InternalServerErrorResult)));
            }
        }



        internal class OkWithBodyFeature : IFeature
        {
            private readonly IFakeService _fakeService;

            public OkWithBodyFeature(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task<string> Execute(string value)
            {
                _fakeService.SetValue(value);
                return await Task.FromResult("Bla");
            }
        }

        internal class OkWithBodyFeatureWithException : IFeature
        {
            private readonly IFakeService _fakeService;

            public OkWithBodyFeatureWithException(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task<string> Execute(string value)
            {
                await Task.Run(() => throw new ArgumentException());
                return "Bla";
            }
        }
    }
}
