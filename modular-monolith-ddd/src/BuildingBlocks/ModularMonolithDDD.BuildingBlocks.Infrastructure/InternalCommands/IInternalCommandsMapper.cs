namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.InternalCommands
{
    /// <summary>
    /// Defines the contract for mapping between internal command types and their string representations.
    /// This interface is used in the internal commands pattern to enable serialization and deserialization
    /// of commands stored in the database for background processing.
    /// </summary>
    public interface IInternalCommandsMapper
    {
        /// <summary>
        /// Gets the string name representation of a command type.
        /// This method is used when serializing commands to store them in the database.
        /// </summary>
        /// <param name="type">The command type to get the name for</param>
        /// <returns>The string name of the command type, or null if not found</returns>
        string GetName(Type type);

        /// <summary>
        /// Gets the command type from its string name representation.
        /// This method is used when deserializing commands from the database for processing.
        /// </summary>
        /// <param name="name">The string name of the command type</param>
        /// <returns>The command type, or null if not found</returns>
        Type GetType(string name);
    }
}