using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AzureFunctions.Plus.Dependency.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace AzureFunctions.Plus.Dependency
{
    [ExcludeFromCodeCoverage]
    public class AutoFeatureContainer<T> : IDisposable, IAutoFeatureContainer where T: IServiceInitializer
    {

        public AutoFeatureContainer(ILogger log)
        {
            var serviceInitializer = Activator.CreateInstance(typeof(T)) as IServiceInitializer;
            var innerProvider = serviceInitializer.CreateServiceCollection(log).BuildServiceProvider(true);
            Services = new FeatureAndServiceProvider(innerProvider);
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
            (Services as FeatureAndServiceProvider)?.Dispose();
        }

        public IServiceProvider Services { get; }
    }
}
