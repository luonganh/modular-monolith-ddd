namespace ModularMonolithDDD.BuildingBlocks.Infrastructure
{
    /// <summary>
    /// Custom value converter selector for Entity Framework Core that automatically
    /// selects appropriate value converters for strongly typed ID values. This selector
    /// extends the default EF Core value converter selection to handle strongly typed
    /// IDs by converting them to/from their underlying primitive types (typically Guid).
    /// 
    /// Based on: https://andrewlock.net/strongly-typed-ids-in-ef-core-using-strongly-typed-entity-ids-to-avoid-primitive-obsession-part-4/
    /// </summary>
    public class StronglyTypedIdValueConverterSelector : ValueConverterSelector
    {
        /// <summary>
        /// Thread-safe cache of value converter instances to avoid creating duplicate
        /// converters for the same type combinations. This improves performance by
        /// reusing converter instances across multiple entity configurations.
        /// </summary>
        private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
            = new ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo>();

        /// <summary>
        /// Initializes a new instance of the StronglyTypedIdValueConverterSelector class.
        /// This constructor sets up the value converter selector with the required
        /// dependencies from Entity Framework Core.
        /// </summary>
        /// <param name="dependencies">The dependencies required by the value converter selector</param>
        public StronglyTypedIdValueConverterSelector(ValueConverterSelectorDependencies dependencies)
            : base(dependencies)
        {
        }

        /// <summary>
        /// Selects appropriate value converters for the specified model and provider types.
        /// This method first returns any base converters, then checks if the model type
        /// is a strongly typed ID and creates a custom converter if needed.
        /// </summary>
        /// <param name="modelClrType">The CLR type of the model property</param>
        /// <param name="providerClrType">The CLR type of the database provider (optional)</param>
        /// <returns>An enumerable of value converter information for the specified types</returns>
        public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type providerClrType = null)
        {
            var baseConverters = base.Select(modelClrType, providerClrType);
            foreach (var converter in baseConverters)
            {
                yield return converter;
            }

            var underlyingModelType = UnwrapNullableType(modelClrType);
            var underlyingProviderType = UnwrapNullableType(providerClrType);

            if (underlyingProviderType is null || underlyingProviderType == typeof(Guid))
            {
                var isTypedIdValue = typeof(TypedIdValueBase).IsAssignableFrom(underlyingModelType);
                if (isTypedIdValue)
                {
                    var converterType = typeof(TypedIdValueConverter<>).MakeGenericType(underlyingModelType);

                    yield return _converters.GetOrAdd((underlyingModelType, typeof(Guid)), _ =>
                    {
                        return new ValueConverterInfo(
                            modelClrType: modelClrType,
                            providerClrType: typeof(Guid),
                            factory: valueConverterInfo => (ValueConverter)Activator.CreateInstance(converterType, valueConverterInfo.MappingHints));
                    });
                }
            }
        }

        /// <summary>
        /// Unwraps nullable types to get their underlying non-nullable type.
        /// This helper method is used to handle both nullable and non-nullable
        /// strongly typed IDs consistently.
        /// </summary>
        /// <param name="type">The type to unwrap</param>
        /// <returns>The underlying non-nullable type, or the original type if it's not nullable</returns>
        private static Type UnwrapNullableType(Type type)
        {
            if (type is null)
            {
                return null;
            }

            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}