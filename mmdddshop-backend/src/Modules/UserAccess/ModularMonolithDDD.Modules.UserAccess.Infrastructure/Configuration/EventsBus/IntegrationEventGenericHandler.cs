namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.EventsBus
{
    /// <summary>
    /// Generic integration event handler that processes incoming integration events from other modules or external systems.
    /// This handler implements the inbox pattern by storing integration events in the database for later processing,
    /// ensuring reliable event handling and eventual consistency across module boundaries.
    /// </summary>
    /// <typeparam name="T">The type of integration event to handle. Must inherit from IntegrationEvent.</typeparam>
    internal class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T>
        where T : IntegrationEvent
    {
        /// <summary>
        /// Handles the incoming integration event by storing it in the inbox table for later processing.
        /// This method implements the inbox pattern to ensure reliable event processing and prevent
        /// data loss in case of system failures or processing errors.
        /// </summary>
        /// <param name="event">The integration event to be processed and stored.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task Handle(T @event)
        {
			// Create a new lifetime scope for this event processing
			// This ensures proper dependency resolution and resource cleanup
			using (var scope = UserAccessCompositionRoot.BeginLifetimeScope())
			{
				// Get a database connection for storing the event
				using (var connection = scope.Resolve<ISqlConnectionFactory>().GetOpenConnection())
				{
					// Get the full type name for event identification
					string type = @event.GetType().FullName ?? throw new InvalidOperationException("Event type name is null");
					
					// Serialize the event data using JSON with custom contract resolver
					// AllPropertiesContractResolver ensures all properties are serialized
					var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
					{
						ContractResolver = new AllPropertiesContractResolver()
					});

					// SQL query to insert the integration event into the inbox table
					// This implements the inbox pattern for reliable event processing
					var sql = "INSERT INTO [users].[InboxMessages] (Id, OccurredOn, Type, Data) " +
							  "VALUES (@Id, @OccurredOn, @Type, @Data)";

					// Execute the insert operation asynchronously
					await connection.ExecuteScalarAsync(sql, new
					{
						@event.Id,
						@event.OccurredOn,
						type,
						data
					});
				}
			}
		}
    }
}