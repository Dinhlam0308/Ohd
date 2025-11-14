namespace Ohd.Entities
{
    public class RateLimitBucket
    {
        public int id { get; set; }
        public string bucket_key { get; set; } = null!;
        public int counter { get; set; }
        public DateTime reset_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}