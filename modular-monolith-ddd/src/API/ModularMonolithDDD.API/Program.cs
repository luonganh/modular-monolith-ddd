
//ConfigureEnvironment();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureLogger();
//SetConnectionString();
EnvironmentHelper.ConfigureEnvironment();
builder.Configuration["ConnectionStrings:AppConnectionString"] = EnvironmentHelper.GetConnectionString(new ConfigurationBuilder().Build());

// Configure IdentitySettings, used to bind the configuration to the IdentitySettings class
builder.Services.Configure<IdentitySettings>(
    builder.Configuration.GetSection("Identity"));

builder.Services.Configure<IdTokenSettings>(options =>
{
    options.Issuer = Environment.GetEnvironmentVariable("IDENTITY_TOKEN_ISSUER")
        ?? "http://localhost:8082/realms/modular-monolith-ddd";
    options.Audience = Environment.GetEnvironmentVariable("IDENTITY_TOKEN_AUDIENCE")
        ?? "admin-spa";
    options.SecretKey = Environment.GetEnvironmentVariable("IDENTITY_TOKEN_SECRET_KEY")
        ?? throw new InvalidOperationException("IDENTITY_TOKEN_SECRET_KEY is not configured");
});

builder.Services.Configure<IdpSettings>(options =>
{           
    options.IdpBaseUrl = Environment.GetEnvironmentVariable("IDP_BASE_URL") 
        ?? "http://localhost:8082";  // Default for development
    options.IdpLoginUrl = Environment.GetEnvironmentVariable("IDP_LOGIN_URL") 
        ?? "http://localhost:8082/realms/modular-monolith-ddd/protocol/openid-connect/auth";
    options.IdpAuthorizationEndpoint = Environment.GetEnvironmentVariable("IDP_AUTHORIZATION_ENDPOINT") 
        ?? "http://localhost:8082/realms/modular-monolith-ddd/protocol/openid-connect/auth";
    options.IdpTokenEndpoint = Environment.GetEnvironmentVariable("IDP_TOKEN_ENDPOINT") 
        ?? "http://localhost:8082/realms/modular-monolith-ddd/protocol/openid-connect/token";
});

// // Register AuthenticationHelper to help with authentication use OpenIddict Idp
// builder.Services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();

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

#region Keycloak Idp

// Add Keycloak JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{    
    var keycloakAuthority = Environment.GetEnvironmentVariable("KEYCLOAK_AUTHORITY") 
        ?? "http://localhost:8082/realms/modular-monolith-ddd";
    var keycloakAudience = Environment.GetEnvironmentVariable("KEYCLOAK_AUDIENCE") 
        ?? "admin-spa";
        
    options.Authority = keycloakAuthority;
    options.Audience = keycloakAudience;

    // Dev: false, Non-dev: true
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.MapInboundClaims = false; // keep "sub", "role"...
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
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

// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    var scopeName = builder.Configuration["Identity:Scope:Name"] ?? "modular-monolith-ddd-api";
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion(ctx =>
            ctx.User.Claims.Any(c => c.Type == "scope" && 
                c.Value.Split(' ').Contains(scopeName)));
    });
});

#endregion Keycloak Idp

#region OpenIddict Idp
// // Call UserAccess module for register OpenIddict/Identity with Custom Stores (Dapper)
// builder.Services.AddUserAccessAuthenticationCustomStores(builder.Configuration);
// // Configure Default authentication scheme, Authentication schemes, Authentication handler
// builder.Services.AddAuthentication(options =>
// {
//    // Set the default authentication scheme to use OpenIddict validation
//    options.DefaultScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
// })

// // Use a cookie with tbe authentication scheme named "IdentityProviderCookie" to maintain the user session at the Authorization Server. 
// //When an authenticated session exists, OpenIddict’s /connect/authorize issues the authorization code.
// .AddCookie("IdentityProviderCookie", options =>
// {
//    options.LoginPath = "/login"; // path to the login form of Identity Provider OpenIddict (OIDC) when no session exists.
//    options.Cookie.HttpOnly = true; // disallow JavaScript access to the cookie.

//    // Production (https required)
//    // The actual cookie name stored in the browser.
//    options.Cookie.Name = "__Host-idp";

//    // Must be Secure when using SameSite=None and __Host- prefix
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

//    // For cross-site OIDC redirects (spa -> IdP), use SameSite=None together with CookieSecurePolicy.Always.
//    options.Cookie.SameSite = SameSiteMode.None;

//    // Scope the cookie to the entire host. Required when using the Host- prefix (Path must be "/").     
//    options.Cookie.Path = "/";
   
//    // Longer session in development for convenience, shorter in production for security
//    options.ExpireTimeSpan = builder.Environment.IsDevelopment()
//    ? TimeSpan.FromMinutes(90)   // dev
//    : TimeSpan.FromMinutes(30);  // prod

