using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AzureFunctions.Plus.Dependency
{
    public class FeatureAndServiceProvider : IServiceProvider,IDisposable
    {
        private readonly IServiceProvider _innerServiceProvider;

        public FeatureAndServiceProvider(IServiceProvider innerServiceProvider)
        {
            _innerServiceProvider = innerServiceProvider;
        }
        public object GetService(Type serviceType)
        {
            return _innerServiceProvider.GetService(serviceType) ??
                   ActivatorUtilities.CreateInstance(this, serviceType);
        }

        public void Dispose()
        {
            (_innerServiceProvider as ServiceProvider)?.Dispose();
        }
    }
}
