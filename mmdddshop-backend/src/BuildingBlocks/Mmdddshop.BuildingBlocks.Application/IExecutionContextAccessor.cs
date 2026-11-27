namespace Mmdddshop.BuildingBlocks.Application
{
    /// <summary>
    /// Interface for accessing the execution context.
    /// Audit logging.
    /// Request tracking.
    /// Security.
    /// </summary>
    public interface IExecutionContextAccessor
    {
        /// <summary>
        /// Gets the user id.
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// Gets the correlation id for tracking the request.
        /// </summary>
        Guid CorrelationId { get; }

        /// <summary>
        /// Gets a value indicating whether the availability of the execution context.
        /// </summary>
        bool IsAvailable { get; }
    }
}