//    // Sliding expiration: extend the cookie’s lifetime on each user activity 
//    // (resets the 90-minute window in Development, 30‑minute window in Production).
//    options.SlidingExpiration = true;       
// })

// // Use JWT Bearer for Authorization Server to generate access token
// .AddJwtBearer(options =>
// {
//    var apiResource = builder.Configuration["Identity:ApiResource"] ?? "modular-monolith-ddd-api";
//    var authority = builder.Configuration["Identity:Authority"] 
//        ?? Environment.GetEnvironmentVariable("IDENTITY_TOKEN_ISSUER")
//        ?? "https://localhost:5000";

//    options.Authority = authority;   // Issuer of OpenIddict (Authority)
//    options.Audience = apiResource; // ApiResource of OpenIddict (Audience)
//    options.RequireHttpsMetadata = true; // Require HTTPS metadata
// })
// ;

// // Add authorization with 'ApiScope' policy requiring 'api' scope
// builder.Services.AddAuthorization(options =>
// {
    
//     var scopeName = builder.Configuration["Identity:Scope:Name"] ?? "modular-monolith-ddd-api";
// 	options.AddPolicy("ApiScope", policy =>
// 	{
// 		policy.RequireAuthenticatedUser();
// 		policy.RequireAssertion(ctx =>
// 			ctx.User.Claims.Any(c => c.Type == "scope" && c.Value.Split(' ').Contains(scopeName)));
// 	});
// });

// // For render login page in OpenIddict (OIDC)
// builder.Services.AddControllersWithViews();
// builder.Services.Configure<RazorViewEngineOptions>(o =>
// {
//    // {1} = controller name (e.g., OpenIddict Authentication), {0} = action name (e.g., Login)
//    o.ViewLocationFormats.Add("/Modules/UserAccess/Views/{1}/{0}.cshtml");  
//    o.ViewLocationFormats.Add("/Modules/UserAccess/Views/Shared/{0}.cshtml"); // for _Layout.cshtml, partials
// });
#endregion OpenIddict Idp

builder.Services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(builder.Configuration["ConnectionStrings:AppConnectionString"]));

// Build the application    
var app = builder.Build();


// Configure the HTTP request pipeline.
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
var config = app.Services.GetRequiredService<IConfiguration>();
var clientUri = (config["Identity:Client:ClientUri"] ?? "http://localhost:3000").TrimEnd('/');
app.UseCors(builder =>
    builder.WithOrigins(clientUri)
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    );

// Use CorrelationMiddleware to add correlation ID to the request context
// Correlation ID helps track requests across multiple services and modules
// This is essential for distributed tracing and debugging in a modular monolith
// The middleware generates a unique ID for each request and makes it available throughout the request lifecycle
app.UseMiddleware<CorrelationMiddleware>();

#region OpenIddict middleware
// // Add custom OpenIddict middleware to log all requests to /connect/token
// app.Use(async (context, next) =>
// {
//    if (context.Request.Path.StartsWithSegments("/connect/token"))
//    {
//        Console.WriteLine($"=== Token Endpoint Request ===");
//        Console.WriteLine($"Method: {context.Request.Method}");
//        Console.WriteLine($"Path: {context.Request.Path}");
//        Console.WriteLine($"Query: {context.Request.QueryString}");
//        Console.WriteLine($"Content-Type: {context.Request.ContentType}");
//        Console.WriteLine($"Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");

//        // Read request body
//        context.Request.EnableBuffering();
//        var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
//        context.Request.Body.Position = 0;
//        Console.WriteLine($"Body: {body}");
//    }

//    await next();

//    if (context.Request.Path.StartsWithSegments("/connect/token"))
//    {
//        Console.WriteLine($"=== AFTER OpenIddict ===");
//        Console.WriteLine($"Response Status: {context.Response.StatusCode}");
//        Console.WriteLine($"Response Headers: {string.Join(", ", context.Response.Headers.Select(h => $"{h.Key}={h.Value}"))}");
//    }
// });
#endregion OpenIddict middleware

// Use Swagger middleware
// Swagger is used to document the API
// Learn more: https://swagger.io/
app.UseSwaggerDocumentation();
        
// HSTS need Cors headers to be enabled        
// Strict-Transport-Security (HSTS) header is used to inform browsers to only access the site via HTTPS        
// for one year. This helps protect against MITM attacks and ensures data is transmitted securely.        
// Learn more: https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security        
app.UseHsts();

// HTTPS Redirection need Cors headers to be enabled        
// HSTS + HTTPS redirection: not Development
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
{
    // Dev: avoid redirect to miss Authorization header when call wrong scheme/port
    Console.WriteLine("Skipping HSTS/HTTPS redirection in Development");
}

