using Microsoft.Extensions.Configuration;
using ModularMonolithDDD.BuildingBlocks.Infrastructure.Configuration;
using System.IO;

namespace ModularMonolithDDD.Tests.IntegrationTests.SeedWork
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
            //// Load .env from root solution
            //// Get .env container path from root solution
            //var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".."));

            //// Load by environment
            //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            //var envFile = environment == "Production" ? ".env.production" : ".env";
            //var envPath = Path.Combine(rootPath, envFile);
            //if (File.Exists(envPath))
            //{
            //    DotNetEnv.Env.Load(envPath);
            //}

            var solutionRoot = typeof(EnvironmentHelper).Assembly.Location;            
            Console.WriteLine($"Solution Root: {solutionRoot}");
            var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".."));
            Console.WriteLine($"Root: {root}");
            EnvironmentHelper.ConfigureEnvironment(root);            
            ConnectionString = EnvironmentHelper.GetConnectionString(new ConfigurationBuilder().Build());
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to integration tests database");
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

        [TearDown]
        public void AfterEachTest()
        {
            //UserAccessStartup.Stop();
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
            const string sql = "DELETE FROM [useraccess].[InboxMessages] " +
                               "DELETE FROM [useraccess].[InternalCommands] " +
                               "DELETE FROM [useraccess].[OutboxMessages] " +
                               "DELETE FROM [useraccess].[UserRoles] " +
                               "DELETE FROM [useraccess].[Users] " +
                               "DELETE FROM [useraccess].[Roles] " +
                               "DELETE FROM [useraccess].[Permissions] " +
                               "DELETE FROM [useraccess].[RolePermissions] ";

            await connection.ExecuteScalarAsync(sql);
        }
    }
}
