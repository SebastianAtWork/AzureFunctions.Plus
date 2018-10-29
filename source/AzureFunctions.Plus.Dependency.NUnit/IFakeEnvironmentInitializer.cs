using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Plus.Dependency.NUnit
{
    public interface IFakeEnvironmentInitializer
    {
        void Initialize();
    }
}