// Use ProblemDetails middleware to handle custom exceptions and return standardized error responses
// This middleware catches unhandled exceptions and converts them to RFC 7807 Problem Details format
// It provides consistent error responses across the API, making it easier for clients to handle errors
// Should be placed after CORS but before Authentication to catch all exceptions
app.UseProblemDetails();

// // (OpenIddict Idp) This is used to serve the login page
// // Use Static Files middleware to serve static files from the wwwroot folder
// app.UseStaticFiles();

app.Use(async (ctx, next) =>
{
    var auth = ctx.Request.Headers["Authorization"].ToString();
    Console.WriteLine($"Auth header (server sees): {(string.IsNullOrEmpty(auth) ? "<empty>" : auth[..Math.Min(auth.Length, 40)])}");
    await next();
});

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

// OpenIddict Idp
// Start a new Autofac lifetime scope from the UserAccess module entry point
// using var scope = UserAccessStartup.BeginLifetimeScope();
// // Resolve the scoped IServiceProvider from the Autofac lifetime scope
// var sp = scope.Resolve<IServiceProvider>();
// // Seed OpenIddict default client/scope at startup
// await OpenIddictCustomStoreSeeder.SeedAsync(
//        sp,
//        app.Configuration);

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
    Console.WriteLine($"=====SQL: {host},{port}");
   
    string connectionString;
    
    if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(port) && 
        !string.IsNullOrEmpty(db) && !string.IsNullOrEmpty(user) && 
        !string.IsNullOrEmpty(password))
    {
        // Use environment variables (Docker/Production)
        connectionString = $"Server={host},{port};Database={db};User Id={user};Password={password};TrustServerCertificate=True;Max Pool Size=100;Min Pool Size=5;Connection Timeout=30;Pooling=true;";
        Console.WriteLine("Using environment variables for connection string");
    }
    else
    {
        // Use appsettings.json (Local Development)
        var configConnectionString = builder.Configuration.GetConnectionString("AppConnectionString");
        if (string.IsNullOrEmpty(configConnectionString))
        {
            throw new InvalidOperationException("Connection string not found in appsettings.json and environment variables are not set.");
        }
        connectionString = configConnectionString;
        Console.WriteLine("Using appsettings.json for connection string");
    }
    
    builder.Configuration["ConnectionStrings:AppConnectionString"] = connectionString;   
}

void ConfigureEnvironment()
{
    // Load .env by environment
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";    

    // Checkin if API run in Docker
    // Get .env container path from root solution
    var rootPath = Directory.GetCurrentDirectory();
    
    // If in Docker, .env file will have copy into /app (app folder in docker container)
    if (Directory.Exists("/app"))
    {
        // Production environment
        rootPath = "/app";        
    }
    else
    {
        // Local development
        rootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
    }
    

    // The priority: .env.{Environment} -> .env
    var envFiles = environment == "Development" ? new[] { ".env" } : new[] { $".env.{environment.ToLower()}", ".env" };

    foreach (var envFile in envFiles)
    {
        var envPath = Path.Combine(rootPath, envFile);        
        if (File.Exists(envPath))
        {
            Env.Load(envPath);           
            break;
        }
    }    
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

    var connectionString = builder.Configuration.GetConnectionString("AppConnectionString") ?? throw new InvalidOperationException("Connection string 'AppConnectionString' not found in configuration"); ;
    //string fromEmail = builder.Configuration["EmailsConfiguration:FromEmail"] ?? throw new InvalidOperationException("Emails:From configuration is required");
    //string apiKey = builder.Configuration["EmailsConfiguration:ApiKey"] ?? throw new InvalidOperationException("Emails:ApiKey configuration is required");
    //string domain = builder.Configuration["EmailsConfiguration:Domain"] ?? throw new InvalidOperationException("Emails:Domain configuration is required");
    //string secretKey = builder.Configuration["EmailsConfiguration:SecretKey"] ?? throw new InvalidOperationException("Emails:SecretKey configuration is required");
    //string smtpServer = builder.Configuration["EmailsConfiguration:SMTPServer"] ?? throw new InvalidOperationException("Emails:SMTPServer configuration is required");
    //string sslPort = builder.Configuration["EmailsConfiguration:SSLPort"] ?? throw new InvalidOperationException("Emails:SSLPort configuration is required");
    //string tlsPort = builder.Configuration["EmailsConfiguration:TLSPort"] ?? throw new InvalidOperationException("Emails:TLSPort configuration is required");
    //var emailsConfiguration = new EmailsConfiguration(fromEmail, apiKey, domain, secretKey, smtpServer, sslPort, tlsPort);
    //string textEncryption = builder.Configuration["Security:TextEncryptionKey"] ?? throw new InvalidOperationException("Security:TextEncryptionKey configuration is required");
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