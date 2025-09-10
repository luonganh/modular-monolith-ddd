namespace ModularMonolithDDD.BuildingBlocks.Domain
{
	/// <summary>
	/// Ignore member attribute for value object comparison.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class IgnoreMemberAttribute : Attribute
	{
	}
}
