namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.InternalCommands
{
	/// <summary>
    /// Implementation of the internal commands mapper that provides bidirectional mapping
    /// between internal command types and their string representations. This class uses
    /// a BiDictionary to enable efficient lookup in both directions for serialization
    /// and deserialization of internal commands.
    /// </summary>
    public class InternalCommandsMapper : IInternalCommandsMapper
    {
        private readonly BiDictionary<string, Type> _internalCommandsMap;

        /// <summary>
        /// Initializes a new instance of the InternalCommandsMapper class.
        /// This constructor sets up the bidirectional mapping between command types
        /// and their string representations for serialization and deserialization.
        /// </summary>
        /// <param name="internalCommandsMap">The bidirectional dictionary containing type-to-name mappings</param>
        public InternalCommandsMapper(BiDictionary<string, Type> internalCommandsMap)
        {
            _internalCommandsMap = internalCommandsMap;
        }

        /// <summary>
        /// Gets the string name representation of a command type.
        /// This method performs a reverse lookup in the bidirectional dictionary
        /// to find the string name associated with the given command type.
        /// </summary>
        /// <param name="type">The command type to get the name for</param>
        /// <returns>The string name of the command type, or null if not found</returns>
        public string? GetName(Type type)
        {
            return _internalCommandsMap.TryGetBySecond(type, out var name) ? name : null;
        }

        /// <summary>
        /// Gets the command type from its string name representation.
        /// This method performs a forward lookup in the bidirectional dictionary
        /// to find the command type associated with the given string name.
        /// </summary>
        /// <param name="name">The string name of the command type</param>
        /// <returns>The command type, or null if not found</returns>
        public Type? GetType(string name)
        {
            return _internalCommandsMap.TryGetByFirst(name, out var type) ? type : null;
        }
    }
}