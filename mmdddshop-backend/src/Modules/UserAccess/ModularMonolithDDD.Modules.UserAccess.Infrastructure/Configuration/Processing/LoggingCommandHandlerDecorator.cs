namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    /// <summary>
    /// Decorator for command handlers that adds logging functionality.
    /// This decorator logs command execution start, success, and failure events with structured logging.
    /// It also enriches log events with command context and correlation ID information.
    /// </summary>
    /// <typeparam name="T">The type of command being handled.</typeparam>
    internal class LoggingCommandHandlerDecorator<T> : ICommandHandler<T>
        where T : ICommand
    {
        private readonly Serilog.ILogger _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly ICommandHandler<T> _decorated;

        /// <summary>
        /// Initializes a new instance of the LoggingCommandHandlerDecorator class.
        /// </summary>
        /// <param name="logger">The Serilog logger instance.</param>
        /// <param name="executionContextAccessor">Accessor for execution context information.</param>
        /// <param name="decorated">The decorated command handler.</param>
        public LoggingCommandHandlerDecorator(
            Serilog.ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            ICommandHandler<T> decorated)
        {
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
            _decorated = decorated;             
		}

        /// <summary>
        /// Handles the command with logging functionality.
        /// Logs command execution start, success, and failure events.
        /// Recurring commands are executed without detailed logging to avoid log spam.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(T command, CancellationToken cancellationToken)
        {			
			// Skip detailed logging for recurring commands to avoid log spam
			if (command is IRecurringCommand)
            {
                 await _decorated.Handle(command, cancellationToken);

                 return;
            }

            // Enrich log context with request and command information
            using (
                Serilog.Context.LogContext.Push(
                    new RequestLogEnricher(_executionContextAccessor),
                    new CommandLogEnricher(command)))
            {
                try
                {
                    this._logger.Information(
                        "Executing command {Command}",
                        command.GetType().Name);

                    await _decorated.Handle(command, cancellationToken);

                    this._logger.Information("Command {Command} processed successful", command.GetType().Name);
                }
                catch (Exception exception)
                {
                    this._logger.Error(exception, "Command {Command} processing failed", command.GetType().Name);
                    throw;
                }
            }
        }

        /// <summary>
        /// Log event enricher that adds command-specific context to log events.
        /// This enricher adds the command ID to the log context for better traceability.
        /// </summary>
        private class CommandLogEnricher : ILogEventEnricher
        {
            private readonly ICommand _command;

            /// <summary>
            /// Initializes a new instance of the CommandLogEnricher class.
            /// </summary>
            /// <param name="command">The command to enrich log events with.</param>
            public CommandLogEnricher(ICommand command)
            {
                _command = command;
            }

            /// <summary>
            /// Enriches the log event with command context information.
            /// </summary>
            /// <param name="logEvent">The log event to enrich.</param>
            /// <param name="propertyFactory">Factory for creating log event properties.</param>
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{_command.Id.ToString()}")));
            }
        }

        /// <summary>
        /// Log event enricher that adds request-specific context to log events.
        /// This enricher adds the correlation ID to the log context for request tracing.
        /// </summary>
        private class RequestLogEnricher : ILogEventEnricher
        {
            private readonly IExecutionContextAccessor _executionContextAccessor;

            /// <summary>
            /// Initializes a new instance of the RequestLogEnricher class.
            /// </summary>
            /// <param name="executionContextAccessor">Accessor for execution context information.</param>
            public RequestLogEnricher(IExecutionContextAccessor executionContextAccessor)
            {
                _executionContextAccessor = executionContextAccessor;
            }

            /// <summary>
            /// Enriches the log event with request context information.
            /// </summary>
            /// <param name="logEvent">The log event to enrich.</param>
            /// <param name="propertyFactory">Factory for creating log event properties.</param>
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_executionContextAccessor.IsAvailable)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_executionContextAccessor.CorrelationId)));
                }
            }
        }
    }
}