namespace ModularMonolithDDD.BuildingBlocks.Application.Queries
{
    /// <summary>
    /// Standardized Pagination – Consistent pagination for all queries.
    /// Optional Parameters – Page and PerPage are nullable with defaults.
    /// Flexible Implementation – Query classes may implement this interface.
    /// </summary>
    public interface IPagedQuery
    {
        /// <summary>
        /// Page number. If null then default is 1.
        /// </summary>
        int? Page { get; }

        /// <summary>
        /// Records number per page (page size).
        /// </summary>
        int? PerPage { get; }
    }
}