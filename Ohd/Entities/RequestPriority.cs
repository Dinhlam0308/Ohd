namespace Ohd.Entities
{
    public class RequestPriority
    {
        public int id { get; set; }
        public string code { get; set; } = null!;
        public string name { get; set; } = null!;
        public int sort_order { get; set; }
        public DateTime created_at { get; set; }
    }
}