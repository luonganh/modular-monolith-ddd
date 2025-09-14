namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration
{
    /// <summary>
    /// Provides access to assembly references for the UserAccess module.
    /// This class centralizes assembly references to avoid hardcoded assembly names throughout the codebase.
    /// </summary>
    internal static class Assemblies
    {
        /// <summary>
        /// Gets the UserAccess Application assembly.
        /// This assembly contains all the application layer components including commands, queries, and handlers.
        /// </summary>
        public static readonly Assembly Application = typeof(IUserAccessModule).Assembly;
    }
}