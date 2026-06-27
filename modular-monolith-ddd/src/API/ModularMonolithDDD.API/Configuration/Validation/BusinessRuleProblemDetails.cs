namespace ModularMonolithDDD.API.Configuration.Validation
{
	/// <summary>
	/// Problem details for business rule validation errors.
	/// </summary>
	public class BusinessRuleProblemDetails : ProblemDetails
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BusinessRuleProblemDetails"/> class.
		/// </summary>
		/// <param name="exception"></param>
		public BusinessRuleProblemDetails(BusinessRuleValidationException exception)
		{
			Title = "Business rule validation error";
			Status = StatusCodes.Status400BadRequest;
			Type = "https://somedomain/business-rule-error";
			Detail = exception.Details;
		}
	}
}