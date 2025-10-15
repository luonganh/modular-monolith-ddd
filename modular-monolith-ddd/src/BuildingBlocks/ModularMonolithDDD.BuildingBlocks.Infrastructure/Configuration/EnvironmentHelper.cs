namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.Configuration
{
    public static class EnvironmentHelper
    {
        public static void ConfigureEnvironment(string? rootPath = null)
        {
            // Load .env by environment
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // If rootPath has value
            var basePath = rootPath;

            // If rootPath has novalue, check /app (Docker), if in Docker, .env file will have copy into /app (app folder in docker container)
            if (string.IsNullOrWhiteSpace(basePath) && Directory.Exists("/app"))
                basePath = "/app";
            else
            {
                basePath = FindSolutionRoot(Directory.GetCurrentDirectory(), "ModularMonolithDDD.sln");
            }           
            Console.WriteLine($"GET .ENV PATH: {basePath}");
            // Finally, fallback current directory
            basePath ??= Directory.GetCurrentDirectory();
           
            // Checkin if API run in Docker
            // Get .env container path from root solution
            //var rootPath = Directory.GetCurrentDirectory();

            //// If in Docker, .env file will have copy into /app (app folder in docker container)
            //if (Directory.Exists("/app"))
            //{
            //    // Production environment
            //    rootPath = "/app";
            //}
            //else
            //{
            //    // Local development
            //    rootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
            //}

            // The priority: .env.{Environment} -> .env
            var envFiles = environment == "Development" ? new[] { ".env" } : new[] { $".env.{environment.ToLower()}", ".env" };

            foreach (var envFile in envFiles)
            {
                var envPath = Path.Combine(basePath, envFile);
                if (File.Exists(envPath))
                {
                    Env.Load(envPath);
                    return;
                }
            }
        }

        private static string? FindSolutionRoot(string startDir, string slnName)
        {
            var dir = new DirectoryInfo(startDir);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, slnName)))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return null;
        }

        public static string GetConnectionString(IConfiguration configuration)
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
                connectionString = configuration.GetConnectionString("AppConnectionString");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not found in appsettings.json and environment variables are not set.");
                }                
                Console.WriteLine("Using appsettings.json for connection string");
            }            
            return connectionString;
        }
    }
}
