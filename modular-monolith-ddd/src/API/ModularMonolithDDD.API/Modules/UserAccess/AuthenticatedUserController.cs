using ModularMonolithDDD.API.Configuration.Authorization;
using ModularMonolithDDD.Modules.UserAccess.Application.Authorization.GetAuthenticatedUserPermissions;
using ModularMonolithDDD.Modules.UserAccess.Application.Contracts;
using ModularMonolithDDD.Modules.UserAccess.Application.Users.GetAuthenticatedUser;
using Microsoft.AspNetCore.Mvc;

namespace ModularMonolithDDD.API.Modules.UserAccess
{
    /// <summary>
    /// Controller for the authenticated user.
    /// </summary>
    [Route("api/v1/userAccess/authenticatedUser")]
    [ApiController]
    public class AuthenticatedUserController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedUserController"/> class.
        /// </summary>
        /// <param name="userAccessModule"></param>
        public AuthenticatedUserController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        /// <summary>
        /// Gets the authenticated user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("")]
        [ProducesResponseType(typeof(AuthenticatedUserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            var user = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatedUserQuery());

            return Ok(user);
        }

        /// <summary>        
        /// Gets the authenticated user permissions.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("permissions")]
        [ProducesResponseType(typeof(List<UserPermissionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUserPermissions()
        {
            var permissions = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatedUserPermissionsQuery());

            return Ok(permissions);
        }
    }
}