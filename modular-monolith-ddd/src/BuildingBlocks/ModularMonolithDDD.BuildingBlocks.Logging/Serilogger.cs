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
    }
}
