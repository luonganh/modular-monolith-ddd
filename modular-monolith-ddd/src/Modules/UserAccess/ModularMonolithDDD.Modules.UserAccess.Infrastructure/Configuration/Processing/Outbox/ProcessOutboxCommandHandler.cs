namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
    /// <summary>
    /// Command handler for processing outbox messages to ensure reliable domain event delivery.
    /// This handler retrieves unprocessed outbox messages from the database, deserializes them,
    /// publishes them as domain events, and marks them as processed to prevent duplicate processing.
    /// </summary>
    internal class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
    {
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IDomainNotificationsMapper _domainNotificationsMapper;

        /// <summary>
        /// Initializes a new instance of the ProcessOutboxCommandHandler.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for publishing domain events.</param>
        /// <param name="sqlConnectionFactory">Factory for creating database connections.</param>
        /// <param name="domainNotificationsMapper">Mapper for resolving domain event notification types.</param>
        public ProcessOutboxCommandHandler(
            IMediator mediator,
            ISqlConnectionFactory sqlConnectionFactory,
            IDomainNotificationsMapper domainNotificationsMapper)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
            _domainNotificationsMapper = domainNotificationsMapper;
        }

        /// <summary>
        /// Handles the process outbox command by processing all unprocessed outbox messages.
        /// This method retrieves unprocessed messages from the database, deserializes them,
        /// publishes them as domain events, and marks them as processed.
        /// </summary>
        /// <param name="command">The process outbox command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
        {
            // Get an open database connection
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            
            // Check if the users schema exists (optional validation)
            var schemaExists = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sys.schemas WHERE name = 'users'");
            
            // SQL query to retrieve unprocessed outbox messages ordered by occurrence time
            const string sql = $"""
                               SELECT 
                                   [OutboxMessage].[Id] AS [{nameof(OutboxMessageDto.Id)}], 
                                   [OutboxMessage].[Type] AS [{nameof(OutboxMessageDto.Type)}], 
                                   [OutboxMessage].[Data] AS [{nameof(OutboxMessageDto.Data)}] 
                               FROM [users].[OutboxMessages] AS [OutboxMessage] 
                               WHERE [OutboxMessage].[ProcessedDate] IS NULL 
                               ORDER BY [OutboxMessage].[OccurredOn]
                               """;

            // Execute the query and get the results
            var messages = await connection.QueryAsync<OutboxMessageDto>(sql);
            var messagesList = messages.AsList();

            // SQL query to update the processed date for processed messages
            const string sqlUpdateProcessedDate = """
                                                  UPDATE [users].[OutboxMessages] 
                                                  SET [ProcessedDate] = @Date 
                                                  WHERE [Id] = @Id
                                                  """;
            
            // Process each unprocessed message
            if (messagesList.Count > 0)
            {
                foreach (var message in messagesList)
                {
                    // Resolve the domain event notification type from the message type
                    var type = _domainNotificationsMapper.GetType(message.Type);
                    if (type == null)
                    {                       
                        continue;
                    }

                    // Deserialize the message data to the appropriate domain event notification
                    var @event = JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;
                    if (@event == null)
                    {                       
                        continue;
                    }

                    // Enrich logging context with outbox message information
                    using (Serilog.Context.LogContext.Push(new OutboxMessageContextEnricher(@event)))
                    {
                        // Publish the domain event notification through MediatR
                        await this._mediator.Publish(@event, cancellationToken);

                        // Mark the message as processed by updating the ProcessedDate
                        await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                        {
                            Date = DateTime.UtcNow,
                            message.Id
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Log event enricher that adds outbox message context to log events.
        /// This enricher adds the outbox message ID to the log context for better traceability
        /// when processing outbox messages.
        /// </summary>
        private class OutboxMessageContextEnricher : ILogEventEnricher
        {
            private readonly IDomainEventNotification _notification;

            /// <summary>
            /// Initializes a new instance of the OutboxMessageContextEnricher class.
            /// </summary>
            /// <param name="notification">The domain event notification being processed.</param>
            public OutboxMessageContextEnricher(IDomainEventNotification notification)
            {
                _notification = notification;
            }

            /// <summary>
            /// Enriches the log event with outbox message context information.
            /// </summary>
            /// <param name="logEvent">The log event to enrich.</param>
            /// <param name="propertyFactory">Factory for creating log event properties.</param>
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"OutboxMessage:{_notification.Id.ToString()}")));
            }
        }
    }
}