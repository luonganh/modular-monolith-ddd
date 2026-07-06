using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ModularMonolithDDD.BuildingBlocks.Logging
{
    public static class Serilogger
    {
        /// <summary>
        /// Provides a configuration for Serilog.
        /// </summary>
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
            (context, configuration) =>
            {
                var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
                var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";

                configuration
                .WriteTo.Debug()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                //.WriteTo.File(new CompactJsonFormatter(), "logs/logs", rollingInterval: RollingInterval.Day)
                .WriteTo.File(
                    path: "logs/logs-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 365,
                    shared: true,                // useful for run in container/k8s
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)                
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
            };

        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {
            var bootstrapLogger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("Logs/bootstrap.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
            try
            {
                // ConfigureLogger() setup static logger
                // Get temporary logger
                Log.Logger = bootstrapLogger;
                bootstrapLogger.Information("Starting application (pre-Serilog config)...");

                // Configure Serilog as the logging provider for the application, using Serilog to replace the default logging provider
                builder.Host.UseSerilog(Serilogger.Configure);

                // Get the logger for the API module
                var _loggerForApi = Log.ForContext("Module", "API");
                _loggerForApi.Information("Logger configured");
            }
            catch (Exception ex)
            {
                bootstrapLogger.Fatal(ex, "Failed to configure Serilog");
                Console.WriteLine($"Serilog configuration failed. See Logs/bootstrap.log for details.");
                throw;
            }
        }
    }
}
