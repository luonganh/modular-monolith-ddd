namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity;

public class IdentitySettings
{
    /// <summary>
    /// The default role for the identity system.   
    /// </summary>
	public string Role { get; init; } = "Admin";

    /// <summary>
    /// The default API resource for the identity system.
    /// </summary>
	public string ApiResource { get; init; } = "modular-monolith-ddd-api";

    /// <summary>
    /// The default scope in the identity system.
    /// </summary>
    public DefaultScope Scope { get; init; } = new();

    /// <summary>
    /// The default client in the identity system.
    /// </summary>
    public DefaultClient Client { get; init; } = new();
}

