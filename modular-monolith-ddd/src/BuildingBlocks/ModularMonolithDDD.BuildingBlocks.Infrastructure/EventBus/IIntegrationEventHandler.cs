namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.EventBus
{
    /// <summary>
    /// Defines the contract for handling integration events of a specific type.
    /// This interface is implemented by classes that need to process integration events
    /// published by other modules in the modular monolith architecture.
    /// </summary>
    /// <typeparam name="TIntegrationEvent">The type of integration event this handler processes</typeparam>
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Handles the specified integration event.
        /// This method is called by the event bus when an integration event
        /// of the handler's registered type is published.
        /// </summary>
        /// <param name="event">The integration event to handle</param>
        /// <returns>A task representing the asynchronous handling operation</returns>
        Task Handle(TIntegrationEvent @event);
    }

    /// <summary>
    /// Base interface for all integration event handlers.
    /// This marker interface allows for type-agnostic handling of integration
    /// event handlers in the event bus infrastructure.
    /// </summary>
    public interface IIntegrationEventHandler
    {
    }
}