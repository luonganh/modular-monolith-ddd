namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.Serialization
{
	/// <summary>
    /// Custom JSON contract resolver that includes all properties (both public and private)
    /// in serialization and deserialization. This resolver extends the default contract resolver
    /// to ensure that all properties of a type are serialized, including private and protected members,
    /// which is particularly useful for domain entities and value objects in DDD.
    /// </summary>
    public class AllPropertiesContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Creates a list of JSON properties for the specified type, including all properties
        /// regardless of their access modifiers. This method overrides the default behavior
        /// to include private, protected, and public properties in serialization.
        /// </summary>
        /// <param name="type">The type to create properties for</param>
        /// <param name="memberSerialization">The member serialization mode</param>
        /// <returns>A list of JSON properties that includes all properties of the type</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = type.GetProperties(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance)
                .Select(p => this.CreateProperty(p, memberSerialization))
                .ToList();

            properties.ForEach(p =>
            {
                p.Writable = true;
                p.Readable = true;
            });

            return properties;
        }
    }
}