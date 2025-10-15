namespace ModularMonolithDDD.API.Modules.UserAccess
{
    /// <summary>
    /// OpenIddict IdP
    /// Controller responsible for user authentication.
    /// </summary>    
    public class OpenIddictController : Controller
    {
        private readonly IUserAccessModule _userAccessModule;
        private readonly IdTokenSettings _idTokenSettings;
        private readonly IdentitySettings _identitySettings;

        private readonly string _clientBaseUri;

        private readonly IdpSettings _idpSettings;
        private readonly IAuthenticationHelper _authenHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIddictController"/> class.
        /// </summary>
        /// <param name="userAccessModule"></param>
        public OpenIddictController(IUserAccessModule userAccessModule,
        IOptions<IdTokenSettings> idTokenSettings,
        IOptions<IdentitySettings> identitySettings,
        IOptions<IdpSettings> idpSettings, IAuthenticationHelper authenHelper
        )
        {
            _userAccessModule = userAccessModule;
            _idTokenSettings = idTokenSettings.Value;
            _identitySettings = identitySettings.Value;
            _clientBaseUri = (_identitySettings.Client?.ClientUri ?? "http://localhost:3000").TrimEnd('/');
            _idpSettings = idpSettings.Value;
            _authenHelper = authenHelper;
        }

        /// <summary>
        /// Displays the login page.
        /// Pass the returnUrl (/connect/authorize) to the login page.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet("/login")]
        public IActionResult Login(string returnUrl = "/connect/authorize") => View(new AuthenticateRequest { ReturnUrl = returnUrl });

        /// <summary>
        /// Handles user login authentication.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(AuthenticateRequest request)
        {
            try
            {
                // Authentication is a Command,
                // because Command has side effects (logging, session, audit), changes state (authentication context),
                // represents a business operation (login process), and can fail requiring error handling.
                // Authentication can log: login attempt, failed login, successful login, ip address, timestamps.
                // Authentication create: Claims principal, Authentication context, session data, security tokens.
                var result = await _userAccessModule.ExecuteCommandAsync(
                    new AuthenticateCommand(request.Login, request.Password));

                // Only allow local URLs and preserve the original query string.
                var returnUrl = string.IsNullOrEmpty(request.ReturnUrl) || !Url.IsLocalUrl(request.ReturnUrl)
                ? "/"
                : request.ReturnUrl;
                if (!result.IsAuthenticated)
                {
                    ModelState.AddModelError(string.Empty, result?.AuthenticationError ?? "Empty error message");
                    return View(new AuthenticateRequest { ReturnUrl = returnUrl });
                }

                // Create principal with fresh user data
                var principal = CreatePrincipal(result.User);

                await HttpContext.SignInAsync("IdentityProviderCookie", principal); // create a session at Identity Provider (IdP)
                return Redirect(returnUrl); // redirect to /connect/authorize (OIDC)
            }
            catch (InvalidCommandException ex)
            {
                // FluentValidation errors are handled by ValidationCommandHandlerDecorator
                // FluentValidation errors → ModelState used in Razor view to display errors
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                // Display form with errors
                return View(new AuthenticateRequest { ReturnUrl = request.ReturnUrl });
            }
        }

        /// <summary>
        /// Handles the authorization request and returns an authorization code that can be
        /// exchanged at the token endpoint. In dev, this issues a demo user principal.
        /// Authorization endpoint - generates authorization code for OAuth2 Authorization Code + PKCE flow.
        /// Client redirects here to start the authentication process. 
        /// </summary>
        [HttpGet("~/connect/authorize")]
        [AllowAnonymous]
        public async Task<IActionResult> Authorize()
        {
            // If user isn't authenticated via cookie 
            if (!await _authenHelper.IsUserAuthenticatedAsync(HttpContext))
            {
                Console.WriteLine("User not authenticated, redirecting to login");
                return _authenHelper.RedirectToLogin(this);
            }

            // if user is already authenticated via cookie
            return await ProcessAuthorizationRequest();
        }


        /// <summary>
        /// Token endpoint - exchanges authorization code for tokens or refreshes access tokens.
        /// Handles both Authorization Code Grant and Refresh Token Grant flows.
        /// </summary>
        [HttpPost("~/connect/token")]
        [AllowAnonymous]
        public async Task<IActionResult> Exchange()
        {
            // OpenIddict auto parse form data from request body
            // Request contains all data from body: grant_type, client_id, code, redirect_uri, code_verifier, etc.
            // - request.GrantType = "authorization_code"
            // - request.ClientId = "spa"
            // - request.Code = "authorization_code_here"
            // - request.RedirectUri = "http://localhost:3000/callback"
            // - request.CodeVerifier = "code_verifier_here"
            var request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenIddict server request cannot be retrieved.");

            // Handle Authorization Code Grant Type
            // Client sends: code, code_verifier, redirect_uri, client_id
            if (request.IsAuthorizationCodeGrantType())
            {
                return await HandleAuthorizationCodeGrantAsync(request);
            }

            // Handle Refresh Token Grant Type
            // Client sends: refresh_token, client_id
            if (request.IsRefreshTokenGrantType())
            {
                return await HandleRefreshTokenGrantAsync(request);
            }

            throw new NotImplementedException("The specified grant type is not implemented.");
        }

        /// <summary>
        /// User info endpoint to get user information
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/connect/userinfo")]
        [Authorize]
        public IActionResult UserInfo()
        {
            var roles = User.FindAll(CustomClaimTypes.Roles).Select(c => c.Value).ToArray();
            var claims = new Dictionary<string, object>
            {
                ["sub"] = User.FindFirst(OpenIddictConstants.Claims.Subject)?.Value ?? throw new InvalidOperationException("Subject claim is missing"),
                ["name"] = User.FindFirst(OpenIddictConstants.Claims.Name)?.Value ?? throw new InvalidOperationException("Name claim is missing"),
                ["email"] = User.FindFirst(OpenIddictConstants.Claims.Email)?.Value ?? throw new InvalidOperationException("Email claim is missing"),
                ["roles"] = roles
            };
            return Ok(claims);
        }


        #region Private Helper Methods

        [HttpGet("~/hash-password")]
        [AllowAnonymous]
        public IActionResult HashPassword(string password)
        {
            var hashed = PasswordManager.HashPassword(password);
            return Ok(new { hashed });
        }

        private ClaimsPrincipal CreatePrincipal(UserDto? user)
        {
            // Create claims identity for OpenIddict
            var identity = new ClaimsIdentity(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                ClaimTypes.Name,
                ClaimTypes.Role);

            // Add user claims from database
            identity.AddClaim(OpenIddictConstants.Claims.Subject, user?.Id.ToString() ?? string.Empty);
            identity.AddClaim(OpenIddictConstants.Claims.Name, user?.Name ?? string.Empty);
            identity.AddClaim(OpenIddictConstants.Claims.Email, user?.Email ?? string.Empty);
            if (user?.Roles?.Any() == true)
            {
                foreach (var role in user.Roles)
                {
                    identity.AddClaim(CustomClaimTypes.Roles, role.RoleCode);
                }
            }

            // Create principal from identity
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

        //private IActionResult RedirectToLogin() => Redirect($"{_clientBaseUri}/login?returnUrl={Uri.EscapeDataString(Request.GetEncodedUrl())}");

        private async Task<AuthenticatedUserDto?> ValidateAndGetUserAsync(string userId)
        {
            try
            {
                var user = await _userAccessModule.ExecuteQueryAsync(new GetUserQuery(Guid.Parse(userId)));
                return (user != null && user.IsActive) ? user : null;
            }
            catch
            {
                return null;
            }
        }

        private ClaimsPrincipal CreatePrincipal(AuthenticatedUserDto user, OpenIddictRequest request)
        {
            // Create claims identity for OpenIddict
            var identity = new ClaimsIdentity(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                ClaimTypes.Name,
                ClaimTypes.Role);

            // Add user claims from database
            identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
            identity.AddClaim(OpenIddictConstants.Claims.Name, user.Name);
            identity.AddClaim(OpenIddictConstants.Claims.Email, user.Email);
            if (user.Roles?.Any() == true)
            {
                foreach (var role in user.Roles)
                {
                    identity.AddClaim(CustomClaimTypes.Roles, role.RoleCode);
                }
            }

            // Create principal from identity
            var principal = new ClaimsPrincipal(identity);

            // Define allowed scopes for this application
            var allowed = new[] {
                OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Profile,
                //OpenIddictConstants.Scopes.Email,
                OpenIddictConstants.Scopes.OfflineAccess,
                _identitySettings.Scope.Name };

            Console.WriteLine($"Requested scopes: {string.Join(", ", allowed)}");
            Console.WriteLine($"Allowed scopes: {string.Join(", ", allowed)}");

            var finalScopes = request.GetScopes().Intersect(allowed);
            Console.WriteLine($"Final scopes: {string.Join(", ", finalScopes)}");

            // Set scopes and resources
            principal.SetScopes(request.GetScopes().Intersect(allowed));
            principal.SetResources(_identitySettings.ApiResource);

            // Attach the authorized presenter (requesting client) to this principal.
            // This binds the issued authorization code/refresh token to the given client_id,
            // ensuring only the same client can redeem them (critical for public clients without a client_secret).
            // OpenIddict validates presenters at /connect/token: the caller's client_id must match
            // the presenter stored in the code/refresh token, preventing token theft/replay by other clients.
            principal.SetPresenters(request?.ClientId ?? throw new InvalidOperationException("Client ID is required."));

            // Configure claim destinations for tokens
            ConfigureClaimDestinations(principal);

            return principal;
        }

        private void ConfigureClaimDestinations(ClaimsPrincipal principal)
        {
            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(claim.Type switch
                {
                    // Include user info (e.g, name, email, and roles... etc) in both access token and identity token								
                    OpenIddictConstants.Claims.Name => new[] { OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken },
                    OpenIddictConstants.Claims.Email => new[] { OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken },
                    CustomClaimTypes.Roles => new[] { OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken },
                    // Other claims only go to access token
                    _ => new[] { OpenIddictConstants.Destinations.AccessToken }
                });
            }
        }

        private async Task<IActionResult> HandleAuthorizationCodeGrantAsync(OpenIddictRequest request)
        {
            // Thêm logging để track authorization code
            Console.WriteLine($"Processing authorization code: {request.Code}");
            Console.WriteLine($"Request timestamp: {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss.fffZ}");

            // Validate the authorization code
            var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            if (info.Principal == null)
            {
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Get user ID from the authorization code
            var userId = info.Principal.GetClaim(OpenIddictConstants.Claims.Subject);
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            var user = await ValidateAndGetUserAsync(userId);
            if (user == null)
            {
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Create principal with fresh user data
            var principal = CreatePrincipal(user, request);

            // Return access token, identity token, and refresh token (first time)
            // OpenIddict stores the refresh token in database (Custom Stores)
            // Access token and identity token are generated and returned in JSON response
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> HandleRefreshTokenGrantAsync(OpenIddictRequest request)
        {
            // Validate the refresh token
            var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            if (info.Principal == null)
            {
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Get user ID from refresh token
            var userId = info.Principal.GetClaim(OpenIddictConstants.Claims.Subject);
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            var user = await ValidateAndGetUserAsync(userId);
            if (user == null)
            {
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Create principal with fresh user data
            var principal = CreatePrincipal(user, request);

            // Return new access token, identity token, and refresh token (renewal)
            // OpenIddict stores the new refresh token in database (Custom Stores)
            // Access token and identity token are generated and returned in JSON response
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> ProcessAuthorizationRequest()
        {
            // Get OIDC request parameters from the OpenIddict context
            var request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenIddict server request cannot be retrieved.");
            // Console.WriteLine($"=== OpenIddict Debug ===");
            // Console.WriteLine($"Request: {request}");
            // Console.WriteLine($"ClientId: {request.ClientId}");
            // Console.WriteLine($"redirect_uri (decoded) = {request.RedirectUri}");
            // Console.WriteLine($"Scopes: {string.Join(", ", request.GetScopes())}");
            // Console.WriteLine($"User.IsAuthenticated: {User.Identity?.IsAuthenticated}");
            // Console.WriteLine($"User.Claims.Count: {User.Claims.Count()}");
            // foreach (var claim in User.Claims)
            // {
            //     Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            // }

            var redirectUri = request.RedirectUri;
            var state = request.State;
            Console.WriteLine($"=== Authorization Request Debug ===");
            Console.WriteLine($"ClientId: {request.ClientId}");
            Console.WriteLine($"RedirectUri: {redirectUri}");
            Console.WriteLine($"State request: {state}");
            Console.WriteLine($"Scopes: {string.Join(", ", request.GetScopes())}");
            Console.WriteLine($"User.IsAuthenticated: {User.Identity?.IsAuthenticated}");

            // Check if OpenIddict can find the client
            try
            {
                var appStore = HttpContext.RequestServices.GetRequiredService<IOpenIddictApplicationStore<CustomApplication>>();
                var client = await appStore.FindByClientIdAsync(request.ClientId, CancellationToken.None);

                if (client == null)
                {
                    throw new InvalidOperationException($"Client '{request.ClientId}' not found in database.");
                }

                // Validate redirect URI
                var allowedRedirectUris = await appStore.GetRedirectUrisAsync(client, CancellationToken.None);
                Console.WriteLine($"Allowed redirect URIs: {string.Join(", ", allowedRedirectUris)}");

                if (!allowedRedirectUris.Contains(redirectUri))
                {
                    Console.WriteLine($"ERROR: Redirect URI '{redirectUri}' not in allowed list: {string.Join(", ", allowedRedirectUris)}");
                    throw new InvalidOperationException($"Invalid redirect URI: {redirectUri}");
                }

                Console.WriteLine($"Redirect URI validation passed: {redirectUri}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading client: {ex.Message}");
                throw;
            }

            // Check if scope exists
            try
            {
                var scopeStore = HttpContext.RequestServices.GetRequiredService<IOpenIddictScopeStore<CustomScope>>();
                var scope = await scopeStore.FindByNameAsync("modular-monolith-ddd-api", CancellationToken.None);

                if (scope == null)
                {
                    Console.WriteLine("Scope 'modular-monolith-ddd-api' NOT found in database!");
                }
                else
                {
                    Console.WriteLine("Scope found in database:");
                    Console.WriteLine($"  - Name: {scope.Name}");
                    Console.WriteLine($"  - Resources: {scope.ResourcesJson}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading scope: {ex.Message}");
            }

            // Extract user ID from authenticated user's claims            
            var userId = User.FindFirst(OpenIddictConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return _authenHelper.RedirectToLogin(this);
            }

            // Validate user exists and is active
            var user = await ValidateAndGetUserAsync(userId);
            if (user == null)
            {
                return _authenHelper.RedirectToLogin(this);
            }

            // Create principal with user claims
            var principal = CreatePrincipal(user, request);
            Console.WriteLine($"Principal Resources: {string.Join(", ", principal.GetResources())}");
            Console.WriteLine($"Principal Presenters: {string.Join(", ", principal.GetPresenters())}");

            //Console.WriteLine("=== Principal Debug ===");
            //Console.WriteLine($"Principal Claims Count: {principal.Claims.Count()}");
            //foreach (var claim in principal.Claims)
            //{
            //    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            //}

            // THÊM: Set state claim để OpenIddict preserve
            //if (!string.IsNullOrEmpty(state))
            //{
            //    principal.SetClaim("state", state);
            //    Console.WriteLine($"Added state claim: {state}");
            //}

            //Console.WriteLine("=== After Adding State ===");
            //foreach (var claim in principal.Claims)
            //{
            //    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            //}

            // OpenIddict does not automatically preserve the state parameter in the redirect URL.
            // Add state - the OIDC parameter by OpenIddict-specific properties
            // State: 
            // Prevent CSRF attacks (no attackers fake response), not to identify user
            // Verify that response from the same request
            var properties = new AuthenticationProperties();
            if (!string.IsNullOrEmpty(state))
            {
                // Using string literal
                properties.SetParameter("state", state);
                Console.WriteLine($"Setting state in properties: {state}");
            }

            // Generate authorization code and redirect to client
            return SignIn(principal, properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        
        #endregion
    }
}


