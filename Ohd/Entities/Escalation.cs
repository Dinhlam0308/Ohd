namespace Ohd.Entities
{
    public class Escalation
    {
        public long id { get; set; }
        public long request_id { get; set; }
        public DateTime triggered_at { get; set; }
        public int level { get; set; }
        public long? notified_to_user_id { get; set; }
        public DateTime? resolved_at { get; set; }
        public string? remarks { get; set; }
    }
}