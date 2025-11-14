namespace Ohd.Entities
{
    public class audit_logs
    {
        public long id { get; set; }
        public long? actor_user_id { get; set; }
        public string action { get; set; } = null!;
        public string entity { get; set; } = null!;
        public long entity_id { get; set; }
        public string? before_json { get; set; }
        public string? after_json { get; set; }
        public string? ip { get; set; }
        public string? user_agent { get; set; }
        public DateTime created_at { get; set; }
    }
}