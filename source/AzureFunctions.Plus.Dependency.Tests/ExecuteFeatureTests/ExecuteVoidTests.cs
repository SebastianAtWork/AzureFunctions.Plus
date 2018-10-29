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
    public class ExecuteVoidTests
    {
        [Test]
        public async Task ExecutesVoidFeature()
        {
            var collectionContainer = new FakeServiceCollectionContainer();
            var fakeService = collectionContainer.Services.GetService<IFakeService>();

            var result = await ExecuteFeature.ExecuteVoid<VoidFeature>(collectionContainer, f => f.Execute("Test"));

            Assert.That(fakeService.Value,Is.EqualTo("Test"));
            Assert.That(result.GetType(),Is.EqualTo(typeof(OkResult)));
        }

        [Test]
        public async Task ExecutesVoidFeatureThrowsException()
        {
            var collectionContainer = new FakeServiceCollectionContainer();

            var result = await ExecuteFeature.ExecuteVoid<VoidFeatureWithException>(collectionContainer, f => f.Execute("Test"));
            
            Assert.That(result.GetType(), Is.EqualTo(typeof(InternalServerErrorResult)));
        }

    

        internal class VoidFeature : IFeature
        {
            private readonly IFakeService _fakeService;

            public VoidFeature(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task Execute(string value)
            {
                await Task.FromResult(1);
                _fakeService.SetValue(value);
            }
        }
        internal class VoidFeatureWithException : IFeature
        {
            private readonly IFakeService _fakeService;

            public VoidFeatureWithException(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task Execute(string value)
            {
                await Task.Run(()=>throw new ArgumentException());
            }
        }

        
    }
}
