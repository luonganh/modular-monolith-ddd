namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.InternalCommands
{
	/// <summary>
	/// The <see cref="InternalCommandsMapper"/> class is responsible for mapping internal commands to their names and vice versa.
	/// It provides a way to retrieve the name of a command from its type and the type of a command from its name.
	/// </summary>
	public class InternalCommandsMapper : IInternalCommandsMapper
    {
        private readonly BiDictionary<string, Type> _internalCommandsMap;

		/// <summary>
		/// Initializes a new instance of the <see cref="InternalCommandsMapper"/> class.
		/// </summary>
		/// <param name="internalCommandsMap">The bidirectional dictionary that maps command names to command types and vice versa.</param>
		public InternalCommandsMapper(BiDictionary<string, Type> internalCommandsMap)
        {
            _internalCommandsMap = internalCommandsMap;
        }

		/// <summary>
		/// Retrieves the name associated with a given command type.
		/// </summary>
		/// <param name="type">The type of the command.</param>
		/// <returns>The name of the command, or <c>null</c> if no name is found for the given type.</returns>
		public string GetName(Type type)
        {
            return _internalCommandsMap.TryGetBySecond(type, out var name) ? name : null;
        }

		/// <summary>
		/// Retrieves the type associated with a given command name.
		/// </summary>
		/// <param name="name">The name of the command.</param>
		/// <returns>The type of the command, or <c>null</c> if no type is found for the given name.</returns>
		public Type GetType(string name)
        {
            return _internalCommandsMap.TryGetByFirst(name, out var type) ? type : null;
        }
    }
}