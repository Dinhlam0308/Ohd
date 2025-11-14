namespace Ohd.Entities
{
    public class ip_blocklist
    {
        public int id { get; set; }
        public string ip { get; set; } = null!;
        public string? reason { get; set; }
        public DateTime blocked_at { get; set; }
        public DateTime? expires_at { get; set; }
    }
}