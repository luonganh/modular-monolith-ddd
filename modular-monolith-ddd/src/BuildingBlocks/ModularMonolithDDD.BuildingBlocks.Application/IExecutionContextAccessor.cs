namespace ModularMonolithDDD.BuildingBlocks.Application
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
		/// The user id.
		/// </summary>
		Guid UserId { get; }
		
		/// <summary>		
		/// The correlation id for tracking the request.
		/// </summary>
		Guid CorrelationId { get; }

		/// <summary>
		/// The availability of the execution context.
		/// </summary>

		bool IsAvailable { get; }
	}
}
