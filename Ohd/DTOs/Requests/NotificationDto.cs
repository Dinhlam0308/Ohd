namespace Ohd.DTOs.Requests
{
    public class NotificationCreateDto
    {
        public long RequestId { get; set; }
        public long SentToUserId { get; set; }
        public string Message { get; set; } = null!;
    }
}