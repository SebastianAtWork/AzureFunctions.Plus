using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ninject.AzureFunctions.Contracts;
using Ninject.AzureFunctions.Features;
using Ninject.AzureFunctions.Tests.Utility;
using NUnit.Framework;

namespace Ninject.AzureFunctions.Tests.ExecuteFeatureTests
{
    public class ExecuteVoidWithBodyTests
    {
        [Test]
        public async Task ExecutesVoidWithBodyFeature()
        {
            var kernelContainer = new FakeKernelContainer();
            var fakeService = kernelContainer.Kernel.Get<IFakeService>();
            var request = new FakeHttpRequest<string>("Test");

            var result =
                await ExecuteFeature.ExecuteVoidWithBody<VoidWithBodyFeature,string>(kernelContainer, request,(f,b) => f.Execute(b)) as
                    OkResult;

            Assert.That(fakeService.Value, Is.EqualTo("Test"));
            Assert.That(result, Is.Not.Null);

            request.Dispose();
        }

        [Test]
        public async Task ExecutesVoidWithBodyFeatureThrowsException()
        {
            var kernelContainer = new FakeKernelContainer();
            var request = new FakeHttpRequest<string>("Test");

            var result =
                await ExecuteFeature.ExecuteVoidWithBody<VoidWithBodyFeatureWithException,string>(kernelContainer, request,(f, b) => f.Execute(b));

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
