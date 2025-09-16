using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace ModularMonolithDDD.API.Controllers
{
	/// <summary>
	/// Minimal authorization endpoint to issue authorization codes for public clients using PKCE.
	/// This sample signs in a demo user to unblock frontend integration during development.
	/// Replace with your real login flow and user validation.
	/// </summary>
	public class AuthorizationController : Controller
	{
		/// <summary>
		/// Handles the authorization request and returns an authorization code that can be
		/// exchanged at the token endpoint. In dev, this issues a demo user principal.
		/// </summary>
		[HttpGet("~/connect/authorize")]
		[AllowAnonymous]
		public IActionResult Authorize()
		{
			var request = HttpContext.GetOpenIddictServerRequest()
				?? throw new InvalidOperationException("The OpenIddict server request cannot be retrieved.");

			// In development, sign in a demo user without prompting for credentials.
			var identity = new ClaimsIdentity(
				OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
				ClaimTypes.Name,
				ClaimTypes.Role);

			identity.AddClaim(OpenIddictConstants.Claims.Subject, "demo-user-id");
			identity.AddClaim(OpenIddictConstants.Claims.Name, "Modular Monolith DDD");
			identity.AddClaim(OpenIddictConstants.Claims.Email, "luonganh@gmail.com");

			var principal = new ClaimsPrincipal(identity);

			// Flow requested scopes through; restrict to allowed set.
			var allowed = new[] { 
				OpenIddictConstants.Scopes.OpenId, 
				OpenIddictConstants.Scopes.Profile, 
				OpenIddictConstants.Scopes.Email, 
				OpenIddictConstants.Scopes.OfflineAccess,
				"modular-monolith-ddd-api" };
			var requested = request.GetScopes();
			principal.SetScopes(requested.Intersect(allowed));
			principal.SetResources("modular-monolith-ddd-api");

			// Indicate which claims should be included in tokens.
			foreach (var claim in principal.Claims)
			{
				claim.SetDestinations(claim.Type switch
				{
					OpenIddictConstants.Claims.Name => new[] { OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken },
					OpenIddictConstants.Claims.Email => new[] { OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken },
					_ => new[] { OpenIddictConstants.Destinations.AccessToken }
				});
			}

			return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
		}
	}
}


