namespace Ninject.AzureFunctions.Contracts
{
    public interface IAutoFeatureContainer
    {
        IReadOnlyKernel Kernel { get; }
    }
}