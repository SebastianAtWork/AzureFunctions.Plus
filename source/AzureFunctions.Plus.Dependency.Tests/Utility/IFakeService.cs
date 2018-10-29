namespace AzureFunctions.Plus.Dependency.Tests.Utility
{
        public interface IFakeService
        {
            string Value { get; set; }

            void SetValue(string value);

        }
}
