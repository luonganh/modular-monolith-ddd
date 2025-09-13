namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.Emails
{
	public class EmailsConfiguration
	{
        public EmailsConfiguration(string fromEmail, string apiKey, string? domain, string? secretKey, string? smtpServer, string? sslPort, string? tlsPort)
        {
            FromEmail = fromEmail;
            ApiKey = apiKey;
            Domain = domain;
            SecretKey = secretKey;
            SMTPServer = smtpServer;
            SSLPort = sslPort;
            TLSPort = tlsPort;
        }

        public string FromEmail { get; }
        public string ApiKey { get; }
        public string? SecretKey { get; }
        public string? Domain { get; }
        public string? SMTPServer { get; }
        public string? SSLPort { get; }
        public string? TLSPort { get; }
    }
}
