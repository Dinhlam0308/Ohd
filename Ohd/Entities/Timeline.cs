namespace Ohd.Entities
{
    public class Timeline
    {
        public long Id { get; set; }
        public long request_id { get; set; }
        public string title { get; set; }
        public string? note { get; set; }
        public DateTime created_at { get; set; }
        public string? user_name { get; set; }
        public bool is_internal { get; set; }

    }
} 