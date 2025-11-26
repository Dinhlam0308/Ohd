using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.Services;

public class ReminderWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReminderWorker> _logger;

    public ReminderWorker(IServiceScopeFactory scopeFactory, ILogger<ReminderWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("⏳ Reminder Worker Started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<OhdDbContext>();
                var mail = scope.ServiceProvider.GetRequiredService<EmailService>();

                var now = DateTime.UtcNow;

                var requests = await db.requests
                    .Include(r => r.Severity)
                    .Where(r => r.StatusId != 5) // 5 = Closed
                    .ToListAsync();

                foreach (var req in requests)
                {
                    var sev = req.Severity;

                    var firstReminderTime = req.CreatedAt.AddMinutes(sev.FirstReminderMinutes );
                    
                    // ===== Reminder lần đầu =====
                    if (!req.FirstReminderSent && now >= firstReminderTime)
                    {
                        await mail.SendSeverityReminderAsync(req);
                        req.FirstReminderSent = true;
                        req.LastReminderAt = now;

                        _logger.LogInformation($"📧 First reminder sent for Request #{req.Id}");
                        continue;
                    }

                    // ===== Reminder lặp lại =====
                    if (req.FirstReminderSent && req.LastReminderAt.HasValue)
                    {
                        var nextTime = req.LastReminderAt.Value.AddMinutes(sev.RepeatReminderMinutes);

                        if (now >= nextTime)
                        {
                            await mail.SendSeverityReminderAsync(req);
                            req.LastReminderAt = now;

                            _logger.LogInformation($"🔁 Repeat reminder sent for Request #{req.Id}");
                        }
                    }
                }

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reminder Worker Error");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
