using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Serialization;
using Ninject.AzureFunctions.Contracts;
using Ninject.AzureFunctions.Features;
using Ninject.AzureFunctions.Tests.Utility;
using NUnit.Framework;

namespace Ninject.AzureFunctions.Tests.ExecuteFeatureTests
{
    public class ExecuteVoidTests
    {
        [Test]
        public async Task ExecutesVoidFeature()
        {
            var kernelContainer = new FakeKernelContainer();
            var fakeService = kernelContainer.Kernel.Get<IFakeService>();

            var result = await ExecuteFeature.ExecuteVoid<VoidFeature>(kernelContainer, f => f.Execute("Test"));

            Assert.That(fakeService.Value,Is.EqualTo("Test"));
            Assert.That(result.GetType(),Is.EqualTo(typeof(OkResult)));
        }

        [Test]
        public async Task ExecutesVoidFeatureThrowsException()
        {
            var kernelContainer = new FakeKernelContainer();

            var result = await ExecuteFeature.ExecuteVoid<VoidFeatureWithException>(kernelContainer, f => f.Execute("Test"));
            
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
