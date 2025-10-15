namespace ModularMonolithDDD.API.Controllers
{
	/// <summary>
	/// Controller for the protected endpoints.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class ProtectedController : ControllerBase
	{
		/// <summary>
		/// Pings the protected endpoint.
		/// </summary>
		/// <returns></returns>
		[HttpGet("ping")]
		[Authorize(Policy = "ApiScope")]
		public IActionResult Ping()
		{
			return Ok(new { message = "pong", user = User.Identity?.Name });
		}
	}
}


