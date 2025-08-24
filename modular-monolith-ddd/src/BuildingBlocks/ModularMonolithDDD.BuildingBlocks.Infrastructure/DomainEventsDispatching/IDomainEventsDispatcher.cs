namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
	public interface IDomainEventsDispatcher
	{
		Task DispatchEventsAsync();
	}
}
