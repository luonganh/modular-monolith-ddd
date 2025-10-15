namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity;
public class DefaultClient
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string ClientType { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string ClientUri { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
    public string PostLogoutRedirectUri { get; set; } = string.Empty;
}