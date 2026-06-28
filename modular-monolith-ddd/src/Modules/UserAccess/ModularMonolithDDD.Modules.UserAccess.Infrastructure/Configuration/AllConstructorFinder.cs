namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration
{
    /// <summary>
    /// Custom constructor finder for Autofac that finds all constructors for a given type.
    /// This implementation caches constructor information to improve performance.
    /// Used to enable dependency injection for types with multiple constructors.
    /// </summary>
    internal class AllConstructorFinder : IConstructorFinder
    {
        /// <summary>
        /// Thread-safe cache for storing constructor information by type.
        /// Prevents repeated reflection calls for the same type.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> Cache =
            new ConcurrentDictionary<Type, ConstructorInfo[]>();

        /// <summary>
        /// Finds all constructors for the specified target type.
        /// Uses caching to improve performance for repeated lookups.
        /// </summary>
        /// <param name="targetType">The type to find constructors for.</param>
        /// <returns>Array of constructor information for the target type.</returns>
        /// <exception cref="NoConstructorsFoundException">Thrown when no constructors are found for the target type.</exception>
        public ConstructorInfo[] FindConstructors(Type targetType)
        {
            var result = Cache.GetOrAdd(
                targetType,
                t => t.GetTypeInfo().DeclaredConstructors.ToArray());

            return result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType, this);
        }
    }
}