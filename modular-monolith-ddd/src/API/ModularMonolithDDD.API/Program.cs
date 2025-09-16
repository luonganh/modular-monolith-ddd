
using Autofac.Extensions.DependencyInjection;
using ModularMonolithDDD.API.Configuration.ExecutionContext;
using ModularMonolithDDD.API.Modules.UserAccess;
using ModularMonolithDDD.BuildingBlocks.Infrastructure.Emails;
using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration;
using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureLogger();
SetConnectionString();

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

// Call UserAccess module for register OpenIddict/Identity
builder.Services.AddUserAccessAuthentication(builder.Configuration);

// 3) Set default scheme (if use OpenIddict.Validation)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

var app = builder.Build();

// Registers the ExceptionHandlerMiddleware in the request processing pipeline. 
// This middleware catches unhandled exceptions, logs them, and returns a standardized error response.
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Get the Serilog logger from DI (service container)
var logger = app.Services.GetRequiredService<ILogger>();

// Get the Autofac root container from the built service provider
var container = app.Services.GetAutofacRoot();

app.UseCors(builder =>
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Initialize modules   
InitializeModules(container, logger);

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

// Seed OpenIddict default client/scope at startup
await OpenIddictSeeder.SeedAsync(
        app.Services,
        app.Configuration);

app.Run();

void SetConnectionString()
{
    var host = Environment.GetEnvironmentVariable("SQLSERVER_HOST");
    var port = Environment.GetEnvironmentVariable("SQLSERVER_HOST_PORT");
    var db = Environment.GetEnvironmentVariable("SQLSERVER_DATABASE_NAME");
    var user = Environment.GetEnvironmentVariable("SQLSERVER_USER");
    var password = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
    builder.Configuration["ConnectionStrings:AppConnectionString"] = $"Server={host},{port};Database={db};User Id={user};Password={password};TrustServerCertificate=True;";
}

void ConfigureLogger()
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

void InitializeModules(ILifetimeScope container, ILogger logger)
{
    var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
    var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

    var connectionString = builder.Configuration.GetConnectionString("OrderConnectionString");
    string fromEmail = builder.Configuration.GetSection("EmailsConfiguration:FromEmail")?.Value;
    string apiKey = builder.Configuration.GetSection("EmailsConfiguration:ApiKey")?.Value;
    string domain = builder.Configuration.GetSection("EmailsConfiguration:Domain")?.Value;
    string secretKey = builder.Configuration.GetSection("EmailsConfiguration:SecretKey")?.Value;
    string smtpServer = builder.Configuration.GetSection("EmailsConfiguration:SMTPServer")?.Value;
    string sslPort = builder.Configuration.GetSection("EmailsConfiguration:SSLPort")?.Value;
    string tlsPort = builder.Configuration.GetSection("EmailsConfiguration:TLSPort")?.Value;
    var emailsConfiguration = new EmailsConfiguration(fromEmail, apiKey, domain, secretKey, smtpServer, sslPort, tlsPort);
    string textEncryption = builder.Configuration.GetSection("Security:TextEncryptionKey")?.Value;
   
    UserAccessStartup.Initialize(
        connectionString,
        executionContextAccessor,
        logger,
        emailsConfiguration,
        textEncryption,
        null,
        null);
    
}