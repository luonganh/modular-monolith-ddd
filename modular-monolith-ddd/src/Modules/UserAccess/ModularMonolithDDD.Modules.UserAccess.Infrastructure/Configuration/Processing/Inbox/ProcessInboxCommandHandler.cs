namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox
{
    /// <summary>
    /// Handler for processing inbox messages.
    /// This handler retrieves unprocessed messages from the inbox table and publishes them
    /// as domain notifications. It handles message deserialization and marks processed messages.
    /// </summary>
    internal class ProcessInboxCommandHandler : ICommandHandler<ProcessInboxCommand>
    {
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the ProcessInboxCommandHandler class.
        /// </summary>
        /// <param name="mediator">The mediator for publishing domain notifications.</param>
        /// <param name="sqlConnectionFactory">Factory for creating database connections.</param>
        public ProcessInboxCommandHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        /// <summary>
        /// Processes all unprocessed inbox messages.
        /// Retrieves messages from the database, deserializes them, publishes them as notifications,
        /// and marks them as processed.
        /// </summary>
        /// <param name="command">The process inbox command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            var schemaExists = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sys.schemas WHERE name = 'users'");
            
            // Query for unprocessed inbox messages ordered by occurrence date
            const string sql = $"""
                               SELECT 
                                  [InboxMessage].[Id] AS [{nameof(InboxMessageDto.Id)}], 
                                  [InboxMessage].[Type] AS [{nameof(InboxMessageDto.Type)}], 
                                  [InboxMessage].[Data] AS [{nameof(InboxMessageDto.Data)}] 
                               FROM [users].[InboxMessages] AS [InboxMessage] 
                               WHERE [InboxMessage].[ProcessedDate] IS NULL 
                               ORDER BY [InboxMessage].[OccurredOn]
                               """;

            var messages = await connection.QueryAsync<InboxMessageDto>(sql);

            const string sqlUpdateProcessedDate = """
                                                  UPDATE [users].[InboxMessages] 
                                                  SET [ProcessedDate] = @Date 
                                                  WHERE [Id] = @Id
                                                  """;

            // Process each message
            foreach (var message in messages)
            {
                // Find the assembly containing the message type
                var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name));

                Type type = messageAssembly.GetType(message.Type);
                if (type == null)
                {
					continue; // Skip if type not found
				}

				// Deserialize the message data using System.Text.Json first, fallback to Newtonsoft.Json
				object request = null;
				try
				{
					request = System.Text.Json.JsonSerializer.Deserialize(message.Data, type);
				}
				catch
				{
					// If System.Text.Json fails, fallback to Newtonsoft.Json
					request = JsonConvert.DeserializeObject(message.Data, type);
				}

                try
                {
                    // Publish the message as a domain notification
                    await _mediator.Publish((INotification?)request, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                // Mark the message as processed
                await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                {
                    Date = DateTime.UtcNow,
                    message.Id
                });
            }
        }
    }
}