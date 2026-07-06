namespace ModularMonolithDDD.API.Controllers
{
    /// <summary>
    /// Home controller for ModularMonolithDDD API.
    /// </summary>    
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {

        /// <summary>
        /// Gets the welcome message for the API.
        /// </summary>
        /// <returns>A welcome message.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Modular Monolith DDD API");
        }
    }
}