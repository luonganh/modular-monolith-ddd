namespace ModularMonolithDDD.BuildingBlocks.Application.Queries
{
    /// <summary>
    /// Paged query helper.
    /// </summary>
    public static class PagedQueryHelper
    {
        public const string Offset = "Offset";

        public const string Next = "Next";

        /// <summary>
        /// Get the page data.
        /// Pagination Calculation - Translate Page/PerPage into OFFSET and FETCH in SQL.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The page data.</returns>
        public static PageData GetPageData(IPagedQuery query)
        {
            int offset;
            if (!query.Page.HasValue ||
                !query.PerPage.HasValue)
            {
                offset = 0;
            }
            else
            {
                offset = (query.Page.Value - 1) * query.PerPage.Value;
            }

            int next;
            if (!query.PerPage.HasValue)
            {
                next = int.MaxValue;
            }
            else
            {
                next = query.PerPage.Value;
            }

            return new PageData(offset, next);
        }

        /// <summary>
        /// Append the page statement.
        /// SQL Generation - Append OFFSET and FETCH to the SQL query.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>The SQL.</returns>
        public static string AppendPageStatement(string sql)
        {
            return $"{sql} " +
                   $"OFFSET @{Offset} ROWS FETCH NEXT @{Next} ROWS ONLY; ";
        }
    }
}