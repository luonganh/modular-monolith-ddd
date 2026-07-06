namespace ModularMonolithDDD.Modules.UserAccess.Application.Configuration.Queries
{
    /// <summary>
    /// Interface for query handlers that process queries and return results.
    /// Implements the Command Query Responsibility Segregation (CQRS) pattern.
    /// </summary>
    /// <typeparam name="TQuery">The type of query to handle</typeparam>
    /// <typeparam name="TResult">The type of result returned by the query</typeparam>
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}