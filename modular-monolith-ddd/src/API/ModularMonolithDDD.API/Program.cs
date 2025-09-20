
// Load .env file for local development - MUST be first!
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureLogger();
SetConnectionString();

// Configure the app to use Autofac as the DI container (service provider factory) instead of the default Microsoft DI
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Autofac module is used to register the modules
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new UserAccessAutofacModule());
   
});

// Register DbContext as a scoped service in the DI container
// This registration allows controllers and services to inject DbContext directly
// The factory function resolves IDomainDbContext from the container and casts it to DbContext
// In a modular monolith, this ensures only one DbContext is used across all modules
// If multiple DbContexts are found, it throws an exception to prevent ambiguity
builder.Services.AddScoped<DbContext>(provider =>
{
    var dbContexts = provider.GetServices<IDomainDbContext>();
    if (dbContexts.Count() > 1)
    {
        throw new InvalidOperationException("Multiple DbContexts found. Specify which one to use.");
    }
    return (DbContext)dbContexts.FirstOrDefault()!;
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


// Configure ProblemDetails middleware to handle custom exceptions and return standardized error responses
builder.Services.AddProblemDetails(configure =>
{
    // Map InvalidCommandException to 400 Bad Request with custom title and detail
    configure.Map<InvalidCommandException>(ex => new StatusCodeProblemDetails(400)
    {
        Title = "Invalid Command",
        Detail = ex.Message
    });
    // Map BusinessRuleValidationException to 400 Bad Request with custom title and detail
    configure.Map<BusinessRuleValidationException>(ex => new StatusCodeProblemDetails(400)
    {
        Title = "Business Rule Validation Error",
        Detail = ex.Message
    });
});

// Register controllers
builder.Services.AddControllers();

// Call UserAccess module for register OpenIddict/Identity with Custom Stores (Dapper)
builder.Services.AddUserAccessAuthenticationCustomStores(builder.Configuration);

// 3) Set default scheme (if use OpenIddict.Validation)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

// Build the application    
var app = builder.Build();

// Registers the ExceptionHandlerMiddleware in the request processing pipeline. 
// This middleware catches unhandled exceptions, logs them, and returns a standardized error response.
// Should be placed after CORS but before Authentication to catch auth-related exceptions
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Get the Serilog logger from DI (service container)
var logger = app.Services.GetRequiredService<ILogger>();

// Get the Autofac root container from the built service provider
var container = app.Services.GetAutofacRoot();

// Initialize modules   
InitializeModules(container);


// Use CORS middleware
// CORS is used to allow cross-origin requests
// Must be placed before Authentication to handle preflight requests
app.UseCors(builder =>
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Use CorrelationMiddleware to add correlation ID to the request context
// Correlation ID helps track requests across multiple services and modules
// This is essential for distributed tracing and debugging in a modular monolith
// The middleware generates a unique ID for each request and makes it available throughout the request lifecycle
app.UseMiddleware<CorrelationMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Use Swagger middleware
    // Swagger is used to document the API
    // Learn more: https://swagger.io/
    app.UseSwaggerDocumentation();
}
else
{    
    // HSTS need Cors headers to be enabled
    // Strict-Transport-Security (HSTS) header is used to inform browsers to only access the site via HTTPS
    // for one year. This helps protect against MITM attacks and ensures data is transmitted securely.
    // Learn more: https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security
    app.UseHsts();

     // HTTPS Redirection need Cors headers to be enabled
    app.UseHttpsRedirection();
}

// Use ProblemDetails middleware to handle custom exceptions and return standardized error responses
// This middleware catches unhandled exceptions and converts them to RFC 7807 Problem Details format
// It provides consistent error responses across the API, making it easier for clients to handle errors
// Should be placed after CORS but before Authentication to catch all exceptions
app.UseProblemDetails();


// Use Authentication middleware
// Authentication is used to authenticate the user
app.UseAuthentication();

// Use Authorization middleware
// Authorization is used to authorize the user
app.UseAuthorization();

// Map controllers to the request pipeline
// Controllers are used to handle the request and return the response
app.MapControllers();

// Start a new Autofac lifetime scope from the UserAccess module entry point
using var scope = UserAccessStartup.BeginLifetimeScope();

// Resolve the scoped IServiceProvider from the Autofac lifetime scope
var sp = scope.Resolve<IServiceProvider>();

// Seed OpenIddict default client/scope at startup
await OpenIddictSeeder.SeedAsync(
        sp,
        app.Configuration);

app.Run();

// Set connection string
// Connection string is used to connect to the database
void SetConnectionString()
{
    var host = Environment.GetEnvironmentVariable("SQLSERVER_HOST");
    var port = Environment.GetEnvironmentVariable("SQLSERVER_HOST_PORT");
    var db = Environment.GetEnvironmentVariable("SQLSERVER_DATABASE_NAME");
    var user = Environment.GetEnvironmentVariable("SQLSERVER_USER");
    var password = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
    
    string connectionString;
    
    if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(port) && 
        !string.IsNullOrEmpty(db) && !string.IsNullOrEmpty(user) && 
        !string.IsNullOrEmpty(password))
    {
        // Use environment variables (Docker/Production)
        connectionString = $"Server={host},{port};Database={db};User Id={user};Password={password};TrustServerCertificate=True;";
        Console.WriteLine("Using environment variables for connection string");
    }
    else
    {
        // Use appsettings.json (Local Development)
        connectionString = builder.Configuration.GetConnectionString("AppConnectionString");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string not found in appsettings.json and environment variables are not set.");
        }
        Console.WriteLine("Using appsettings.json for connection string");
    }
    
    builder.Configuration["ConnectionStrings:AppConnectionString"] = connectionString;
    Console.WriteLine("Connection string: " + connectionString);
}

void ConfigureLogger()
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
    }    
}

// Initialize modules
// Modules are used to handle the request and return the response
void InitializeModules(ILifetimeScope container)
{
    var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
    var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

    var connectionString = builder.Configuration.GetConnectionString("AppConnectionString");
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