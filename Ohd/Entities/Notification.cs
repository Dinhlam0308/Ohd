namespace Ohd.Entities
{
    public class Notification
    {
        public long id { get; set; }
        public long request_id { get; set; }
        public long sent_to_user_id { get; set; }
        public string message { get; set; } = null!;
        public bool is_read { get; set; }
        public DateTime created_at { get; set; }
    }
}