namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.Emails
{
    /// <summary>
    /// Infrastructure implementation of the email sender service that stores email messages
    /// in the database for later processing. This implementation follows the outbox pattern
    /// for email delivery, ensuring reliable email processing in the modular monolith architecture.
    /// </summary>
	public class EmailSender : IEmailSender
	{
		private readonly ILogger _logger;

		private readonly EmailsConfiguration _configuration;

		private readonly ISqlConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the EmailSender class.
        /// This constructor sets up the dependencies required for email processing,
        /// including logging, configuration, and database access.
        /// </summary>
        /// <param name="logger">The logger instance used for email operation logging</param>
        /// <param name="configuration">The email configuration containing sender settings</param>
        /// <param name="sqlConnectionFactory">The database connection factory for storing email messages</param>		
        public EmailSender(
			ILogger logger,
			EmailsConfiguration configuration,
			ISqlConnectionFactory sqlConnectionFactory)
		{
			_logger = logger;
			_configuration = configuration;
			_sqlConnectionFactory = sqlConnectionFactory;           
        }

        /// <summary>
        /// Sends an email message by storing it in the database for later processing.
        /// This method implements the outbox pattern for email delivery, ensuring that
        /// email messages are reliably stored and can be processed asynchronously.
        /// </summary>
        /// <param name="message">The email message to be sent</param>
        /// <returns>A task representing the asynchronous email sending operation</returns>
		public async Task SendEmail(EmailMessage message)
		{
            try
            {
                var sqlConnection = _sqlConnectionFactory.GetOpenConnection();
                var sql = @"
                INSERT INTO [app].[Emails] ([Id], [From], [To], [Subject], [Content], [Date]) 
                VALUES (@Id, @From, @To, @Subject, @Content, @Date) ";

                var result = await sqlConnection.ExecuteAsync(sql, new
                {
                    Id = Guid.NewGuid(),
                    From = _configuration.FromEmail,
                    message.To,
                    message.Subject,
                    message.Content,
                    Date = DateTime.UtcNow
                });
                Console.WriteLine($"=================================================");
                Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Is Background: {Thread.CurrentThread.IsBackground}");
                Console.WriteLine($"Thread Name: {Thread.CurrentThread.Name ?? "Unnamed"}");
                Console.WriteLine($"Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
                _logger.Error($"=================================================");
                _logger.Error($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
               _logger.Error($"Is Background: {Thread.CurrentThread.IsBackground}");
               _logger.Error($"Thread Name: {Thread.CurrentThread.Name ?? "Unnamed"}");
               _logger.Error($"Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
                if (result > 0)
                {
                    await SendEmailViaMailGun(message);
                    _logger.Information(
                    "Email sent via Mailjet SUCCESSFULLY. From: {From}, To: {To}, Subject: {Subject}.",
                    _configuration.FromEmail,
                    message.To,
                    message.Subject,
                    message.Content);
                }
                else
                {
                    _logger.Information(
                    "Email sent via Mailjet FAILED. From: {From}, To: {To}, Subject: {Subject}.",
                    _configuration.FromEmail,
                    message.To,
                    message.Subject,
                    message.Content);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error when inserting email log");                
            }			           
		}

		//private async Task SendEmailViaMailjet(EmailMessage message)
		//{
  //          var email = new MimeMessage();
  //          email.From.Add(MailboxAddress.Parse(_configuration.FromEmail));
  //          email.To.Add(MailboxAddress.Parse(message.To));
  //          email.Subject = message.Subject;
  //          email.Body = new TextPart("html") { Text = message.Content };

  //          using var smtp = new SmtpClient();
  //          var tlsPort = int.TryParse(_configuration.TLSPort, out int port) ? port : 587;
  //          await smtp.ConnectAsync(_configuration.SMTPServer, tlsPort, MailKit.Security.SecureSocketOptions.StartTls);
  //          await smtp.AuthenticateAsync(_configuration.ApiKey, _configuration.SecretKey);
  //          var result = await smtp.SendAsync(email);
  //          Console.WriteLine(result);
  //          _logger.Information($"[Mailjet] Received {result}");
  //          await smtp.DisconnectAsync(true);
  //      }

		private async Task SendEmailViaMailGun(EmailMessage message)
		{
            var request = new HttpRequestMessage(HttpMethod.Post,
            $"https://api.mailgun.net/v3/{_configuration.Domain}/messages");

            var authToken = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"api:{_configuration.ApiKey}"));

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var formContent = new Dictionary<string, string>
            {
                { "from", _configuration.FromEmail },
                { "to", message.To },
                { "subject", message.Subject },
                { "html", message.Content }
            };

            request.Content = new FormUrlEncodedContent(formContent);
            var _httpClient = new HttpClient();
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.Error("Failed to send email via Mailgun: {Error}", error);
                throw new Exception($"Mailgun send failed: {error}");
            }
        }
    }
}
