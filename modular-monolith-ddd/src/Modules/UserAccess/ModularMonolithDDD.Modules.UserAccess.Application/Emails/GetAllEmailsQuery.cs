namespace ModularMonolithDDD.Modules.UserAccess.Application.Emails
{
    /// <summary>
    /// Query class for retrieving all email messages from the system.
    /// This query implements the CQRS pattern and is used to fetch a complete list
    /// of all email messages stored in the database, ordered by date in descending order.
    /// </summary>
    public class GetAllEmailsQuery : QueryBase<List<EmailDto>>
    {
        // This query does not require any additional parameters as it retrieves all emails
        // The base QueryBase class provides the necessary infrastructure for query execution
    }
}