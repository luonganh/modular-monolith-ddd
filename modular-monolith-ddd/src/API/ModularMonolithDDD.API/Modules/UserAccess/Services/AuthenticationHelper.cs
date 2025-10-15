namespace ModularMonolithDDD.API.Modules.UserAccess.Services
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly IdpSettings _idp;
        public AuthenticationHelper(IOptions<IdpSettings> idp) => _idp = idp.Value;

        /// <summary>
        /// Checks if user is authenticated via cookie authentication
        /// </summary>
        /// <param name="httpContext">The HTTP context</param>
        /// <returns>True if user is authenticated, false otherwise</returns>
        public async Task<bool> IsUserAuthenticatedAsync(HttpContext httpContext)
        {
            try
            {
                // Check if user is authenticated via cookie
                var result = await httpContext.AuthenticateAsync("IdentityProviderCookie");
                
                if (result.Succeeded && result.Principal?.Identity?.IsAuthenticated == true)
                {
                    // Set the authenticated user in HttpContext
                    httpContext.User = result.Principal;
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication check failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IActionResult RedirectToLogin(Controller controller, string? returnUrl = null)
        {
            var currentUrl = controller.Request.GetEncodedPathAndQuery(); // GetEncodedUrl()
            var urlToReturn = returnUrl ?? currentUrl;            
            return controller.Redirect($"{_idp.IdpLoginUrl}?returnUrl={Uri.EscapeDataString(urlToReturn)}");
        }

        /// <summary>
        /// Checks authentication and redirects to login if not authenticated
        /// </summary>
        /// <param name="controller">The controller instance</param>
        /// <param name="returnUrl">Optional return URL</param>
        /// <returns>Null if authenticated, RedirectResult if not authenticated</returns>
        public async Task<IActionResult?> CheckAuthenticationAndRedirectAsync(Controller controller, string? returnUrl = null)
        {
            if (!await IsUserAuthenticatedAsync(controller.HttpContext))
            {
                return RedirectToLogin(controller, returnUrl);
            }
            return null; // User is authenticated
        }

       
    }
}
