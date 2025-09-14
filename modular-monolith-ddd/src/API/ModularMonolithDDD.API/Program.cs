
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureLogger();
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

var app = builder.Build();

// Registers the ExceptionHandlerMiddleware in the request processing pipeline. 
// This middleware catches unhandled exceptions, logs them, and returns a standardized error response.
app.UseMiddleware<ExceptionHandlerMiddleware>();



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

app.UseAuthorization();

app.MapControllers();

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