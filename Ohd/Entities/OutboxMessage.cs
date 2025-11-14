namespace Ohd.Entities
{
    public class OutboxMessage
    {
        public long id { get; set; }
        public string recipient_email { get; set; } = null!;
        public string subject { get; set; } = null!;
        public string body_html { get; set; } = null!;
        public string status { get; set; } = null!; // 'Pending' | 'Sent' | 'Failed'
        public int attempts { get; set; }
        public DateTime? last_attempt_at { get; set; }
        public DateTime created_at { get; set; }
    }
}