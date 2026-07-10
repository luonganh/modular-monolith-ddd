// File: BuildingBlocks/Tests/IntegrationTests/EnvironmentVariablesProvider.cs
// Summary: Small helper to read/write/clear environment variables used
//          by integration tests (e.g., database connection strings).
namespace ModularMonolithDDD.BuildingBlocks.Tests.IntegrationTests
{
    public static class EnvironmentVariablesProvider
    {
        public static string GetVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

        public static void SetVariable(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value);
        }

        public static void ClearVariable(string name)
        {
            Environment.SetEnvironmentVariable(name, null);
        }
    }
}
