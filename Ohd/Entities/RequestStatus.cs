namespace Ohd.Entities
{
    public class RequestStatus
    {
        public int id { get; set; }
        public string code { get; set; } = null!;
        public string name { get; set; } = null!;
        public bool is_final { get; set; }
        public DateTime created_at { get; set; }
    }
}