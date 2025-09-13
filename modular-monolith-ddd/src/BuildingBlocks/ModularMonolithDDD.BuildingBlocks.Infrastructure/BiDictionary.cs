namespace ModularMonolithDDD.BuildingBlocks.Infrastructure
{
	/// <summary>
    /// A bidirectional dictionary that maintains mappings between two types in both directions.
    /// This generic class allows efficient lookup in both directions (TFirst → TSecond and TSecond → TFirst)
    /// while ensuring that each key-value pair is unique in both directions. This is particularly useful
    /// for scenarios like type-to-name mappings in serialization or command mapping systems.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the bidirectional mapping</typeparam>
    /// <typeparam name="TSecond">The second type in the bidirectional mapping</typeparam>
    public class BiDictionary<TFirst, TSecond>
    {
        /// <summary>
        /// Internal dictionary that maps from the first type to the second type.
        /// This dictionary is used for forward lookups (TFirst → TSecond).
        /// </summary>
        private readonly IDictionary<TFirst, TSecond> _firstToSecond = new Dictionary<TFirst, TSecond>();

        /// <summary>
        /// Internal dictionary that maps from the second type to the first type.
        /// This dictionary is used for reverse lookups (TSecond → TFirst).
        /// </summary>
        private readonly IDictionary<TSecond, TFirst> _secondToFirst = new Dictionary<TSecond, TFirst>();

        /// <summary>
        /// Adds a new bidirectional mapping between the specified first and second values.
        /// This method ensures that both the forward and reverse mappings are created,
        /// and throws an exception if either key already exists in either direction.
        /// </summary>
        /// <param name="first">The first value in the mapping</param>
        /// <param name="second">The second value in the mapping</param>
        /// <exception cref="ArgumentException">Thrown when either the first or second value already exists in the dictionary</exception>
        public void Add(TFirst first, TSecond second)
        {
            if (_firstToSecond.ContainsKey(first) ||
                _secondToFirst.ContainsKey(second))
            {
                throw new ArgumentException("Duplicate first or second");
            }

            _firstToSecond.Add(first, second);
            _secondToFirst.Add(second, first);
        }

        /// <summary>
        /// Attempts to get the second value associated with the specified first value.
        /// This method performs a forward lookup (TFirst → TSecond) and returns true
        /// if the mapping exists, false otherwise.
        /// </summary>
        /// <param name="first">The first value to look up</param>
        /// <param name="second">When this method returns, contains the second value if found; otherwise, the default value</param>
        /// <returns>True if the mapping exists; otherwise, false</returns>
        public bool TryGetByFirst(TFirst first, out TSecond second)
        {
            return _firstToSecond.TryGetValue(first, out second);
        }

        /// <summary>
        /// Attempts to get the first value associated with the specified second value.
        /// This method performs a reverse lookup (TSecond → TFirst) and returns true
        /// if the mapping exists, false otherwise.
        /// </summary>
        /// <param name="second">The second value to look up</param>
        /// <param name="first">When this method returns, contains the first value if found; otherwise, the default value</param>
        /// <returns>True if the mapping exists; otherwise, false</returns>
        public bool TryGetBySecond(TSecond second, out TFirst first)
        {
            return _secondToFirst.TryGetValue(second, out first);
        }
    }
}
