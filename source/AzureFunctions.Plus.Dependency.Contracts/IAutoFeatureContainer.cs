using Ninject;

namespace AzureFunctions.Plus.Dependency.Contracts
{
    public interface IAutoFeatureContainer
    {
        IReadOnlyKernel Kernel { get; }
    }
}