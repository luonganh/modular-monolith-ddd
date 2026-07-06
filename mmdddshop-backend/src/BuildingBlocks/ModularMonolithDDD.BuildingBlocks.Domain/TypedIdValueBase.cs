namespace ModularMonolithDDD.BuildingBlocks.Domain
{
	/// <summary>
	/// Base class for strongly-typed id value objects.
	/// </summary>
	public abstract class TypedIdValueBase : IEquatable<TypedIdValueBase>
	{
		/// <summary>
		/// The value of the typed id value object.
		/// </summary>
		public Guid Value { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TypedIdValueBase"/> class.
		/// </summary>
		/// <param name="value">The value of the typed id value object.</param>
		protected TypedIdValueBase(Guid value)
		{
			if (value == Guid.Empty)
			{
				throw new InvalidOperationException("Id value cannot be empty!");
			}

			Value = value;
		}

		/// <summary>
		/// Check if the typed id value object is equal to another typed id value object.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>True if the typed id value object is equal to another typed id value object, false otherwise.</returns>
		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is TypedIdValueBase other && Equals(other);
		}

		/// <summary>
		/// Get the hash code of the typed id value object.
		/// </summary>
		/// <returns>The hash code of the typed id value object.</returns>
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		/// <summary>
		/// Check if the typed id value object is equal to another typed id value object.
		/// </summary>
		/// <param name="other">The other typed id value object.</param>
		/// <returns>True if the typed id value object is equal to another typed id value object, false otherwise.</returns>
		public bool Equals(TypedIdValueBase? other)
		{
			return other is not null && this.Value == other.Value;
		}

		/// <summary>
		/// Check if the typed id value object is equal to another typed id value object.
		/// </summary>
		/// <param name="obj1">The first typed id value object.</param>
		/// <param name="obj2">The second typed id value object.</param>
		/// <returns>True if the typed id value object is equal to another typed id value object, false otherwise.</returns>
		public static bool operator ==(TypedIdValueBase? obj1, TypedIdValueBase obj2)
		{
			if (ReferenceEquals(obj1, null)) return ReferenceEquals(obj2, null);
			return obj1.Equals(obj2);			
		}

		/// <summary>
		/// Check if the typed id value object is not equal to another typed id value object.
		/// </summary>
		/// <param name="x">The first typed id value object.</param>
		/// <param name="y">The second typed id value object.</param>
		/// <returns>True if the typed id value object is not equal to another typed id value object, false otherwise.</returns>
		public static bool operator !=(TypedIdValueBase x, TypedIdValueBase y)
		{
			return !(x == y);
		}
	}
}
