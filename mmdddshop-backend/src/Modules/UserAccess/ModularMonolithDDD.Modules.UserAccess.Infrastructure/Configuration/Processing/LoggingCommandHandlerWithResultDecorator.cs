namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    /// <summary>
    /// Decorator for command handlers that return results and adds logging functionality.
    /// This decorator logs command execution start, success with result, and failure events with structured logging.
    /// It also enriches log events with command context and correlation ID information.
    /// </summary>
    /// <typeparam name="T">The type of command being handled.</typeparam>
    /// <typeparam name="TResult">The type of result returned by the command.</typeparam>
    internal class LoggingCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult>
        where T : ICommand<TResult>
    {
        private readonly Serilog.ILogger _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly ICommandHandler<T, TResult> _decorated;

        /// <summary>
        /// Initializes a new instance of the LoggingCommandHandlerWithResultDecorator class.
        /// </summary>
        /// <param name="logger">The Serilog logger instance.</param>
        /// <param name="executionContextAccessor">Accessor for execution context information.</param>
        /// <param name="decorated">The decorated command handler.</param>
        public LoggingCommandHandlerWithResultDecorator(
            Serilog.ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            ICommandHandler<T, TResult> decorated)
        {
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
            _decorated = decorated;             
		}

        /// <summary>
        /// Handles the command with logging functionality and returns the result.
        /// Logs command execution start, success with result, and failure events.
        /// Recurring commands are executed without detailed logging to avoid log spam.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task containing the result of the command execution.</returns>
        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {			
			// Skip detailed logging for recurring commands to avoid log spam
			if (command is IRecurringCommand)
            {
                return await _decorated.Handle(command, cancellationToken);
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
                        "Executing command {@Command}",
                        command);

                    var result = await _decorated.Handle(command, cancellationToken);

                    this._logger.Information("Command processed successful, result {Result}", result);

                    return result;
                }
                catch (Exception exception)
                {
                    this._logger.Error(exception, "Command processing failed");
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
            private readonly ICommand<TResult> _command;

            /// <summary>
            /// Initializes a new instance of the CommandLogEnricher class.
            /// </summary>
            /// <param name="command">The command to enrich log events with.</param>
            public CommandLogEnricher(ICommand<TResult> command)
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
                logEvent.AddOrUpdateProperty(new LogEventProperty(
                    "Context",
                    new ScalarValue($"Command:{_command.Id.ToString()}")));
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
                    logEvent.AddOrUpdateProperty(new LogEventProperty(
                        "CorrelationId",
                        new ScalarValue(_executionContextAccessor.CorrelationId)));
                }
            }
        }
    }
}