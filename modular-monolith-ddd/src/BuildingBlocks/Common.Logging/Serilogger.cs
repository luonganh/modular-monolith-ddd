using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Logging
{
    public class Serilogger
    {
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
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
            };
    }
}
