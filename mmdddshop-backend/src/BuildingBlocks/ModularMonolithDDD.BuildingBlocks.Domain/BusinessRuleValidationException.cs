namespace ModularMonolithDDD.BuildingBlocks.Domain
{
	/// <summary>
	/// Exception for business rule validation.
	/// </summary>
	public class BusinessRuleValidationException : Exception
	{
		/// <summary>
		/// The broken business rule.
		/// </summary>
		public IBusinessRule BrokenRule { get; }

		/// <summary>
		/// The details of the business rule validation.
		/// </summary>
		public string Details { get; }
	
		/// <summary>
		/// Initializes a new instance of the <see cref="BusinessRuleValidationException"/> class.
		/// </summary>
		/// <param name="brokenRule">The broken business rule.</param>
		public BusinessRuleValidationException(IBusinessRule brokenRule)
			: base(brokenRule.Message)
		{
			BrokenRule = brokenRule;
			this.Details = brokenRule.Message;
		}
	
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
		}
	}
}
