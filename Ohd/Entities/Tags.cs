namespace Ohd.Entities
{
    public class Tags
    {
        public int id { get; set; }
        public string code { get; set; } = null!;
        public string name { get; set; } = null!;
        public DateTime created_at { get; set; }
    }
}