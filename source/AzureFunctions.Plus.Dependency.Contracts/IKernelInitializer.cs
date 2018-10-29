using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ninject.AzureFunctions.Contracts
{
    public interface IKernelInitializer
    {
        IKernelConfiguration CreateKernelConfiguration(ILogger log);
    }
}
