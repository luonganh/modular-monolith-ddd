namespace ModularMonolithDDD.BuildingBlocks.Domain
{
	/// <summary>
	/// Define a business rule.
	/// </summary>
	public interface IBusinessRule
    {
		/// <summary>
		/// Check if the business rule is broken.
		/// </summary>
		/// <returns>True if the business rule is broken, false otherwise.</returns>
		bool IsBroken();
		/// <summary>
		/// The message of the business rule.
		/// </summary>
		string Message { get; }
	}
}