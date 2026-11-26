namespace Mmdddshop.API.Configuration.Authorization
{
    /// <summary>
    /// Maps Keycloak realm roles (JSON claim "realm_access") to individual "role" claims
    /// so that [Authorize(Roles = ...)] and RequireRole(...) work.
    /// </summary>
    public sealed class KeycloakRolesClaimsTransformation : IClaimsTransformation
    {
        public const string RoleClaimType = "role";
        private const string RealmAccessClaimType = "realm_access";

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Can be called more than once per request → must be idempotent.
            if (principal.Identity is not ClaimsIdentity { IsAuthenticated: true } identity
                || identity.HasClaim(c => c.Type == RoleClaimType))
            {
                return Task.FromResult(principal);
            }

            var realmAccess = identity.FindFirst(RealmAccessClaimType)?.Value;
            if (string.IsNullOrEmpty(realmAccess))
            {
                return Task.FromResult(principal);
            }

            try
            {
                using var document = JsonDocument.Parse(realmAccess);
                if (document.RootElement.TryGetProperty("roles", out var roles)
                    && roles.ValueKind == JsonValueKind.Array)
                {
                    foreach (var role in roles.EnumerateArray())
                    {
                        if (role.GetString() is { Length: > 0 } value)
                        {
                            identity.AddClaim(new Claim(RoleClaimType, value));
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // Malformed claim → no roles; authorization will deny role-protected endpoints.
            }

            return Task.FromResult(principal);
        }
    }
}