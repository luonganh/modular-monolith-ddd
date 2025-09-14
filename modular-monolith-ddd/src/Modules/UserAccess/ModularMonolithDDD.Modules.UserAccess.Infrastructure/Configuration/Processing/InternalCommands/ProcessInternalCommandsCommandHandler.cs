namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    /// <summary>
    /// Handler for processing queued internal commands.
    /// This handler retrieves unprocessed commands from the database queue and executes them
    /// with retry logic and error handling. Failed commands are marked with error information.
    /// </summary>
    internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the ProcessInternalCommandsCommandHandler class.
        /// </summary>
        /// <param name="sqlConnectionFactory">Factory for creating database connections.</param>
        public ProcessInternalCommandsCommandHandler(
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;			
		}

        /// <summary>
        /// Processes all unprocessed internal commands from the queue.
        /// Retrieves commands from the database, executes them with retry logic,
        /// and marks failed commands with error information.
        /// </summary>
        /// <param name="command">The process internal commands command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
        {			
			var connection = _sqlConnectionFactory.GetOpenConnection();

            // Query for unprocessed internal commands ordered by enqueue date
            const string sql = $"""
                                SELECT 
                                    [Command].[Id] AS [{nameof(InternalCommandDto.Id)}], 
                                    [Command].[Type] AS [{nameof(InternalCommandDto.Type)}], 
                                    [Command].[Data] AS [{nameof(InternalCommandDto.Data)}] 
                                FROM [users].[InternalCommands] AS [Command] 
                                WHERE [Command].[ProcessedDate] IS NULL 
                                ORDER BY [Command].[EnqueueDate]
                                """;
			
			var commands = await connection.QueryAsync<InternalCommandDto>(sql);

            var internalCommandsList = commands.AsList();

            // Configure retry policy for command execution
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                });

            // Process each command with retry logic
            foreach (var internalCommand in internalCommandsList)
            {
                var result = await policy.ExecuteAndCaptureAsync(() => ProcessCommand(
                    internalCommand));

                // Mark failed commands with error information
                if (result.Outcome == OutcomeType.Failure)
                {
                    await connection.ExecuteScalarAsync(
                        """
                        UPDATE [users].[InternalCommands] 
                        SET 
                            ProcessedDate = @NowDate, 
                            Error = @Error 
                        WHERE [Id] = @Id
                        """,
                        new
                        {
                            NowDate = DateTime.UtcNow,
                            Error = result.FinalException.ToString(),
                            internalCommand.Id
                        });
                }
            }			
		}

        /// <summary>
        /// Processes a single internal command by deserializing and executing it.
        /// </summary>
        /// <param name="internalCommand">The internal command data to process.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessCommand(
            InternalCommandDto internalCommand)
        {
            Type type = Assemblies.Application.GetType(internalCommand.Type);
            dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

            await CommandsExecutor.Execute(commandToProcess);
        }

        /// <summary>
        /// Data transfer object for internal command data from the database.
        /// </summary>
        private class InternalCommandDto
        {
            /// <summary>
            /// Gets or sets the command ID.
            /// </summary>
            public Guid Id { get; set; }

            /// <summary>
            /// Gets or sets the command type name.
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// Gets or sets the serialized command data.
            /// </summary>
            public string Data { get; set; }
        }
    }
}