

// Configure services and register dependencies
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureLogger();

try
{
    SetConnectionString();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    // Enable API explorer for endpoint metadata (required for Swagger)
    builder.Services.AddEndpointsApiExplorer();

    // Register custom Swagger documentation configuration
    builder.Services.AddSwaggerDocumentation();

    // Add support for Newtonsoft.Json in Swagger (e.g., handling polymorphic serialization)
    builder.Services.AddSwaggerGenNewtonsoftSupport();

    // Register IHttpContextAccessor to allow access to HttpContext in services, non-controller classes, background tasks,
    // or singleton services (where direct dependency injection of HttpContext is not possible)	
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddControllers();

    // Configure the HTTP request pipeline.
    var app = builder.Build();

    // Registers the ExceptionHandlerMiddleware in the request processing pipeline. 
    // This middleware catches unhandled exceptions, logs them, and returns a standardized error response.
    app.UseMiddleware<ExceptionHandlerMiddleware>();

    // Get the Serilog logger from DI (service container)
    var logger = app.Services.GetRequiredService<ILogger>();

    app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
    {
        app.UseSwaggerDocumentation();
    }    
    else
    {
        // Disabled Swagger in production
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.CloseAndFlush();
}

void ConfigureLogger()
{
    var bootstrapLogger = new LoggerConfiguration()
    .WriteTo.File("Logs/bootstrap.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

    try
    {
        // Get temporary logger
        Log.Logger = bootstrapLogger;
        bootstrapLogger.Information("Starting application (pre-Serilog config)...");

        // Configure Serilog as the logging provider for the application, using Serilog to replace the default logging provider
        builder.Host.UseSerilog(Serilogger.Configure);
    }
    catch (Exception ex)
    {
        bootstrapLogger.Fatal(ex, "Failed to configure Serilog");
        Console.WriteLine($"Serilog configuration failed. See Logs/bootstrap.log for details.");
    }
}

void SetConnectionString()
{
    var host = Environment.GetEnvironmentVariable("SQLSERVER_HOST");
    var port = Environment.GetEnvironmentVariable("SQLSERVER_HOST_PORT");
    var db = Environment.GetEnvironmentVariable("SQLSERVER_DATABASE_NAME");
    var user = Environment.GetEnvironmentVariable("SQLSERVER_USER");
    var password = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
    builder.Configuration.GetSection("ConnectionStrings")["AppConnectionString"] = $"Server={host},{port};Database={db};User Id={user};Password={password};TrustServerCertificate=True;";       
}