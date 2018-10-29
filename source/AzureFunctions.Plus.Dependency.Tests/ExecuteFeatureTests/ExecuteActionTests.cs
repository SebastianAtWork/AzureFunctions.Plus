﻿using System;
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
    public class ExecuteActionTests
    {
        [Test]
        public async Task ExecutesActionFeature()
        {
            var collectionContainer = new FakeServiceCollectionContainer();
            var fakeService = collectionContainer.Services.GetService<IFakeService>();

            var result =
                await ExecuteFeature.ExecuteAction<ActionFeature>(collectionContainer, f => f.Execute("Test")) as
                    OkObjectResult;

            Assert.That(fakeService.Value, Is.EqualTo("Test"));
            Assert.That(result, Is.Not.Null);
            var resultContent = result.Value;
            Assert.That(resultContent, Is.EqualTo("Bla"));
        }

        [Test]
        public async Task ExecutesActionFeatureThrowsException()
        {
            var collectionContainer = new FakeServiceCollectionContainer();

            var result =
                await ExecuteFeature.ExecuteAction<ActionFeatureWithException>(collectionContainer, f => f.Execute("Test"));

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
