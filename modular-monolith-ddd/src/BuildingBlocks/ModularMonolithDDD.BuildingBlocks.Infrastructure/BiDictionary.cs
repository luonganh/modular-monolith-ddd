namespace ModularMonolithDDD.BuildingBlocks.Infrastructure
{
	/// <summary>
	/// A bidirectional dictionary that allows for two-way lookups between two types, TFirst and TSecond.
	/// This class maintains two internal dictionaries, one mapping TFirst to TSecond, and another mapping TSecond to TFirst.
	/// </summary>
	/// <typeparam name="TFirst">The type of the first element in the dictionary.</typeparam>
	/// <typeparam name="TSecond">The type of the second element in the dictionary.</typeparam>
	public class BiDictionary<TFirst, TSecond>
	{
		// Internal dictionary that maps TFirst to TSecond
		private readonly IDictionary<TFirst, TSecond> _firstToSecond = new Dictionary<TFirst, TSecond>();

		// Internal dictionary that maps TSecond to TFirst
		private readonly IDictionary<TSecond, TFirst> _secondToFirst = new Dictionary<TSecond, TFirst>();

		/// <summary>
		/// Adds a pair of values to the bidirectional dictionary.
		/// Throws an exception if either the first or second element already exists in the dictionary.
		/// </summary>
		/// <param name="first">The first element to be added to the dictionary.</param>
		/// <param name="second">The second element to be added to the dictionary.</param>
		public void Add(TFirst first, TSecond second)
		{
			// Ensure no duplicates are added
			if (_firstToSecond.ContainsKey(first) ||
				_secondToFirst.ContainsKey(second))
			{
				throw new ArgumentException("Duplicate first or second");
			}

			// Add both directions to the dictionaries
			_firstToSecond.Add(first, second);
			_secondToFirst.Add(second, first);
		}

		/// <summary>
		/// Tries to get the second value based on a given first value.
		/// </summary>
		/// <param name="first">The first value to search for.</param>
		/// <param name="second">The second value corresponding to the given first value.</param>
		/// <returns>True if the first value exists in the dictionary, otherwise false.</returns>
		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return _firstToSecond.TryGetValue(first, out second);
		}

		/// <summary>
		/// Tries to get the first value based on a given second value.
		/// </summary>
		/// <param name="second">The second value to search for.</param>
		/// <param name="first">The first value corresponding to the given second value.</param>
		/// <returns>True if the second value exists in the dictionary, otherwise false.</returns>
		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return _secondToFirst.TryGetValue(second, out first);
		}
	}
}
