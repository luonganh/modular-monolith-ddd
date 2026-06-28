namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    /// <summary>
    /// Quartz job for processing queued internal commands.
    /// This job is scheduled to run periodically to process commands that were enqueued
    /// for asynchronous execution. The DisallowConcurrentExecution attribute ensures
    /// that only one instance of this job runs at a time.
    /// </summary>
    [DisallowConcurrentExecution]
    public class ProcessInternalCommandsJob : IJob
    {
        /// <summary>
        /// Executes the job to process internal commands.
        /// This method is called by the Quartz scheduler to process queued commands.
        /// </summary>
        /// <param name="context">The job execution context provided by Quartz.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.Execute(new ProcessInternalCommandsCommand());
        }
    }
}