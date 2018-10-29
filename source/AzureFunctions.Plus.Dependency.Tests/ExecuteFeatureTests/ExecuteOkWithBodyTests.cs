using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Ninject.AzureFunctions.Contracts;
using Ninject.AzureFunctions.Features;
using Ninject.AzureFunctions.Tests.Utility;
using NUnit.Framework;

namespace Ninject.AzureFunctions.Tests.ExecuteFeatureTests
{
    public class ExecuteOkWithBodyTests
    {
        [Test]
        public async Task ExecutesOkWithBodyFeature()
        {
            var kernelContainer = new FakeKernelContainer();
            var fakeService = kernelContainer.Kernel.Get<IFakeService>();
            var request = new FakeHttpRequest<string>("Test");

            var result =
                await ExecuteFeature.ExecuteOkWithBody<OkWithBodyFeature, string,string>(kernelContainer, request, (f, b) => f.Execute(b)) as
                    OkObjectResult;

            Assert.That(fakeService.Value, Is.EqualTo("Test"));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo("Bla"));

            request.Dispose();
        }

        [Test]
        public async Task ExecutesOkWithBodyFeatureThrowsException()
        {
            var kernelContainer = new FakeKernelContainer();
            var request = new FakeHttpRequest<string>("Test");

            var result =
                await ExecuteFeature.ExecuteOkWithBody<OkWithBodyFeatureWithException, string,string>(kernelContainer, request, (f, b) => f.Execute(b));

            Assert.That(result.GetType(), Is.EqualTo(typeof(InternalServerErrorResult)));
            request.Dispose();
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
