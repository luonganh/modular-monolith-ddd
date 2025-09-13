namespace ModularMonolithDDD.BuildingBlocks.Infrastructure
{
    /// <summary>
    /// Generic value converter for Entity Framework Core that converts between strongly typed ID values
    /// and their underlying Guid values. This converter enables strongly typed IDs to be stored
    /// in the database as primitive Guid values while maintaining type safety in the application.
    /// </summary>
    /// <typeparam name="TTypedIdValue">The strongly typed ID type that inherits from TypedIdValueBase</typeparam>
    public class TypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, Guid>
        where TTypedIdValue : TypedIdValueBase
    {
        /// <summary>
        /// Initializes a new instance of the TypedIdValueConverter class.
        /// This constructor sets up the bidirectional conversion between the strongly typed ID
        /// and its underlying Guid value using lambda expressions for conversion functions.
        /// </summary>
        /// <param name="mappingHints">Optional mapping hints for the converter</param>
        public TypedIdValueConverter(ConverterMappingHints mappingHints = null)
            : base(id => id.Value, value => Create(value), mappingHints)
        {
        }

        /// <summary>
        /// Creates a new instance of the strongly typed ID from a Guid value.
        /// This method uses reflection to instantiate the strongly typed ID with the
        /// provided Guid value, enabling conversion from database values to domain objects.
        /// </summary>
        /// <param name="id">The Guid value to create the strongly typed ID from</param>
        /// <returns>A new instance of the strongly typed ID with the specified Guid value</returns>
        private static TTypedIdValue Create(Guid id) => Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue;
    }
}