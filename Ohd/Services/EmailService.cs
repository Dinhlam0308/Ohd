using System.Net;
using System.Net.Mail;
using Ohd.Entities;

namespace Ohd.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendSeverityReminderAsync(Request req)
        {
            string subject = $"[Reminder] Request #{req.Id} – Severity {req.Severity.name}";
            
            string body = $@"
                <h2>Request #{req.Id} – {req.Title}</h2>
                <p><b>Severity:</b> {req.Severity.name}</p>
                <p><b>Description:</b> {req.Description}</p>
                <p>This request is <b>pending</b> and requires your attention.</p>
                <br/>
                <p>Thank you,<br/>Online Help Desk System</p>
            ";

            await SendEmailAsync(req.RequesterEmail, subject, body);
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var email = _config["Email:Address"];
            var password = _config["Email:Password"];
            var host = _config["Email:Smtp"];
            var port = int.Parse(_config["Email:Port"]!);

            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true
            };

            var message = new MailMessage(email, to, subject, htmlBody);
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);
        }
    }
}