namespace ModularMonolithDDD.BuildingBlocks.Domain
{
	/// <summary>
	/// Entity base class with domain events support.
	/// </summary>
    public abstract class Entity
    {	
		/// <summary>
		/// Domain events occurred.
		/// </summary>
		private List<IDomainEvent>? _domainEvents;

		/// <summary>
		/// Domain events occurred.
		/// </summary>
		public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

		/// <summary>
		/// Clear domain events.
		/// </summary>
		public void ClearDomainEvents()
		{
			_domainEvents?.Clear();
		}

		/// <summary>
		/// Add domain event.
		/// </summary>
		/// <param name="domainEvent">Domain event.</param>
		protected void AddDomainEvent(IDomainEvent domainEvent)
		{
			_domainEvents ??= [];			
			this._domainEvents.Add(domainEvent);
		}

		/// <summary>
		/// Check if the business rule is broken.
		/// </summary>
		/// <param name="rule">The business rule to check.</param>
		protected void CheckRule(IBusinessRule rule)
		{
			if (rule.IsBroken())
			{
				throw new BusinessRuleValidationException(rule);
			}
		}
	}
}
