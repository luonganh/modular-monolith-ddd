using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
namespace ModularMonolithDDD.Tests.IntegrationTests.UserAccess.UA_LOGIN_004
{
    public class ApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Identity:Scope:Name"] = "modular-monolith-ddd-api"
                });
            });

            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(o => o.DefaultScheme = TestAuthHandler.Scheme)
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.Scheme, _ => { });
            });
        }
    }
}