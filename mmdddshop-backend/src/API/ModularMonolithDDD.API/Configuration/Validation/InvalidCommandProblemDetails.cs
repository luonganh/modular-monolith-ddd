
namespace ModularMonolithDDD.API.Configuration.Validation
{
    /// <summary>
    /// Problem details for invalid commands.
    /// </summary>
    public class InvalidCommandProblemDetails : ProblemDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandProblemDetails"/> class.
        /// </summary>
        /// <param name="exception"></param>
        public InvalidCommandProblemDetails(InvalidCommandException exception)
        {
            Title = "Command validation error";
            Status = StatusCodes.Status400BadRequest;
            Type = "https://somedomain/validation-error";
            Errors = exception.Errors;
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public List<string> Errors { get; }
    }
}