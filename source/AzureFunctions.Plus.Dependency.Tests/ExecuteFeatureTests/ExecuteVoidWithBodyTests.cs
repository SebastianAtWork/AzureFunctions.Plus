using System;
using System.Threading.Tasks;
using System.Web.Http;
using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.Features;
using AzureFunctions.Plus.Dependency.Tests.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.Tests.ExecuteFeatureTests
{
    public class ExecuteVoidWithBodyTests
    {
        [Test]
        public async Task ExecutesVoidWithBodyFeature()
        {
            var collectionContainer = new FakeServiceCollectionContainer();
            var fakeService = collectionContainer.Services.GetService<IFakeService>();
            var request = new FakeHttpRequest<string>("Test");

            var result =
                await ExecuteFeature.ExecuteVoidWithBody<VoidWithBodyFeature,string>(collectionContainer, request,(f,b) => f.Execute(b)) as
                    OkResult;

            Assert.That(fakeService.Value, Is.EqualTo("Test"));
            Assert.That(result, Is.Not.Null);

            request.Dispose();
        }

        [Test]
        public async Task ExecutesVoidWithBodyFeatureThrowsException()
        {
            var collectionContainer = new FakeServiceCollectionContainer();
            var request = new FakeHttpRequest<string>("Test");

            var result =
                await ExecuteFeature.ExecuteVoidWithBody<VoidWithBodyFeatureWithException,string>(collectionContainer, request,(f, b) => f.Execute(b));

            Assert.That(result.GetType(), Is.EqualTo(typeof(InternalServerErrorResult)));
            request.Dispose();
        }



        internal class VoidWithBodyFeature : IFeature
        {
            private readonly IFakeService _fakeService;

            public VoidWithBodyFeature(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task Execute(string value)
            {
                await Task.FromResult(1);
                _fakeService.SetValue(value);
            }
        }

        internal class VoidWithBodyFeatureWithException : IFeature
        {
            private readonly IFakeService _fakeService;

            public VoidWithBodyFeatureWithException(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task Execute(string value)
            {
                await Task.Run(() => throw new ArgumentException());
            }
        }
    }
}
