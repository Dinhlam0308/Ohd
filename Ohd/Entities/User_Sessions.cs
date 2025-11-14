namespace Ohd.Entities
{
    public class user_sessions
    {
        public long id { get; set; }
        public long user_id { get; set; }
        public string jwt { get; set; } = null!;
        public string? ip { get; set; }
        public string? user_agent { get; set; }
        public DateTime last_seen_at { get; set; }
        public DateTime? revoked_at { get; set; }
        public DateTime created_at { get; set; }
    }
}