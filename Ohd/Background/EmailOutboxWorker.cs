using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ohd.Data;
using Ohd.Utils;

namespace Ohd.Background
{
    public class EmailOutboxWorker : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<EmailOutboxWorker> _logger;

        public EmailOutboxWorker(IServiceProvider services, ILogger<EmailOutboxWorker> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("📨 Email Outbox Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<OhdDbContext>();
                    var mail = scope.ServiceProvider.GetRequiredService<MailSender>();

                    var pending = await db.outbox_messages
                        .Where(x => x.status == "Pending" && x.attempts < 3)
                        .OrderBy(x => x.created_at)
                        .Take(5)
                        .ToListAsync(stoppingToken);

                    foreach (var msg in pending)
                    {
                        bool sent = await mail.SendEmailAsync(
                            msg.recipient_email,
                            msg.subject,
                            msg.body_html
                        );

                        msg.attempts++;
                        msg.last_attempt_at = DateTime.UtcNow;
                        msg.status = sent ? "Sent" : "Failed";

                        _logger.LogInformation($"📧 Email to {msg.recipient_email}: {msg.status}");
                    }

                    await db.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Email worker crashed");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
