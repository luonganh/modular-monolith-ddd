namespace ModularMonolithDDD.BuildingBlocks.Application
{	
	/// <summary>
	/// Exception for invalid commands.
	/// Multiple validation errors.
	/// Structured error handling.
	/// User-friendly messages
	/// </summary>
	public class InvalidCommandException : Exception
	{
		/// <summary>
		/// The errors.
		/// </summary>
		public List<string> Errors { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCommandException"/> class.
		/// </summary>
		/// <param name="errors">The errors.</param>
		public InvalidCommandException(List<string> errors)
		{
			this.Errors = errors;
		}
	}
}
