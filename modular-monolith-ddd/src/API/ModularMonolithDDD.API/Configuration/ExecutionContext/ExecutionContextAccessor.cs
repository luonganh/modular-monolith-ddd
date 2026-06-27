namespace ModularMonolithDDD.API.Configuration.ExecutionContext
{
    /// <summary>
    /// Accessor for the execution context.
    /// </summary>
    public class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionContextAccessor"/> class.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        public Guid UserId
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    var subClaim = httpContext.User.Claims.SingleOrDefault(x => x.Type == "sub");
                    if (subClaim?.Value != null)
                    {
                        return Guid.Parse(subClaim.Value);
                    }
                }               

                throw new ApplicationException("User context is not available");
            }
        }

        /// <summary>
        /// Gets the correlation ID.
        /// </summary>
        public Guid CorrelationId
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (IsAvailable && httpContext?.Request?.Headers != null &&
                    httpContext.Request.Headers.Keys.Any(x => x == CorrelationMiddleware.CorrelationHeaderKey))
                {
                    var headerValue = httpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey].FirstOrDefault();
                    if (!string.IsNullOrEmpty(headerValue))
                    {
                        try
                        {
                            return Guid.Parse(headerValue);
                        }
                        catch (FormatException)
                        {
                            throw new ApplicationException($"Invalid correlation ID format: {headerValue}");
                        }
                    }                    
                }

                throw new ApplicationException("Http context and correlation id is not available");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the execution context is available.
        /// </summary>
        public bool IsAvailable => _httpContextAccessor.HttpContext != null;
    }
}