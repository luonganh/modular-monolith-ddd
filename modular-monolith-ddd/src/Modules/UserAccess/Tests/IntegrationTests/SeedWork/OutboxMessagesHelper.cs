namespace ModularMonolithDDD.Modules.UserAccess.Tests.IntegrationTests.SeedWork
{
    public class OutboxMessagesHelper
    {
        public static async Task<List<OutboxMessageDto>> GetOutboxMessages(IDbConnection connection)
        {
            const string sql = $"""
                               SELECT 
                                   [OutboxMessage].[Id] as [{nameof(OutboxMessageDto.Id)}], 
                                   [OutboxMessage].[Type] as [{nameof(OutboxMessageDto.Type)}], 
                                   [OutboxMessage].[Data] as [{nameof(OutboxMessageDto.Data)}] 
                               FROM [users].[OutboxMessages] AS [OutboxMessage] 
                               ORDER BY [OutboxMessage].[OccurredOn]
                               """;

            var messages = await connection.QueryAsync<OutboxMessageDto>(sql);
            return messages.AsList();
        }

        public static T Deserialize<T>(OutboxMessageDto message)
            where T : class, INotification
        {
            Type type = Assembly.GetAssembly(typeof(AuthenticateCommand)).GetType(typeof(T).FullName);
            return JsonConvert.DeserializeObject(message.Data, type) as T;
        }
    }

    public class AuthenticateCommand : CommandBase<AuthenticationResult>
    {
        public AuthenticateCommand(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; }

        public string Password { get; }
    }
}