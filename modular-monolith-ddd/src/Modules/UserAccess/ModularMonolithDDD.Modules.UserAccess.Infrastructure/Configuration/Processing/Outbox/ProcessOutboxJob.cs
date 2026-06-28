namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
    /// <summary>
    /// Quartz job for processing outbox messages on a scheduled basis.
    /// This job is designed to run periodically to ensure reliable delivery of domain events
    /// by processing unprocessed outbox messages from the database.
    /// The DisallowConcurrentExecution attribute ensures only one instance runs at a time.
    /// </summary>
    [DisallowConcurrentExecution]
    public class ProcessOutboxJob : IJob
    {
        /// <summary>
        /// Executes the outbox processing job.
        /// This method is called by the Quartz scheduler to process outbox messages.
        /// It delegates the actual processing to the CommandsExecutor with a ProcessOutboxCommand.
        /// </summary>
        /// <param name="context">The Quartz job execution context containing job and trigger information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            // Execute the ProcessOutboxCommand through the CommandsExecutor
            // This ensures proper dependency injection and command handling pipeline
            await CommandsExecutor.Execute(new ProcessOutboxCommand());
        }
    }
}