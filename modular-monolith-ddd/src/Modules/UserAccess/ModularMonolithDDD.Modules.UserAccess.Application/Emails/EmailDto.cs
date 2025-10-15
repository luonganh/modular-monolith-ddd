namespace ModularMonolithDDD.Modules.UserAccess.Application.Emails
{
    /// <summary>
    /// Data Transfer Object (DTO) representing an email message.
    /// This class is used to transfer email data between different layers of the application,
    /// providing a clean contract for email information without exposing internal domain logic.
    /// </summary>
    public class EmailDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the email message.
        /// This property represents the primary key for the email record in the database.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sender's email address.
        /// This property contains the email address of the person or system that sent the email.
        /// </summary>
        public string From { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the recipient's email address.
        /// This property contains the email address of the person or system that received the email.
        /// </summary>
        public string To { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subject line of the email message.
        /// This property contains the brief description or title of the email content.
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the body content of the email message.
        /// This property contains the main text content of the email, which can be in plain text or HTML format.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the email was sent.
        /// This property represents the timestamp of when the email message was created or sent.
        /// </summary>
        public DateTime Date { get; set; }
    }
}