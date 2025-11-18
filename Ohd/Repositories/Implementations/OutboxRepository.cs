using Ohd.Data;
using Ohd.Entities;
using Ohd.Repositories.Interfaces;

namespace Ohd.Repositories.Implementations
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly OhdDbContext _context;

        public OutboxRepository(OhdDbContext context)
        {
            _context = context;
        }

        public async Task QueueEmailAsync(string toEmail, string subject, string bodyHtml)
        {
            var msg = new OutboxMessage
            {
                recipient_email = toEmail,
                subject = subject,
                body_html = bodyHtml,
                status = "Pending",   // ENUM('Pending','Sent','Failed')
                attempts = 0,
                last_attempt_at = null,
                created_at = DateTime.UtcNow
            };

            await _context.outbox_messages.AddAsync(msg);
            await _context.SaveChangesAsync();
        }
    }
}