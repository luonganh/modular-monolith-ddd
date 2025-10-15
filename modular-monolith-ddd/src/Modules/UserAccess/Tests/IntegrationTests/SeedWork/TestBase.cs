// File: Modules/UserAccess/Tests/IntegrationTests/SeedWork/TestBase.cs
// Summary: Base class bootstrapping `UserAccess` module for integration tests.
// - Reads connection string from env var
// - Clears test DB and initializes module with test-friendly dependencies
using Microsoft.Extensions.Configuration;
using ModularMonolithDDD.BuildingBlocks.Infrastructure.Configuration;
using System.IO;

namespace ModularMonolithDDD.Modules.UserAccess.Tests.IntegrationTests.SeedWork
{
    public class TestBase
    {
        protected string ConnectionString { get; private set; }

        protected ILogger Logger { get; private set; }

        protected IUserAccessModule UserAccessModule { get; private set; }

        protected IEmailSender EmailSender { get; private set; }

        protected ExecutionContextMock ExecutionContext { get; private set; }

        protected IEventsBus EventsBus { get; private set; }

        [SetUp]
        public async Task BeforeEachTest()
        {
            var solutionRoot = typeof(EnvironmentHelper).Assembly.Location;
          
            var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..", "..", "..", ".."));
           
            EnvironmentHelper.ConfigureEnvironment(root);
            ConnectionString = EnvironmentHelper.GetConnectionString(new ConfigurationBuilder().Build());
            
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to integration tests database using environment variable");
            }

            //using (var sqlConnection = new SqlConnection(ConnectionString))
            //{
            //    await ClearDatabase(sqlConnection);
            //}

            Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            EmailSender = Substitute.For<IEmailSender>();
            ExecutionContext = new ExecutionContextMock(Guid.NewGuid());

            EventsBus = new InMemoryEventBusClient(Logger);

            UserAccessStartup.Initialize(
                ConnectionString,
                ExecutionContext,
                Logger,
                new EmailsConfiguration("luonganh@gmail.com", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty),
                "key",
                EmailSender,
                EventsBus,
                null);

            UserAccessModule = new UserAccessModule();
        }      

        protected static void AssertBrokenRule<TRule>(AsyncTestDelegate testDelegate)
            where TRule : class, IBusinessRule
        {
            var message = $"Expected {typeof(TRule).Name} broken rule";
            var businessRuleValidationException =
                Assert.CatchAsync<BusinessRuleValidationException>(testDelegate, message);
            if (businessRuleValidationException != null)
            {
                Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
            }
        }

        private static async Task ClearDatabase(IDbConnection connection)
        {
            const string sql = "DELETE FROM [users].[InboxMessages] " +
                               "DELETE FROM [users].[InternalCommands] " +
                               "DELETE FROM [users].[OutboxMessages] " +
                               "DELETE FROM [users].[UserRoles] " +
                               "DELETE FROM [users].[Users] " +
                               "DELETE FROM [users].[Roles] " +
                               "DELETE FROM [users].[Permissions] " +
                               "DELETE FROM [users].[RolePermissions] ";

            await connection.ExecuteScalarAsync(sql);
        }
    }
}
