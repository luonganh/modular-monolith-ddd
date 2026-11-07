
// Create the web application builder and load default configuration, logging, and DI services
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
Serilogger.ConfigureLogging(builder);
EnvironmentHelper.ConfigureEnvironment();

// Set connection string
// Connection string is used to connect to the database
builder.Configuration["ConnectionStrings:AppConnectionString"] = EnvironmentHelper.GetConnectionString(builder.Configuration);

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
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


#region Keycloak Idp

// Add Keycloak JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var keycloakAuthority = Environment.GetEnvironmentVariable("KEYCLOAK_AUTHORITY")
        ?? "http://localhost:8082/realms/mmdddshop";
    var keycloakAudience = Environment.GetEnvironmentVariable("KEYCLOAK_AUDIENCE")
        ?? "mmdddshop-backend-api";
    var keycloakMetadataAddress = Environment.GetEnvironmentVariable("KEYCLOAK_METADATA_ADDRESS")
        ?? "http://keycloak:8080/realms/mmdddshop/.well-known/openid-configuration";

    options.Authority = keycloakAuthority;  // public issuer expected
    options.Audience = keycloakAudience;   
    options.MetadataAddress = keycloakMetadataAddress;  // internal metadata URL
    
    // Dev: false, Non-dev: true
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.MapInboundClaims = false; // keep "sub", "role"...
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = keycloakAuthority,

        ValidateAudience = true,
        ValidAudience = keycloakAudience,

        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromMinutes(2)
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine($"AuthFailed: {ctx.Exception}");
            return Task.CompletedTask;
        },
        OnChallenge = ctx =>
        {
            Console.WriteLine($"Challenge: {ctx.Error} - {ctx.ErrorDescription}");
            return Task.CompletedTask;
        },
        OnMessageReceived = ctx =>
        {
            Console.WriteLine($"Token received? {(!string.IsNullOrEmpty(ctx.Token))}");
            return Task.CompletedTask;
        },
        OnTokenValidated = ctx =>
        {
            Console.WriteLine("Token validated OK");
            return Task.CompletedTask;
        }
    };
});

// Register authorization services for controller and endpoint authorization.
builder.Services.AddAuthorization();

#endregion Keycloak Idp

builder.Services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(builder.Configuration["ConnectionStrings:AppConnectionString"] ?? string.Empty));


// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.

// Use CorrelationMiddleware to add correlation ID to the request context
// Correlation ID helps track requests across multiple services and modules
// This is essential for distributed tracing and debugging in a modular monolith
// The middleware generates a unique ID for each request and makes it available throughout the request lifecycle
app.UseMiddleware<CorrelationMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Dev: avoid redirect to miss Authorization header when call wrong scheme/port
    Console.WriteLine("Skipping HSTS/HTTPS redirection in Development");
}
else
{
    // Strict-Transport-Security (HSTS) header
    // HSTS tells browsers to use HTTPS for future requests.       
    // This helps protect against MITM attacks and ensures data is transmitted securely for one year.           
    app.UseHsts();

    // HTTPS redirection redirects the current HTTP request to HTTPS.
    app.UseHttpsRedirection();
}

// Use CORS middleware
// CORS is used to allow cross-origin requests
// Must be placed before Authentication to handle preflight requests
var config = app.Services.GetRequiredService<IConfiguration>();
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

app.UseCors(builder =>
    builder.WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod()   
    );

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

// Use Swagger middleware
// Swagger is used to document the API
// Learn more: https://swagger.io/
app.UseSwaggerDocumentation();


// Use ProblemDetails middleware to handle custom exceptions and return standardized error responses
// This middleware catches unhandled exceptions and converts them to RFC 7807 Problem Details format
// It provides consistent error responses across the API, making it easier for clients to handle errors
// Should be placed after CORS but before Authentication to catch all exceptions
app.UseProblemDetails();

// Debug middleware: logs whether the server received the Authorization header.
// Remove or disable in production because it may expose sensitive token data.
if (app.Environment.IsDevelopment())
{   
    app.Use(async (ctx, next) =>
    {
        var auth = ctx.Request.Headers["Authorization"].ToString();
        Console.WriteLine($"Auth header (server sees): {(string.IsNullOrEmpty(auth) ? "<empty>" : auth[..Math.Min(auth.Length, 40)])}");
        await next();
    });
}

// Use Authentication middleware
// Authentication is used to authenticate the user
app.UseAuthentication();

#region Keycloak Idp
// Use ProvisioningMiddleware to provision the user from Keycloak Idp
app.UseMiddleware<ProvisioningMiddleware>();
#endregion Keycloak Idp

// Use Authorization middleware
// Authorization is used to authorize the user
app.UseAuthorization();

// Map controllers to the request pipeline
// Controllers are used to handle the request and return the response
app.MapControllers();

app.Run();

// Initialize modules
// Modules are used to handle the request and return the response
void InitializeModules(ILifetimeScope container)
{
    var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
    var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

    var connectionString = builder.Configuration.GetConnectionString("AppConnectionString") ?? throw new InvalidOperationException("Connection string 'AppConnectionString' not found in configuration");

    string fromEmail = builder.Configuration["EmailsConfiguration:FromEmail"] ?? string.Empty;
    string apiKey = builder.Configuration["EmailsConfiguration:ApiKey"] ?? string.Empty;
    string domain = builder.Configuration["EmailsConfiguration:Domain"] ?? string.Empty;
    string secretKey = builder.Configuration["EmailsConfiguration:SecretKey"] ?? string.Empty;
    string smtpServer = builder.Configuration["EmailsConfiguration:SMTPServer"] ?? string.Empty;
    string sslPort = builder.Configuration["EmailsConfiguration:SSLPort"] ?? string.Empty;
    string tlsPort = builder.Configuration["EmailsConfiguration:TLSPort"] ?? string.Empty;
    var emailsConfiguration = new EmailsConfiguration(fromEmail, apiKey, domain, secretKey, smtpServer, sslPort, tlsPort);
    string textEncryption = builder.Configuration["Security:TextEncryptionKey"] ?? string.Empty;
    UserAccessStartup.Initialize(
        connectionString,
        executionContextAccessor,
        logger,
        emailsConfiguration,
        textEncryption,
        null,
        null);
}

/// <summary>
/// Partial class declaration for the Program class to enable integration testing.
/// This allows test projects to access the Program class and its configuration
/// for setting up test web applications using WebApplicationFactory.
/// </summary>
public partial class Program { }
