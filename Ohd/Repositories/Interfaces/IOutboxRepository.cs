namespace Ohd.Repositories.Interfaces
{
    public interface IOutboxRepository
    {
        Task QueueEmailAsync(string toEmail, string subject, string bodyHtml);
    }
}