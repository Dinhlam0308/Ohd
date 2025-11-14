namespace Ohd.Entities
{
    public class Teams
    {
        public int id { get; set; }
        public string team_name { get; set; } = null!;
        public string? description { get; set; }
        public DateTime created_at { get; set; }
    }
}