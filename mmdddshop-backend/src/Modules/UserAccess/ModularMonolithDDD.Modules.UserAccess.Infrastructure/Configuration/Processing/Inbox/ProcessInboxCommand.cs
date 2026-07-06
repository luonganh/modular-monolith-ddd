namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox
{
    /// <summary>
    /// Command for processing inbox messages.
    /// This command is executed by a recurring job to process messages that were received
    /// from external systems and stored in the inbox. It implements IRecurringCommand to indicate it's a scheduled job.
    /// </summary>
    public class ProcessInboxCommand : CommandBase, IRecurringCommand
    {
    }
}