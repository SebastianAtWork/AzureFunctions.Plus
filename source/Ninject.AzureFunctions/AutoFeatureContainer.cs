using Ninject.AzureFunctions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Ninject.AzureFunctions
{
    [ExcludeFromCodeCoverage]
    public class AutoFeatureContainer<T> : IDisposable, IAutoFeatureContainer where T: IKernelInitializer
    {
        private readonly IReadOnlyKernel _kernel;

        public AutoFeatureContainer(ILogger log)
        {
            var kernelInizializer = Activator.CreateInstance(typeof(T)) as IKernelInitializer;
            _kernel = kernelInizializer.CreateKernelConfiguration(log).BuildReadonlyKernel();
        }

        public IReadOnlyKernel Kernel
        {
            get { return _kernel; }
        }

        public async Task<IActionResult> ExecuteVoidFeature<TF>(HttpRequest request,Func<TF,Task> featureCall) where TF : IFeature
        {
            return await Features.ExecuteFeature.ExecuteVoid(this, featureCall);
        }

        public async Task<IActionResult> ExecuteOkFeature<TF,TR>(HttpRequest request, Func<TF, Task<TR>> featureCall) where TF : IFeature
        {
            return await Features.ExecuteFeature.ExecuteOk(this, featureCall);
        }

        public async Task<IActionResult> ExecuteActionFeature<TF>(HttpRequest request, Func<TF, Task<IActionResult>> featureCall) where TF : IFeature
        {
            return await Features.ExecuteFeature.ExecuteAction(this, featureCall);
        }

        public async Task<IActionResult> ExecuteVoidFeatureWithBody<TF,TB>(HttpRequest request, Func<TF,TB, Task> featureCall) where TF : IFeature
        {
            return await Features.ExecuteFeature.ExecuteVoidWithBody(this, request, featureCall);
        }

        public async Task<IActionResult> ExecuteOkFeatureWithBody<TF,TB, TR>(HttpRequest request, Func<TF,TB, Task<TR>> featureCall) where TF : IFeature
        {
            return await Features.ExecuteFeature.ExecuteOkWithBody(this, request, featureCall);
        }

        public async Task<IActionResult> ExecuteActionFeatureWithBody<TF,TB>(HttpRequest request, Func<TF,TB, Task<IActionResult>> featureCall) where TF : IFeature
        {
            return await Features.ExecuteFeature.ExecuteActionWithBody(this, request, featureCall);
        }


        public void Dispose()
        {
            _kernel.Dispose();
        }
    }
}
