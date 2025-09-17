// File: BuildingBlocks/Tests/IntegrationTests/SeedWork/TestBase.cs
// Summary: Common setup for BuildingBlocks integration tests: resolves
// connection string, clears DB state and configures test logger/email bus.
namespace ModularMonolithDDD.BuildingBlocks.Tests.IntegrationTests.SeedWork
{
    public class TestBase
    {
        protected string ConnectionString { get; private set; }

        protected ILogger Logger { get; private set; }

        protected IEmailSender EmailSender { get; private set; }

        protected IEventsBus EventsBus { get; private set; }

        [SetUp]
        public async Task BeforeEachTest()
        {
            const string connectionStringEnvironmentVariable =
                "ASPNETCORE_ModularMonolithDDD_BuildingBlocks_IntegrationTests_ConnectionString";
            ConnectionString = EnvironmentVariablesProvider.GetVariable(connectionStringEnvironmentVariable);
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to integration tests database using environment variable: {connectionStringEnvironmentVariable}");
            }

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await ClearDatabase(sqlConnection);
            }

            Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            EmailSender = Substitute.For<IEmailSender>();
            EventsBus = new InMemoryEventBusClient(Logger);
        }

        [TearDown]
        public void AfterEachTest()
        {
            // Cleanup if needed
        }

        private static async Task ClearDatabase(IDbConnection connection)
        {
            const string sql = "DELETE FROM [app].[Emails] " +
                               "DELETE FROM [app].[InboxMessages] " +
                               "DELETE FROM [app].[InternalCommands] " +
                               "DELETE FROM [app].[OutboxMessages] ";

            await connection.ExecuteScalarAsync(sql);
        }
    }
}
