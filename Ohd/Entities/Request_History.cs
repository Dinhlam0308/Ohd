namespace Ohd.Entities
{
    public class request_history
    {
        public long id { get; set; }
        public long request_id { get; set; }
        public int? from_status_id { get; set; }
        public int to_status_id { get; set; }
        public long changed_by_user_id { get; set; }
        public string? remarks { get; set; }
        public DateTime changed_at { get; set; }
    }
}