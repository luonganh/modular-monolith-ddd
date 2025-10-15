using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace ModularMonolithDDD.Tests.IntegrationTests.UserAccess.UA_LOGIN_004
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public new const string Scheme = "Test";

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var mode = Request.Headers["X-Test-Auth"].ToString();
            if (string.Equals(mode, "none", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.NoResult());

            var identity = new ClaimsIdentity(Scheme);

            var name = Request.Headers["X-Test-Name"].ToString();
            if (!string.IsNullOrEmpty(name))
                identity.AddClaim(new Claim(ClaimTypes.Name, name));

            var scopes = Request.Headers["X-Test-Scopes"].ToString();
            if (!string.IsNullOrEmpty(scopes))
                identity.AddClaim(new Claim("scope", scopes));

            if (identity.Claims.Any())
            {
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("No claims"));
        }
    }
}