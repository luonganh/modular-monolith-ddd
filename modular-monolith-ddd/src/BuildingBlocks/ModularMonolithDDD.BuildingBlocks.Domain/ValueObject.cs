namespace ModularMonolithDDD.BuildingBlocks.Domain
{
	/// <summary>
	/// Base class for value objects. (value-based equality, immutable)
	/// </summary>
	public abstract class ValueObject : IEquatable<ValueObject>
	{
		private List<PropertyInfo> _properties;

		private List<FieldInfo> _fields;

		/// <summary>
		/// Check if two value objects are equal.
		/// </summary>
		/// <param name="obj1">The first value object.</param>
		/// <param name="obj2">The second value object.</param>
		/// <returns>True if the two value objects are equal, false otherwise.</returns>
		public static bool operator ==(ValueObject obj1, ValueObject obj2)
		{
			if (object.Equals(obj1, null))
			{
				if (object.Equals(obj2, null))
				{
					return true;
				}

				return false;
			}

			return obj1.Equals(obj2);
		}

		/// <summary>
		/// Check if two value objects are not equal.
		/// </summary>
		/// <param name="obj1">The first value object.</param>
		/// <param name="obj2">The second value object.</param>
		/// <returns>True if the two value objects are not equal, false otherwise.</returns>
		public static bool operator !=(ValueObject obj1, ValueObject obj2)
		{
			return !(obj1 == obj2);
		}

		/// <summary>
		/// Check if two value objects are equal.
		/// </summary>
		/// <param name="obj">The value object to compare.</param>
		/// <returns>True if the two value objects are equal, false otherwise.</returns>
		public bool Equals(ValueObject obj)
		{
			return Equals(obj as object);
		}

		/// <summary>
		/// Check if two value objects are equal.
		/// </summary>
		/// <param name="obj">The value object to compare.</param>
		/// <returns>True if the two value objects are equal, false otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			return GetProperties().All(p => PropertiesAreEqual(obj, p))
				&& GetFields().All(f => FieldsAreEqual(obj, f));
		}

		/// <summary>
		/// Get the hash code of the value object.
		/// </summary>
		/// <returns>The hash code of the value object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				foreach (var prop in GetProperties())
				{
					var value = prop.GetValue(this, null);
					hash = HashValue(hash, value);
				}

				foreach (var field in GetFields())
				{
					var value = field.GetValue(this);
					hash = HashValue(hash, value);
				}

				return hash;
			}
		}

		/// <summary>
		/// Check if the business rule is broken.
		/// </summary>
		/// <param name="rule">The business rule to check.</param>
		protected static void CheckRule(IBusinessRule rule)
		{
			if (rule.IsBroken())
			{
				throw new BusinessRuleValidationException(rule);
			}
		}

		/// <summary>
		/// Check if two properties are equal.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <param name="p">The property to compare.</param>
		/// <returns>True if the two properties are equal, false otherwise.</returns>
		private bool PropertiesAreEqual(object obj, PropertyInfo p)
		{
			return object.Equals(p.GetValue(this, null), p.GetValue(obj, null));
		}

		/// <summary>
		/// Check if two fields are equal.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <param name="f">The field to compare.</param>
		/// <returns>True if the two fields are equal, false otherwise.</returns>
		private bool FieldsAreEqual(object obj, FieldInfo f)
		{
			return object.Equals(f.GetValue(this), f.GetValue(obj));
		}

		/// <summary>
		/// Get the properties of the value object.
		/// </summary>
		/// <returns>The properties of the value object.</returns>
		private IEnumerable<PropertyInfo> GetProperties()
		{
			if (this._properties == null)
			{
				this._properties = GetType()
					.GetProperties(BindingFlags.Instance | BindingFlags.Public)
					.Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
					.ToList();

				// Not available in Core
				// !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute))).ToList();
			}

			return this._properties;
		}

		/// <summary>
		/// Get the fields of the value object.
		/// </summary>
		/// <returns>The fields of the value object.</returns>
		private IEnumerable<FieldInfo> GetFields()
		{
			if (this._fields == null)
			{
				this._fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
					.ToList();
			}

			return this._fields;
		}

		/// <summary>
		/// Get the hash code of the value object.
		/// </summary>
		/// <param name="seed">The seed for the hash code.</param>
		/// <param name="value">The value to get the hash code of.</param>
		/// <returns>The hash code of the value object.</returns>
		private int HashValue(int seed, object value)
		{
			var currentHash = value?.GetHashCode() ?? 0;

			return (seed * 23) + currentHash;
		}
	}
}
