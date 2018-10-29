using System;
using System.Threading.Tasks;
using System.Web.Http;
using AzureFunctions.Plus.Dependency.Contracts;
using AzureFunctions.Plus.Dependency.Features;
using AzureFunctions.Plus.Dependency.Tests.Utility;
using Microsoft.AspNetCore.Mvc;
using Ninject;
using NUnit.Framework;

namespace AzureFunctions.Plus.Dependency.Tests.ExecuteFeatureTests
{
    public class ExecuteActionTests
    {
        [Test]
        public async Task ExecutesActionFeature()
        {
            var kernelContainer = new FakeKernelContainer();
            var fakeService = kernelContainer.Kernel.Get<IFakeService>();

            var result =
                await ExecuteFeature.ExecuteAction<ActionFeature>(kernelContainer, f => f.Execute("Test")) as
                    OkObjectResult;

            Assert.That(fakeService.Value, Is.EqualTo("Test"));
            Assert.That(result, Is.Not.Null);
            var resultContent = result.Value;
            Assert.That(resultContent, Is.EqualTo("Bla"));
        }

        [Test]
        public async Task ExecutesActionFeatureThrowsException()
        {
            var kernelContainer = new FakeKernelContainer();

            var result =
                await ExecuteFeature.ExecuteAction<ActionFeatureWithException>(kernelContainer, f => f.Execute("Test"));

            Assert.That(result.GetType(), Is.EqualTo(typeof(InternalServerErrorResult)));
        }

       

        internal class ActionFeature : IFeature
        {
            private readonly IFakeService _fakeService;

            public ActionFeature(IFakeService fakeService)
            {
                _fakeService = fakeService;
            }

            public async Task<IActionResult> Execute(string value)
            {
                _fakeService.SetValue(value);
                return await Task.FromResult(new OkObjectResult("Bla"));
            }
        }

        internal class ActionFeatureWithException : IFeature
        {
            private readonly IFakeService _fakeService;

            public ActionFeatureWithException(IFakeService fakeService)
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
