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
    public class ExecuteActionWithBodyTests
    {
        [Test]
        public async Task ExecutesActionWithBodyFeature()
        {
            var kernelContainer = new FakeKernelContainer();
            var fakeService = kernelContainer.Kernel.Get<IFakeService>();
            var request = new FakeHttpRequest<string>("Test");

            var result =
                await ExecuteFeature.ExecuteActionWithBody<ActionWithBodyFeature, string>(kernelContainer, request, (f, b) => f.Execute(b)) as
                    OkObjectResult;

            Assert.That(fakeService.Value, Is.EqualTo("Test"));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo("Bla"));

            request.Dispose();
        }

        [Test]
        public async Task ExecutesActionWithBodyFeatureThrowsException()
        {
            var kernelContainer = new FakeKernelContainer();
            var request = new FakeHttpRequest<string>("Test");

            var result =
                await ExecuteFeature.ExecuteActionWithBody<ActionWithBodyFeatureWithException, string>(kernelContainer, request, (f, b) => f.Execute(b));

            Assert.That(result.GetType(), Is.EqualTo(typeof(InternalServerErrorResult)));
            request.Dispose();
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
