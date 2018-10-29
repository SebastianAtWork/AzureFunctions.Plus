﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using AzureFunctions.Plus.Dependency.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ninject;

namespace AzureFunctions.Plus.Dependency.Features
{
    public static class ExecuteFeature
    {

        public static async Task<IActionResult> ExecuteVoid<TF>(IAutoFeatureContainer kernelContainer, Func<TF, Task> featureCall)
        {
            var log = kernelContainer.Kernel.Get<ILogger>();
            try
            {
                var feature = kernelContainer.Kernel.Get<TF>();
                await featureCall(feature);
                return new OkResult();

            }
            catch (Exception e)
            {
                log.Log(LogLevel.Error,e,e.ToString());
                return new InternalServerErrorResult();
            }
        }

        public static async Task<IActionResult> ExecuteOk<TF, TR>(IAutoFeatureContainer kernelContainer, Func<TF, Task<TR>> featureCall)
        {
            var log = kernelContainer.Kernel.Get<ILogger>();
            try
            {
                var feature = kernelContainer.Kernel.Get<TF>();
                var result = await featureCall(feature);
                return new OkObjectResult(result);

            }
            catch (Exception e)
            {
                log.Log(LogLevel.Error, e, e.ToString());
                return new InternalServerErrorResult();
            }
        }

        public static async Task<IActionResult> ExecuteAction<TF>(IAutoFeatureContainer kernelContainer, Func<TF, Task<IActionResult>> featureCall)
        {
            var log = kernelContainer.Kernel.Get<ILogger>();
            try
            {
                var feature = kernelContainer.Kernel.Get<TF>();
                var result = await featureCall(feature);
                return result;

            }
            catch (Exception e)
            {
                log.Log(LogLevel.Error, e, e.ToString());
                return new InternalServerErrorResult();
            }
        }

        public static async Task<IActionResult> ExecuteVoidWithBody<TF, TB>(IAutoFeatureContainer kernelContainer, HttpRequest request, Func<TF, TB, Task> featureCall)
        {
            var log = kernelContainer.Kernel.Get<ILogger>();
            try
            {
                var feature = kernelContainer.Kernel.Get<TF>();
                var bodySerialized = await request.ReadAsStringAsync();
                var bodyDeserialized = JsonConvert.DeserializeObject<TB>(bodySerialized);
                await featureCall(feature, bodyDeserialized);
                return new OkResult();

            }
            catch (Exception e)
            {
                log.Log(LogLevel.Error, e, e.ToString());
                return new InternalServerErrorResult();
            }
        }

        public static async Task<IActionResult> ExecuteOkWithBody<TF, TB, TR>(IAutoFeatureContainer kernelContainer, HttpRequest request, Func<TF, TB, Task<TR>> featureCall)
        {
            var log = kernelContainer.Kernel.Get<ILogger>();
            try
            {
                var feature = kernelContainer.Kernel.Get<TF>();
                var bodySerialized = await request.ReadAsStringAsync();
                var bodyDeserialized = JsonConvert.DeserializeObject<TB>(bodySerialized);
                var result = await featureCall(feature, bodyDeserialized);
                return new OkObjectResult(result);

            }
            catch (Exception e)
            {
                log.Log(LogLevel.Error, e, e.ToString());
                return new InternalServerErrorResult();
            }
        }

        public static async Task<IActionResult> ExecuteActionWithBody<TF, TB>(IAutoFeatureContainer kernelContainer, HttpRequest request, Func<TF, TB, Task<IActionResult>> featureCall)
        {
            var log = kernelContainer.Kernel.Get<ILogger>();
            try
            {
                var feature = kernelContainer.Kernel.Get<TF>();
                var bodySerialized = await request.ReadAsStringAsync();
                var bodyDeserialized = JsonConvert.DeserializeObject<TB>(bodySerialized);
                var result = await featureCall(feature, bodyDeserialized);
                return result;

            }
            catch (Exception e)
            {
                log.Log(LogLevel.Error, e, e.ToString());
                return new InternalServerErrorResult();
            }
        }
    }
}
