using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Ohd.Utils
{
    public class MailSender
    {
        private readonly IConfiguration _config;

        public MailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string htmlBody)
        {
            try
            {
                var host = _config["Mail:Host"];
                var port = int.Parse(_config["Mail:Port"]!);
                var username = _config["Mail:Username"];
                var password = _config["Mail:Password"];
                var enableSsl = bool.Parse(_config["Mail:EnableSsl"]!);

                var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl
                };

                var msg = new MailMessage
                {
                    From = new MailAddress(username!),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                msg.To.Add(to);

                await client.SendMailAsync(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}