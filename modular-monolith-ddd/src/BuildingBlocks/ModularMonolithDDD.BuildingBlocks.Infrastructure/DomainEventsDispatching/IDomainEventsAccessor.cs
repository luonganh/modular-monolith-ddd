namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
	public interface IDomainEventsAccessor
	{
		IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

		void ClearAllDomainEvents();
	}
}
