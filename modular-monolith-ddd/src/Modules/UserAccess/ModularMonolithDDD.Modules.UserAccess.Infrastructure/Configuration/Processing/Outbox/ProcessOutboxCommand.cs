namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
    /// <summary>
    /// Command for processing outbox messages to ensure reliable domain event delivery.
    /// This command is designed to be executed as a recurring job to process
    /// unprocessed outbox messages from the database and publish them as domain events.
    /// Implements IRecurringCommand to indicate it should be executed periodically.
    /// </summary>
    public class ProcessOutboxCommand : CommandBase, IRecurringCommand
    {
        // This command has no specific properties as it processes all unprocessed outbox messages
        // The actual processing logic is handled by the ProcessOutboxCommandHandler
    }
}