using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;

namespace UF5423_SuperShop.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Response SendEmail(string to, string subject, string body) // Get mail parameters from 'appsettings.js'.
        {
            var nameFrom = _configuration["Mail:NameFrom"]; // Sender name.
            var from = _configuration["Mail:From"]; // Sender email address.
            var smtp = _configuration["Mail:Smtp"]; // Sender protocol.
            var port = _configuration["Mail:Port"];
            var password = _configuration["Mail:Password"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), false); // Connect to mail server.
                    client.Authenticate(from, password); // Authenticate credentials.
                    client.Send(message); // Send email.
                    client.Disconnect(true); // Disconnect from mail server.
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccessful = false,
                    Message = ex.ToString()
                };
            }

            return new Response
            {
                IsSuccessful = true,
            };
        }
    }
}
