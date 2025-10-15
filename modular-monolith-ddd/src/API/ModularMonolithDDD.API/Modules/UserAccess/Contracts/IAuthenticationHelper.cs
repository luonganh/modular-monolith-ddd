namespace ModularMonolithDDD.API.Modules.UserAccess.Contracts
{
    public interface IAuthenticationHelper
    {
        Task<bool> IsUserAuthenticatedAsync(HttpContext httpContext);

        IActionResult RedirectToLogin(Controller controller, string? returnUrl = null);

        Task<IActionResult?> CheckAuthenticationAndRedirectAsync(Controller controller, string? returnUrl = null);
    }
}
