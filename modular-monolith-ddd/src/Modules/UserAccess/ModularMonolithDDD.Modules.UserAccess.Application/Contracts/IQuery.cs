namespace ModularMonolithDDD.Modules.UserAccess.Application.Contracts
{
    /// <summary>
    /// Interface for queries that return data.
    /// Represents a request to retrieve information without modifying state.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the query</typeparam>
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
