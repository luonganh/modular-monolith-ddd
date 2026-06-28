namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Quartz
{
    /// <summary>
    /// Serilog log provider implementation for Quartz.NET scheduler.
    /// This class bridges Quartz.NET's logging system with Serilog, allowing Quartz
    /// to use the same logging configuration as the rest of the application.
    /// </summary>
	internal class SerilogLogProvider : ILogProvider
	{
		private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the SerilogLogProvider class.
        /// </summary>
        /// <param name="logger">The Serilog logger instance to use for Quartz logging.</param>
		internal SerilogLogProvider(ILogger logger)
		{
			_logger = logger;
		}

        /// <summary>
        /// Creates a logger delegate that maps Quartz log levels to Serilog log levels.
        /// This method returns a function that Quartz can call to log messages at different levels.
        /// </summary>
        /// <param name="name">The name of the logger (typically the class name).</param>
        /// <returns>A logger delegate that bridges Quartz logging to Serilog.</returns>
		public Logger GetLogger(string name)
		{
			return (level, func, exception, parameters) =>
			{
				// Return early if no message function is provided
				if (func == null)
				{
					return true;
				}

				// Map Quartz log levels to Serilog log levels
				if (level == LogLevel.Debug || level == LogLevel.Trace)
				{
					_logger.Debug(exception, func(), parameters);
				}

				if (level == LogLevel.Info)
				{
					_logger.Information(exception, func(), parameters);
				}

				if (level == LogLevel.Warn)
				{
					_logger.Warning(exception, func(), parameters);
				}

				if (level == LogLevel.Error)
				{
					_logger.Error(exception, func(), parameters);
				}

				if (level == LogLevel.Fatal)
				{
					_logger.Fatal(exception, func(), parameters);
				}

				return true;
			};
		}

        /// <summary>
        /// Opens a nested logging context. Not implemented in this provider.
        /// </summary>
        /// <param name="message">The message for the nested context.</param>
        /// <returns>Not implemented - throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
		public IDisposable OpenNestedContext(string message)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Opens a mapped logging context with string value. Not implemented in this provider.
        /// </summary>
        /// <param name="key">The key for the mapped context.</param>
        /// <param name="value">The string value for the mapped context.</param>
        /// <returns>Not implemented - throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
		public IDisposable OpenMappedContext(string key, string value)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Opens a mapped logging context with object value. Not implemented in this provider.
        /// </summary>
        /// <param name="key">The key for the mapped context.</param>
        /// <param name="value">The object value for the mapped context.</param>
        /// <param name="destructure">Whether to destructure the object value.</param>
        /// <returns>Not implemented - throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
		public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
		{
			throw new NotImplementedException();
		}
	}
}