namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    /// <summary>
    /// Service for scheduling internal commands to be processed asynchronously.
    /// This service stores commands in the database queue for later processing by background jobs.
    /// Commands are serialized and stored with metadata for proper deserialization and execution.
    /// </summary>
    public class CommandsScheduler : ICommandsScheduler
	{
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the CommandsScheduler class.
        /// </summary>
        /// <param name="sqlConnectionFactory">Factory for creating database connections.</param>
        public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        /// <summary>
        /// Enqueues a command for asynchronous processing.
        /// The command is serialized and stored in the internal commands table for later processing.
        /// </summary>
        /// <param name="command">The command to enqueue.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task EnqueueAsync(ICommand command)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert = "INSERT INTO [users].[InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
                                     "(@Id, @EnqueueDate, @Type, @Data)";

            await connection.ExecuteAsync(sqlInsert, new
            {
                command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = command.GetType().FullName,
                Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                })
            });
        }

        /// <summary>
        /// Enqueues a command that returns a result for asynchronous processing.
        /// The command is serialized and stored in the internal commands table for later processing.
        /// </summary>
        /// <typeparam name="T">The type of result returned by the command.</typeparam>
        /// <param name="command">The command to enqueue.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task EnqueueAsync<T>(ICommand<T> command)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert = "INSERT INTO [users].[InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
                                     "(@Id, @EnqueueDate, @Type, @Data)";

            await connection.ExecuteAsync(sqlInsert, new
            {
                command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = command.GetType().FullName,
                Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                })
            });
        }
    }
}