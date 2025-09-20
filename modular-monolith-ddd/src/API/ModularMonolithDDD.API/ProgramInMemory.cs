using Autofac.Extensions.DependencyInjection;
using ModularMonolithDDD.API.Configuration.ExecutionContext;
using ModularMonolithDDD.API.Modules.UserAccess;
using ModularMonolithDDD.BuildingBlocks.Infrastructure.Emails;
using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration;
using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity;
using Serilog;
using Serilog.Formatting.Compact;

namespace ModularMonolithDDD.API;

/// <summary>
/// Program entry point for the Modular Monolith DDD API using OpenIddict with In-Memory stores.
/// This version does not use Entity Framework Core for OpenIddict data storage.
/// </summary>
public class ProgramInMemory
{
    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigureLogger();
        SetConnectionString(builder);

// Configure the app to use Autofac as the DI container (service provider factory) instead of the default Microsoft DI
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register Autofac module
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new UserAccessAutofacModule());
   
});

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

// Register ExecutionContextAccessor to retrieve user, tenant, or request-related information 
// if it depends on IHttpContextAccessor.
// In modular monolith, it helps extract UserId, CorrelationId, request context from the request.
builder.Services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();

builder.Services.AddControllers();


// 3) Set default scheme (if use OpenIddict.Validation)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

var app = builder.Build();

// Registers the ExceptionHandlerMiddleware in the request processing pipeline. 
// This middleware catches unhandled exceptions, logs them, and returns a standardized error response.
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Get the Autofac root container from the built service provider
var container = app.Services.GetAutofacRoot();

app.UseCors(builder =>
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        // Initialize modules   
        InitializeModules(container, builder);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwaggerDocumentation();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed OpenIddict default client/scope at startup using In-Memory stores
await OpenIddictSeeder.SeedAsync(
        app.Services,
        app.Configuration);

        app.Run();
    }

    private static void SetConnectionString(WebApplicationBuilder builder)
    {
        var host = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("SQLSERVER_HOST_PORT") ?? "1433";
        var db = Environment.GetEnvironmentVariable("SQLSERVER_DATABASE_NAME") ?? "ModularMonolithDDD";
        var user = Environment.GetEnvironmentVariable("SQLSERVER_USER") ?? "sa";
        var password = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD") ?? "YourPassword123!";
        builder.Configuration["ConnectionStrings:AppConnectionString"] = $"Server={host},{port};Database={db};User Id={user};Password={password};TrustServerCertificate=True;";
    }

    private static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
            .CreateLogger();
        var _loggerForApi = Log.ForContext("Module", "API");
        _loggerForApi.Information("Logger configured");
    }

    private static void InitializeModules(ILifetimeScope container, WebApplicationBuilder builder)
    {
        var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
        var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

        var connectionString = builder.Configuration.GetConnectionString("AppConnectionString") ?? "DefaultConnection";
        string fromEmail = builder.Configuration.GetSection("EmailsConfiguration:FromEmail")?.Value ?? "noreply@example.com";
        string apiKey = builder.Configuration.GetSection("EmailsConfiguration:ApiKey")?.Value ?? "default-api-key";
        string? domain = builder.Configuration.GetSection("EmailsConfiguration:Domain")?.Value;
        string? secretKey = builder.Configuration.GetSection("EmailsConfiguration:SecretKey")?.Value;
        string? smtpServer = builder.Configuration.GetSection("EmailsConfiguration:SMTPServer")?.Value;
        string? sslPort = builder.Configuration.GetSection("EmailsConfiguration:SSLPort")?.Value;
        string? tlsPort = builder.Configuration.GetSection("EmailsConfiguration:TLSPort")?.Value;
        var emailsConfiguration = new EmailsConfiguration(fromEmail, apiKey, domain, secretKey, smtpServer, sslPort, tlsPort);
        string textEncryption = builder.Configuration.GetSection("Security:TextEncryptionKey")?.Value ?? "default-encryption-key";
       
        UserAccessStartup.Initialize(
            connectionString,
            executionContextAccessor,
            Log.Logger,
            emailsConfiguration,
            textEncryption,
            null!,
            null!);
    }
}
