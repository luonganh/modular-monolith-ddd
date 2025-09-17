// File: Tests/SUT/SeedWork/TestBase.cs
// Summary: Shared base for scenario/system tests (SUT).
// - Initializes `UserAccess` module with in-memory dependencies
// - Provides helpers for waiting and scripted DB actions
using ModularMonolithDDD.Tests.SUT.SeedWork.Probing;

namespace ModularMonolithDDD.Tests.SUT.SeedWork
{
    public class TestBase
    {
        protected string ConnectionString { get; set; }

        protected virtual bool PerformDatabaseCleanup => false;

        protected IEmailSender EmailSender { get; private set; }

        protected ILogger Logger { get; private set; }

        protected IUserAccessModule UserAccessModule { get; private set; }

        protected ExecutionContextMock ExecutionContextAccessor { get; private set; }

        protected IEventsBus EventsBus { get; private set; }

        [SetUp]
        public async Task BeforeEachTest()
        {
            SetConnectionString();

            if (PerformDatabaseCleanup)
            {
                await this.ClearDatabase();
            }

            ExecutionContextAccessor = new ExecutionContextMock(Guid.NewGuid());

            var emailsConfiguration = new EmailsConfiguration("luonganh@gmail.com", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            Logger = Substitute.For<ILogger>();

            EventsBus = new InMemoryEventBusClient(Logger);

            InitializeUserAccessModule(emailsConfiguration);
        }

        public static async Task<T> GetEventually<T>(IProbe<T> probe, int timeout)
           where T : class
        {
            var poller = new Poller(timeout);

            return await poller.GetAsync(probe);
        }

        [TearDown]
        public void AfterEachTest()
        {
            // Cleanup if needed            
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

        protected async Task ExecuteScript(string scriptPath)
        {
            var sql = await File.ReadAllTextAsync(scriptPath);

            await using var sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.ExecuteScalarAsync(sql);
        }

        private void InitializeUserAccessModule(EmailsConfiguration emailsConfiguration)
        {
            Logger = Substitute.For<ILogger>();
            EmailSender = Substitute.For<IEmailSender>();

            UserAccessStartup.Initialize(
                ConnectionString,
                ExecutionContextAccessor,
                Logger,
                emailsConfiguration,
                "key",
                EmailSender,
                EventsBus,
                null);

            UserAccessModule = new UserAccessModule();
        }

        private void SetConnectionString()
        {
            const string connectionStringEnvironmentVariable = "ModularMonolithDDD_SUTDatabaseConnectionString";
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
