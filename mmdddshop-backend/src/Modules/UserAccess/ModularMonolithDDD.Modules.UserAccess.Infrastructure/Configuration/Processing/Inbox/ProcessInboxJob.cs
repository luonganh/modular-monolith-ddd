namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox
{
    /// <summary>
    /// Quartz job for processing inbox messages.
    /// This job is scheduled to run periodically to process messages that were received
    /// from external systems and stored in the inbox. The DisallowConcurrentExecution attribute ensures
    /// that only one instance of this job runs at a time.
    /// </summary>
    [DisallowConcurrentExecution]
    public class ProcessInboxJob : IJob
    {
        /// <summary>
        /// Executes the job to process inbox messages.
        /// This method is called by the Quartz scheduler to process queued messages.
        /// </summary>
        /// <param name="context">The job execution context provided by Quartz.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.Execute(new ProcessInboxCommand());
        }
    }
}