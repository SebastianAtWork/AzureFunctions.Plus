

using System;

namespace AzureFunctions.Plus.Dependency.Contracts
{
    public interface IAutoFeatureContainer
    {
        IServiceProvider Services { get; }
    }
}