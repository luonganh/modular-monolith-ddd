namespace ModularMonolithDDD.Modules.UserAccess.Application.Emails
{
    /// <summary>
    /// Query handler for retrieving all email messages from the database.
    /// This handler implements the CQRS pattern and is responsible for executing
    /// the GetAllEmailsQuery by querying the database and returning a list of email DTOs.
    /// </summary>
    internal class GetAllEmailsQueryHandler : IQueryHandler<GetAllEmailsQuery, List<EmailDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the GetAllEmailsQueryHandler class.
        /// This constructor sets up the database connection factory dependency
        /// required for executing database queries.
        /// </summary>
        /// <param name="sqlConnectionFactory">The factory for creating database connections</param>
        public GetAllEmailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        /// <summary>
        /// Handles the GetAllEmailsQuery by executing a database query to retrieve all email messages.
        /// This method performs a SQL query against the Emails table and returns the results
        /// ordered by date in descending order (most recent emails first).
        /// </summary>
        /// <param name="query">The query object containing the request parameters</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation</param>
        /// <returns>A task that represents the asynchronous operation and contains a list of EmailDto objects</returns>
        public async Task<List<EmailDto>> Handle(GetAllEmailsQuery query, CancellationToken cancellationToken)
        {
            // Get an open database connection from the connection factory
            var connection = _sqlConnectionFactory.GetOpenConnection();

            // SQL query to retrieve all email records with proper column mapping
            const string sql = $"""
                       SELECT 
                           [Email].[Id] AS [{nameof(EmailDto.Id)}], 
                           [Email].[From] AS [{nameof(EmailDto.From)}], 
                           [Email].[To] AS [{nameof(EmailDto.To)}],
                           [Email].[Subject] AS [{nameof(EmailDto.Subject)}],
                           [Email].[Content] AS [{nameof(EmailDto.Content)}],
                           [Email].[Date] AS [{nameof(EmailDto.Date)}] 
                       FROM [app].[Emails] AS [Email] 
                       ORDER BY [Email].[Date] DESC
                       """;
            
            // Execute the query and map results to EmailDto objects
            var result = await connection.QueryAsync<EmailDto>(sql);
            return result.AsList();
        }
    }
}