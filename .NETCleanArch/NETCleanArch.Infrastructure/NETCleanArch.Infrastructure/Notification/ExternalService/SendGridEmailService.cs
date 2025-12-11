using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NETCleanArchInfrastructure.Notifications.IExternalService;

namespace NETCleanArchInfrastructure.Notifications.ExternalService
{
    public class SendGridEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;

        public SendGridEmailService(IConfiguration configuration, ISendGridClient sendGridClient)
        {
            _configuration = configuration;
            _sendGridClient = sendGridClient;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var fromEmail = _configuration["SendGrid:FromEmail"] ?? "noreply@NETCleanArchcom";
            var fromName = _configuration["SendGrid:FromName"] ?? "VMS System";

            var msg = new SendGridMessage
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = body
            };

            msg.AddTo(new EmailAddress(to));

            var response = await _sendGridClient.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to send email: {response.StatusCode}");
            }
        }
    }
}
