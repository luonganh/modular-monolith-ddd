using Microsoft.Extensions.Caching.Memory;

namespace Mmdddshop.API.Middlewares
{
    /// <summary>
    /// Keeps the local UserAccess copy of the authenticated Keycloak user in sync.
    /// Syncs only when the relevant claims changed (or the cache entry expired),
    /// and never fails the request because of a provisioning error.
    /// </summary>
    public class ProvisioningMiddleware
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        // Serializes provisioning per user (striped: bounded memory, no cleanup needed).
        private static readonly SemaphoreSlim[] ProvisioningLocks =
        Enumerable.Range(0, 64).Select(_ => new SemaphoreSlim(1, 1)).ToArray();

        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ProvisioningMiddleware> _logger;

        public ProvisioningMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<ProvisioningMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
        }

        public async Task Invoke(HttpContext ctx, IUserAccessModule userAccess)
        {
            var user = ctx.User;
            var sub = user.FindFirst("sub")?.Value;

            if (user.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(sub))
            {
                var command = CreateCommand(user, sub);
                var cacheKey = $"user-provisioning:{sub}";
                var fingerprint = CreateFingerprint(command);

                if (!IsUpToDate(cacheKey, fingerprint))
                {
                    var gate = ProvisioningLocks[(uint)sub.GetHashCode() % ProvisioningLocks.Length];
                    await gate.WaitAsync(ctx.RequestAborted);
                    try
                    {
                        // Double-check: a request we waited for may have just synced the same user.
                        if (!IsUpToDate(cacheKey, fingerprint))
                        {
                            try
                            {
                                await userAccess.ExecuteCommandAsync(command);
                                _cache.Set(cacheKey, fingerprint, CacheDuration);
                            }
                            catch (Exception ex) when (ex is not OperationCanceledException)
                            {
                                // Still possible with several API instances (the lock is per process).
                                _logger.LogWarning(ex, "User provisioning failed for sub {Sub}; the request continues.", sub);
                            }
                        }
                    }
                    finally
                    {
                        gate.Release();
                    }
                }               
            }

            await _next(ctx);
        }

        private bool IsUpToDate(string cacheKey, string fingerprint) =>
            _cache.TryGetValue(cacheKey, out string? cached) && cached  == fingerprint;
            
        private static ProvisionUserFromClaimsCommand CreateCommand(ClaimsPrincipal user, string sub) =>
            new(
                externalId: sub,
                username: user.FindFirst("preferred_username")?.Value ?? sub,
                email: user.FindFirst("email")?.Value,
                firstName: user.FindFirst("given_name")?.Value,
                lastName: user.FindFirst("family_name")?.Value,
                displayName: user.FindFirst("name")?.Value,
                roles: ExtractRolesFromClaims(user));

        /// <summary>
        /// Everything that SyncProfile compares. If any of it changes, the fingerprint changes.
        /// </summary>
        private static string CreateFingerprint(ProvisionUserFromClaimsCommand command)
        {
            var roles = command.Roles is null
                ? "<no-role-claim>"
                : string.Join(",", command.Roles.Order(StringComparer.OrdinalIgnoreCase));

            return string.Join("\u001F",
                command.Username, command.Email, command.FirstName, command.LastName, command.DisplayName, roles);
        }
       

        private static List<string>? ExtractRolesFromClaims(ClaimsPrincipal user)
        {
            // No realm_access → the token carries no role information → do not sync roles.
            if (!user.HasClaim(c => c.Type == "realm_access"))
            {
                return null;
            }

            return user.FindAll(KeycloakRolesClaimsTransformation.RoleClaimType)
                .Select(c => c.Value)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();            
        }        
    }   

}