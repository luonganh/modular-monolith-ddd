namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.Serialization
{
	/// <summary>
	/// A custom contract resolver for JSON serialization that ensures all properties of a given type are serialized and deserialized.
	/// This resolver makes both public and non-public instance properties readable and writable during serialization.
	/// </summary>
	public class AllPropertiesContractResolver : DefaultContractResolver
    {
		/// <summary>
		/// Creates a list of JSON properties for the specified type, including both public and non-public instance properties.
		/// All properties are made writable and readable for JSON serialization and deserialization.
		/// </summary>
		/// <param name="type">The type for which to create the properties.</param>
		/// <param name="memberSerialization">Specifies how members of the type should be serialized.</param>
		/// <returns>A list of <see cref="JsonProperty"/> objects representing the properties of the type.</returns>
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
			// Retrieve all properties (public and non-public) of the specified type.
			var properties = type.GetProperties(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance)
                .Select(p => CreateProperty(p, memberSerialization))
                .ToList();

			// Make all properties writable and readable for serialization/deserialization.
			properties.ForEach(p =>
            {
                p.Writable = true;
                p.Readable = true;
            });

            return properties;
        }
    }
}