namespace ModularMonolithDDD.Modules.UserAccess.Application.Contracts
{
    /// <summary>
    /// Base class for queries that return data.
    /// Provides common functionality for all query implementations.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the query</typeparam>
    public abstract class QueryBase<TResult> : IQuery<TResult>
    {
        /// <summary>
        /// Gets the unique identifier for the query.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Initializes a new instance of the QueryBase class with a new GUID.
        /// </summary>
        protected QueryBase()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the QueryBase class with a specific ID.
        /// </summary>
        /// <param name="id">The unique identifier for the query</param>
        protected QueryBase(Guid id)
        {
            Id = id;
        }
    }
}