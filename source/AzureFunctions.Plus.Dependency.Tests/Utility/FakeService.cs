namespace AzureFunctions.Plus.Dependency.Tests.Utility
{
        public class FakeService : IFakeService
        {
            public string Value { get; set; }

            public void SetValue(string value)
            {
                Value = value;
            }
        }
}
