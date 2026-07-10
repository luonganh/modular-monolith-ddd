// File: Tests/SUT/SeedWork/TestBase.cs
// Summary: Shared base for scenario/system tests (SUT) (test harness and configuration for the system under test), 
// it explains how the test system is started, configured, seeded, authenticated, connected to DB, etc
// - Initializes `UserAccess` module with in-memory dependencies
// - Provides helpers for waiting and scripted DB actions
namespace ModularMonolithDDD.Tests.SUT.SeedWork
{
    public class TestBase
    {
        protected string ConnectionString { get; set; }

        protected virtual bool PerformDatabaseCleanup => false;
        
        protected ExecutionContextMock ExecutionContextAccessor { get; private set; }
               

        public static async Task<T> GetEventually<T>(IProbe<T> probe, int timeout)
           where T : class
        {
            var poller = new Poller(timeout);

            return await poller.GetAsync(probe);
        }        

        protected async Task WaitForAsyncOperations()
        {
            // Wait for async operations to complete
            await Task.Delay(100);
        }

        protected void SetDate(DateTime date)
        {
            // Set system date if needed
        }
       
        private void SetConnectionString()
        {
            const string connectionStringEnvironmentVariable = "ModularMonolithDDDShop_SUTDatabaseConnectionString";
            ConnectionString = Environment.GetEnvironmentVariable(connectionStringEnvironmentVariable);
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to SUT database using environment variable: {connectionStringEnvironmentVariable}");
            }
        }

        private async Task ClearDatabase()
        {
            await using var sqlConnection = new SqlConnection(ConnectionString);
            await DatabaseCleaner.ClearAllData(sqlConnection);
        }
    }
}
