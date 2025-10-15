namespace ModularMonolithDDD.API.Middlewares
{
    /// <summary>
	/// Central error/exception handler middleware for the application.
    /// This middleware catches unhandled exceptions, logs them, and returns standardized error responses.
    /// It should be placed early in the pipeline to catch all exceptions from downstream middleware and controllers.
	/// </summary>
	public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _request;
        
        // ExceptionHandlerMiddleware gets ILogger<ExceptionHandlerMiddleware> from the DI container.        
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">The logger used to record errors.</param>
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            this._request = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to process the HTTP request.
        /// </summary>
        /// <param name="context">The HTTP context containing request and response information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task Invoke(HttpContext context) => this.InvokeAsync(context);

         /// <summary>
        /// Asynchronously processes the HTTP request and handles any exceptions that occur.
        /// </summary>
        /// <param name="context">The HTTP context containing request and response information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task InvokeAsync(HttpContext context)
        {
            try
            {
                 // Process the request through the pipeline
                await this._request(context);
            }
            catch (InvalidCommandException ex)
            {
                // Handle validation errors with structured ProblemDetails response
                context.Features.Set(new ProblemDetailsContext
                {
                    HttpContext = context,                    
                    ProblemDetails = new InvalidCommandProblemDetails(ex)
                });
                await Results.Problem().ExecuteAsync(context);
            }
            catch (BusinessRuleValidationException ex)
            {
                // Handle business rule violations with structured ProblemDetails response
                var problemDetails = new BusinessRuleProblemDetails(ex);
                await Results.Problem(problemDetails).ExecuteAsync(context);
            }
            catch (Exception exception)
            {
                // Log the exception details for debugging and monitoring
                var exMess = $"Exception - {exception.Message}";
                var innerExMess = exception.InnerException != null ? $"InnerException - {exception.InnerException.Message}" : string.Empty;
                
                // Uses ILogger<ExceptionHandlerMiddleware> from the DI container.
                _logger.LogError($"Request error at {context.Request.Path} : {exMess}; {innerExMess}");

                // Uses static Serilog ILogger from ConfigureLogger().
                Log.Error(exception, "Request error: {0} ; {1}", exMess, innerExMess);

                // Return a generic error response to the client
                await Results.Problem("An error occurred").ExecuteAsync(context);
            }
        }
    }
}
