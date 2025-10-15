using ModularMonolithDDD.Modules.UserAccess.Application.Contracts;
using ModularMonolithDDD.Modules.UserAccess.Application.Users.ProvisionUserFromClaims;
using ModularMonolithDDD.Modules.UserAccess.Domain.Users;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolithDDD.API.Middlewares
{
    public class ProvisioningMiddleware
    {
        private readonly RequestDelegate _next;
        public ProvisioningMiddleware(RequestDelegate next) { _next = next; }

        public async Task Invoke(HttpContext ctx, IUserAccessModule userAccess)
        {
            var result = await ctx.AuthenticateAsync();
            if (result.Succeeded)
            {
                var user = ctx.User;
                if (user?.Identity?.IsAuthenticated == true)
                {
                    var sub = user.FindFirst("sub")?.Value;
                    if (!string.IsNullOrEmpty(sub))
                    {
                        var email = user.FindFirst("email")?.Value ?? "luonganh@gmail.com";
                        var username = user.FindFirst("preferred_username")?.Value ?? user.Identity?.Name ?? sub;
                        var display = user.FindFirst("name")?.Value ?? username ?? "Anh Luong";
                        var firstName = user.FindFirst("given_name")?.Value ?? "Anh";
                        var lastName = user.FindFirst("family_name")?.Value ?? "Luong";
                        // Extract roles from realm_access
                        var roles = ExtractRolesFromClaims(user);

                        await userAccess.ExecuteCommandAsync(
                            new ProvisionUserFromClaimsCommand(
                                sub,
                                username,
                                email,
                                firstName,
                                lastName,
                                display,
                                roles
                            )
                        );
                    }
                }
            }            
            await _next(ctx);
        }

        private List<string> ExtractRolesFromClaims(ClaimsPrincipal user)
        {
            var roles = new List<string>();           
            // Method 1: From realm_access claim (JSON)
            var realmAccessClaim = user.FindFirst("realm_access");
            if (realmAccessClaim != null)
            {
                try
                {
                    var realmAccess = JsonSerializer.Deserialize<RealmAccess>(realmAccessClaim.Value);
                    if (realmAccess?.Roles?.Any(x => x.Equals("Administrator", StringComparison.OrdinalIgnoreCase)
                    || x.Equals("Member", StringComparison.OrdinalIgnoreCase)) == true)
                    {
                        roles.AddRange(realmAccess?.Roles?.FirstOrDefault(x => 
                        x.Equals("Administrator", StringComparison.OrdinalIgnoreCase)
                        || x.Equals("Member", StringComparison.OrdinalIgnoreCase)
                        ));
                    }
                }
                catch (JsonException)
                {
                    // Fallback: parse manually
                }
            }

            // Method 2: From individual role claims
            var roleClaims = user.FindAll("role");
            foreach (var roleClaim in roleClaims)
            {
                if (!string.IsNullOrEmpty(roleClaim.Value))
                {
                    roles.Add(roleClaim.Value);
                }
            }

            // Method 3: From custom claims
            var customRoles = user.FindAll("custom_roles");
            foreach (var customRole in customRoles)
            {
                if (!string.IsNullOrEmpty(customRole.Value))
                {
                    roles.Add(customRole.Value);
                }
            }

            return roles.Distinct().ToList();
        }        
    }

    // Helper class for deserialize realm_access
    public class RealmAccess
    {
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();
    }

}