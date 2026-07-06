namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    /// <summary>
    /// Marker interface for commands that are executed on a recurring basis.
    /// Commands implementing this interface are typically scheduled jobs that run periodically
    /// and may have different logging behavior compared to regular user-initiated commands.
    /// </summary>
    public interface IRecurringCommand
    {
    }
}