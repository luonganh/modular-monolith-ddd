namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    /// <summary>
    /// Command for processing queued internal commands.
    /// This command is executed by a recurring job to process commands that were enqueued
    /// for asynchronous execution. It implements IRecurringCommand to indicate it's a scheduled job.
    /// </summary>
    internal class ProcessInternalCommandsCommand : CommandBase, IRecurringCommand
    {
    }
}